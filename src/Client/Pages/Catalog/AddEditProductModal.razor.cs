using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Products.Commands.AddEdit;
using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Application.Features.RecipeItems.Commands.AddEdit;
using ArdaManager.Application.Features.RecipeItems.Commands.Delete;
using ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using ArdaManager.Application.Requests;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Product;
using ArdaManager.Client.Infrastructure.Managers.Catalog.RecipeItem;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Warehouse;
using ArdaManager.Client.Infrastructure.Managers.Misc.Category;
using ArdaManager.Domain.Enums;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ArdaManager.Application.Extensions;

namespace ArdaManager.Client.Pages.Catalog
{
    public partial class AddEditProductModal
    {

        private IEnumerable<GetAllPagedProductsResponse> _pagedData;
        private IEnumerable<GetAllPagedProductsResponse> _templatesData;
        private List<GetRecipeItemsByOwnerProductIdResponse> _recipeItems;
        private List<GetAllPatternsResponse> _patterns = new();
        private List<GetCategoriesByTypeResponse> _categories = new();
        private IEnumerable<GetAllWarehousesResponse> _warehouses;
        private IEnumerable<GetAllMeasurementUnitsResponse> _measurementUnits;
        //private IList<GetRecipeItemsByOwnerProductIdResponse> _recipeItems;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private int templateId;

        [Inject] private IProductManager ProductManager { get; set; }
        [Inject] private ICategoryManager CategoryManager { get; set; }
        [Inject] private IPatternManager PatternManager { get; set; }
        [Inject] private IRecipeItemManager RecipeItemManager { get; set; }
        [Inject] private IWarehouseManager WarehouseManager { get; set; }
        [Inject] private IMeasurementUnitManager MeasurementUnitManager { get; set; }

        private IJSRuntime JsRuntime;

        [Parameter] public AddEditProductCommand AddEditProductModel { get; set; } = new();

        private AddEditRecipeItemCommand AddEditRecipeItemModel { get; set; } = new();
        private DeleteRecipeItemCommand DeleteRecipeItemModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        //private List<GetCategoriesByTypeResponse> _productCategories = new();
        

        private bool hideTemplate = true;
        private bool isTemplate = false;
        private bool hideRecipe = true;
        private bool isBasicProduct = false;
        private bool saveAndShowRecipe = false;
        //private bool loading = false;
        private MudTabs recipeTabs;
        private MudButton saveAndShowRecipeButton;
        private RadzenDropDownDataGrid<int> ddgWarehouses;
        private MudTable<GetRecipeItemsByOwnerProductIdResponse> _recipeItemsTable;



        private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.End;
        private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.End;
        private TableEditTrigger editTrigger = TableEditTrigger.RowClick;

        private AddEditRecipeItemCommand backupRecipeItemRow = null;


        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            if (templateId != 0)
                AddEditProductModel.TemplateId = templateId;

            var response = await ProductManager.SaveAsync(AddEditProductModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("dialog init");
            //loading = true;
            await LoadDataAsync();
            TypeChangedEvent();
            
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
            //loading= false;
            Console.WriteLine("dialog loaded");
        }

        private async Task LoadDataAsync()
        {
            if (AddEditProductModel.TemplateId != null)
            {
                int.TryParse(AddEditProductModel.TemplateId.ToString(), out templateId);
                //hideRecipe = false;

            }
            isTemplate = (AddEditProductModel.Type == ProductType.Template) ? true : false;
            if (AddEditProductModel.Id > 0 && (AddEditProductModel.Type == ProductType.Producible || AddEditProductModel.Type == ProductType.Basic))
                hideRecipe = false;
            else
                hideRecipe = true;

            hideTemplate = ((AddEditProductModel.Type != ProductType.Template));
            isBasicProduct = (AddEditProductModel.Type == ProductType.Basic);

            await LoadImageAsync();
            await LoadPatternsAsync();

            Console.WriteLine("patterns loaded");
            await LoadCategoriesAsync();
            await LoadMeasurementUnitsAsync();
            await LoadWarehousesData();
            await LoadReciItemsData();
            Console.WriteLine("recipe loaded");
            await LoadTemplates(1,100000);

            Console.WriteLine("product templates loaded");
            await LoadProductsForRecipeData(1, 10000000);

            Console.WriteLine("productrecies loaded");

        }

        private async Task LoadCategoriesAsync()
        {
            GetCategoriesByTypeQuery query = new GetCategoriesByTypeQuery() { Type = (int)CategoryType.Product };
            var data = await CategoryManager.GetByTypeAsync(query);
            //var data = await CategoryManager.GetAllAsync();

            if (data.Succeeded)
            {
                _categories = data.Data;
            }
        }
        private async Task LoadPatternsAsync()
        {
            var data = await PatternManager.GetAllAsync();
            if (data.Succeeded)
            {
                _patterns = data.Data;
            }
        }
        private async Task LoadImageAsync()
        {
            var data = await ProductManager.GetProductImageAsync(AddEditProductModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditProductModel.ImageDataURL = imageData; 
                }
                /*
                else
                {
                    AddEditProductModel.ImageDataURL = "Files\\Images\\Static\\placeholder-image.png";
                }*/
            }
        }
        private async Task LoadReciItemsData()
        {
            var request = new GetRecipeItemsByOwnerProductIdQuery { OwnerProductId = AddEditProductModel.Id };
            var response = await RecipeItemManager.GetByOwnerProductIdAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.Data.Count;
                //_currentPage = response.CurrentPage;
                _recipeItems = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadWarehousesData()
        {
            var response = await WarehouseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _warehouses = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadProductsForRecipeData(int pageNumber, int pageSize)
        {
            string[] orderings = null;


            //var recipeHasTemplate = (_recipeItems.Where(x => x.RecipeItemProductType == ProductType.Template).Count() >= 1) ? true : false;

            var request = new GetAllPagedProductsRequest { PageSize = pageSize, PageNumber = pageNumber, SearchString = _searchString, Orderby = orderings, isTemplate = false };
            var response = await ProductManager.GetProductsAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task LoadTemplates(int pageNumber, int pageSize)
        {
            

            var request = new GetAllPagedProductsRequest { PageSize = pageSize, PageNumber = pageNumber, SearchString = _searchString, Orderby = null, isTemplate = true };
            var response = await ProductManager.GetProductsAsync(request);
            if (response.Succeeded)
            {
                _templatesData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        
        private async Task LoadMeasurementUnitsAsync()
        {
            var data = await MeasurementUnitManager.GetAllAsync();
            if (data.Succeeded)
            {
                _measurementUnits = data.Data;
            }
        }
        

        private void DeleteAsync()
        {
            AddEditProductModel.ImageDataURL = null;
            AddEditProductModel.UploadRequest = new UploadRequest();
        }

        private IBrowserFile _file;

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditProductModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditProductModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }


        private async Task<IEnumerable<int>> SearchPatterns(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _patterns.Select(x => x.Id);

            return _patterns.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        
        
        private async Task<IEnumerable<int>> SearchMeasurementUnits(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _measurementUnits.Select(x => x.Id);

            return _measurementUnits.Where(x => x.Code.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        private async Task<IEnumerable<int>> SearchMeasurementUnitsWE(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _measurementUnits.Where(x=>x.System == "WE").Select(x => x.Id);

            return _measurementUnits.Where(x => x.Code.Contains(value, StringComparison.InvariantCultureIgnoreCase) && x.System == "WE")
                .Select(x => x.Id);
        }
        private async Task<IEnumerable<int>> SearchMeasurementUnitsDI(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _measurementUnits.Where(x => x.System == "DI").Select(x => x.Id);

            return _measurementUnits.Where(x => x.Code.Contains(value, StringComparison.InvariantCultureIgnoreCase) && x.System == "DI")
                .Select(x => x.Id);
        }
        private async Task<IEnumerable<int?>> SearchCategories(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _categories.Select(x => x.Id);

            return _categories.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        private void TypeChangedEvent() 
        {
            if (AddEditProductModel.Id > 0 && (AddEditProductModel.Type == ProductType.Producible || AddEditProductModel.Type == ProductType.Basic))
                hideRecipe = false;
            else
                hideRecipe = true;


            hideTemplate = ((AddEditProductModel.Type != ProductType.Template) );

            
            isBasicProduct = (AddEditProductModel.Type == ProductType.Basic);

            //await LoadRecipeItems();
            
        }

        private void KindTypeChangedEvent()
        {


        }

        private async Task SaveRecipeItemAsync(AddEditRecipeItemCommand item)
        {

            var response = await RecipeItemManager.SaveAsync(item);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }

            await ReloadData();
        }
    
        private void BackupItem(object element)
        {
            // Backup
            backupRecipeItemRow = new AddEditRecipeItemCommand()
            {
                OwnerProductId = AddEditProductModel.Id,
                Quantity = ((GetRecipeItemsByOwnerProductIdResponse)element).Quantity,
                RecipeProductId = ((GetRecipeItemsByOwnerProductIdResponse)element).RecipeProductId,
                WarehouseId = ((GetRecipeItemsByOwnerProductIdResponse)element).WarehouseId,
                Description = ((GetRecipeItemsByOwnerProductIdResponse)element).Description,
                Id = ((GetRecipeItemsByOwnerProductIdResponse)element).Id

            };
        }

        private async void ItemHasBeenCommitted(object element)
        {
            AddEditRecipeItemCommand item  = new AddEditRecipeItemCommand() { 
                OwnerProductId = AddEditProductModel.Id,
                Quantity = ((GetRecipeItemsByOwnerProductIdResponse)element).Quantity,
                RecipeProductId = ((GetRecipeItemsByOwnerProductIdResponse)element).RecipeProductId,
                WarehouseId = ((GetRecipeItemsByOwnerProductIdResponse)element).WarehouseId,
                Description = ((GetRecipeItemsByOwnerProductIdResponse)element).Description,
                Id = ((GetRecipeItemsByOwnerProductIdResponse)element).Id
            };
            backupRecipeItemRow = null;
            await SaveRecipeItemAsync(item);
            
            await ReloadData();
        }

        
        private async void ResetItemToOriginalValues(object element)
        {
            await ReloadData();
        }

        private void AddNewRow()
        {


            GetRecipeItemsByOwnerProductIdResponse addedRow = new GetRecipeItemsByOwnerProductIdResponse()
            {
                Id = 0,
                OwnerProductId = AddEditProductModel.Id,
                RecipeProductName = "Düzenlemek için dokunun.",
                WarehouseName = "Düzenlemek için dokunun.",
                Quantity = 0
            };
            _recipeItems.Add(addedRow);

        }

        

        private async Task ReloadData()
        {

            await LoadReciItemsData();
            StateHasChanged();

        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await RecipeItemManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    _snackBar.Add("Reçete Ürünü Kaldırıldı", Severity.Error);
                    await ReloadData();
                }
                else
                {
                    await ReloadData();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }


        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                JsRuntime.InvokeVoidAsync("blazorjs.dragable");

            return base.OnAfterRenderAsync(firstRender);
        }

    }
}
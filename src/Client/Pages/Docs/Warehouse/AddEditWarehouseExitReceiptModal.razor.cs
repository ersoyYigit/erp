using ArdaManager.Application.Features.Docs.WarehouseReceipts.Commands.AddEdit;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetById;
using ArdaManager.Application.Features.Molds.Queries.GetAllPaged;
using ArdaManager.Application.Features.ProductionLines.Queries.GetAll;
using ArdaManager.Application.Features.Templates.Queries.GetAllPaged;
using ArdaManager.Application.Interfaces.Chat;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Requests.Catalog;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Mold;
using ArdaManager.Client.Infrastructure.Managers.Communication;
using ArdaManager.Client.Infrastructure.Managers.Docs.WarehouseReceipt;
using ArdaManager.Client.Infrastructure.Managers.Inventory.ProductionLine;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Product;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Shared.Constants.Application;
using AutoMapper;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Warehouse;
using ArdaManager.Application.Features.Warehouses.Queries.GetAll;
using Microsoft.EntityFrameworkCore;
using Nextended.Core.Extensions;
using Radzen;
using static ArdaManager.Client.Pages.Order.AddEditOrder;
using Radzen.Blazor;
using ArdaManager.Domain.Entities.Transactions.WarehouseDocs;
using Polly;
using ArdaManager.Application.Features.Docs.WarehouseReceipts.Queries.GetAll;
using ArdaManager.Application.Features.Products.Queries.GetAllPaged;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Template;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.Inventory.Rack;
using ArdaManager.Application.Features.Racks.Queries.GetAll;
using static ArdaManager.Shared.Constants.Permission.Permissions;
using ArdaManager.Application.Features.RecipeItems.Queries.GetByOwnerProductId;
using ArdaManager.Application.Features.RecipeItems.Commands.AddEdit;
using ArdaManager.Client.Pages.Catalog;
using ArdaManager.Domain.Entities.Catalog;
using ArdaManager.Client.Infrastructure.Managers.Catalog.RecipeItem;
using Microsoft.JSInterop;
using ArdaManager.Client.Pages.Misc;

namespace ArdaManager.Client.Pages.Docs.Warehouse
{
    public partial class AddEditWarehouseExitReceiptModal
    {

        private List<UserResponse> _userList = new();


        [Inject] private IProductManager ProductManager { get; set; }
        [Inject] private IWarehouseReceiptManager WarehouseReceiptManager { get; set; }
        [Inject] private ITemplateManager TemplateManager { get; set; }
        [Inject] private IMoldManager MoldManager { get; set; }
        [Inject] private IProductionLineManager ProductionLineManager { get; set; }
        [Inject] private IChatManager ChatManager { get; set; }
        [Inject] private IWarehouseManager WarehouseManager { get; set; }
        [Inject] private IMeasurementUnitManager MeasurementUnitManager { get; set; }
        [Inject] private IRackManager RackManager { get; set; }


        private IMapper _mapper;
        [Parameter] public AddEditWarehouseReceiptCommand AddEditWarehouseExitReceiptModel { get; set; } = new();
        [Parameter] public int Id { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;



        private GetWarehouseReceiptByIdResponse _warehouseReceipt = new();


        private List<GetAllPagedTemplatesResponse> _templatesData = new();
        private List<GetAllPagedMoldsResponse> _moldsData = new();
        private List<GetAllProductionLinesResponse> _productionLinesData = new();
        private List<GetAllPagedProductsResponse> _productsData = new();
        private List<GetAllMeasurementUnitsResponse> _measurementUnits = new();
        private List<GetAllRacksResponse> _rackList = new();
        private List<GetAllRacksResponse> _rackListAll = new();


        private List<GetAllWarehousesResponse> _warehouses = new();

        private string ownerUserId;
        private string ownerUserName;
        private bool isRework = false;
        private int firstWarehouseId;
        //private string selectedProductionLineCode = AddEditWarehouseReceiptModel.ProductionLine;



        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {

            var response = await WarehouseReceiptManager.SaveAsync(AddEditWarehouseExitReceiptModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }

            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }


        private async Task GetUsersAsync()
        {
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                _userList = response.Data.ToList();
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
            firstWarehouseId = AddEditWarehouseExitReceiptModel.WarehouseId;
            await LoadDataAsync();

            if (AddEditWarehouseExitReceiptModel.Id == 0)
            {
                overrideCurrentUser();
            }



            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task LoadDataAsync()
        {
            isLoading = true;
            //await GetWarehouseReceipt();
            await GetUsersAsync();
            await SetCurrentUser();
            //await LoadTemplates();
            await LoadWarehousesAsync();
            await LoadProductsAsync(1, 10000000);
            await LoadMeasurementUnitsAsync();
            await GetRacksAsync();

            StateHasChanged();
            isLoading = false;
        }

        private async Task SetCurrentUser()
        {

            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            ownerUserId = user.GetUserId();
            ownerUserName = _userList.FirstOrDefault(x => x.Id == ownerUserId).FullName;
            if (AddEditWarehouseExitReceiptModel.Id == 0)
                overrideCurrentUser();

        }
        private void overrideCurrentUser()
        {

        }

        private async Task LoadTemplates()
        {


            var request = new GetAllPagedTemplatesRequest { PageSize = 10000, PageNumber = 1, SearchString = "", Orderby = null };
            var response = await TemplateManager.GetTemplatesAsync(request);
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


        private async Task LoadWarehousesAsync()
        {
            var data = await WarehouseManager.GetAllAsync();
            if (data.Succeeded)
            {
                _warehouses = data.Data;
            }
        }
        private async Task LoadProductsAsync(int pageNumber, int pageSize)
        {
            string[] orderings = null;


            //var recipeHasTemplate = (_recipeItems.Where(x => x.RecipeItemProductType == ProductType.Template).Count() >= 1) ? true : false;

            var request = new GetAllPagedProductsRequest { PageSize = pageSize, PageNumber = pageNumber, SearchString = "", Orderby = orderings, isTemplate = false };
            var response = await ProductManager.GetProductsAsync(request);
            if (response.Succeeded)
            {
                _productsData = response.Data;
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
        private async Task GetRacksAsync()
        {

            var response = await RackManager.GetAllAsync();
            if (response.Succeeded)
            {
                _rackListAll = response.Data.ToList();
                _rackList = _rackListAll;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private async Task<IEnumerable<int>> SearchWarehouses(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _warehouses.Select(x => x.Id);

            return _warehouses.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

















        /***************** ROWS *********************/


        bool isLoading = false;
        int count;
        IEnumerable<WarehouseReceiptRowsDto> employees;
        //IList<Employee> selectedEmployees;
        private MudTable<WarehouseReceiptRowsDto> _rowItemsTable;

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            var result = AddEditWarehouseExitReceiptModel.WarehouseReceiptRowsDto;
            // Update the Data property
            employees = result;
            // Update the count
            count = result.Count();

            isLoading = false;
        }

        WarehouseReceiptRowsDto receiptRowToInsert;
        WarehouseReceiptRowsDto receiptRowToUpdate;
        private WarehouseReceiptRowsDto warehouseReceiptRowsDtoBackup = null;
        private RadzenDropDownDataGrid<int> ddgRacks;
        private RadzenDropDownDataGrid<int> ddgMeasurement;

        private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.End;
        private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.End;
        private TableEditTrigger editTrigger = TableEditTrigger.RowClick;


        private async void ResetItemToOriginalValues(object element)
        {
            await ReloadData();
        }
        private async Task ReloadData()
        {

            //await LoadData();
            StateHasChanged();

        }
        private void BackupItem(object element)
        {
            // Backup
            warehouseReceiptRowsDtoBackup = new WarehouseReceiptRowsDto()
            {
                ProductId = ((WarehouseReceiptRowsDto)element).Id,
                Quantity = ((WarehouseReceiptRowsDto)element).Quantity,
                RackId = ((WarehouseReceiptRowsDto)element).RackId,
                Price = ((WarehouseReceiptRowsDto)element).Price,
                RowNo = ((WarehouseReceiptRowsDto)element).RowNo,
                MeasurementUnitId = ((WarehouseReceiptRowsDto)element).MeasurementUnitId,
                WarehouseReceiptId = AddEditWarehouseExitReceiptModel.Id

            };
        }
        private async void ItemHasBeenCommitted(object element)
        {

            // ((WarehouseReceiptRowsDto)element).RackCode = _rackList.Where(x => x.Id == ((WarehouseReceiptRowsDto)element).RackId);


            //((WarehouseReceiptRowsDto)element).ProductCode = _rackList.Where(x => x.Id == ((WarehouseReceiptRowsDto)element).RackId);
            /*
            try
            {

            
                WarehouseReceiptRowsDto item = new WarehouseReceiptRowsDto()
                {
                    ProductId = ((WarehouseReceiptRowsDto)element).ProductId,
                    Quantity = ((WarehouseReceiptRowsDto)element).Quantity,
                    RackId = ((WarehouseReceiptRowsDto)element).RackId,
                    Price = ((WarehouseReceiptRowsDto)element).Price,
                    RowNo = ((WarehouseReceiptRowsDto)element).RowNo,
                    MeasurementUnitId = ((WarehouseReceiptRowsDto)element).MeasurementUnitId,
                    Id = ((WarehouseReceiptRowsDto)element).Id,
                    WarehouseReceiptId = AddEditWarehouseReceiptModel.Id
                };
                warehouseReceiptRowsDtoBackup = null;

                var id = ((WarehouseReceiptRowsDto)element).Id;
                var editİtem = AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.FirstOrDefault(x => x.Id == id);
                if (editİtem != null)
                {
                    AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.FirstOrDefault(x => x.Id == id).ProductId = item.ProductId;
                    AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.FirstOrDefault(x => x.Id == id).MeasurementUnitId = item.MeasurementUnitId;
                    AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.FirstOrDefault(x => x.Id == id).Quantity = item.Quantity;
                    AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.FirstOrDefault(x => x.Id == id).Price = item.Price;
                    AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.FirstOrDefault(x => x.Id == id).RowNo = item.RowNo;
                    AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.FirstOrDefault(x => x.Id == id).RackId = item.RackId;

                }
                else {
                    AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Add(item);
                }
                //await SaveRecipeItemAsync(item);

                await ReloadData();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            */
            StateHasChanged();
        }

        private async Task Delete(WarehouseReceiptRowsDto id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id.Id)}
            };
            var options = new MudBlazor.DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                //var removeItem = AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Remove(id);
                AddEditWarehouseExitReceiptModel.WarehouseReceiptRowsDto.Remove(id);

            }
        }
        private async Task AddNewRow()
        {


            WarehouseReceiptRowsDto addedRow = new();

            AddEditWarehouseExitReceiptModel.WarehouseReceiptRowsDto.Add(addedRow);
            StateHasChanged();
            await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "rowContainer");

        }

        /*
        async Task InsertRow()
        {
            receiptRowToInsert = new WarehouseReceiptRowsDto();
            await receiptRowsGrid.InsertRow(receiptRowToInsert);
        }

        void OnCreateRow(WarehouseReceiptRowsDto receiptRow)
        {
            AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Add(receiptRow);
        }
        async Task EditRow(WarehouseReceiptRowsDto receiptRow)
        {
            receiptRowToUpdate = receiptRow;
            await receiptRowsGrid.EditRow(receiptRow);
        }

        async Task DeleteRow(WarehouseReceiptRowsDto receiptRow)
        {
            if (receiptRow == receiptRowToInsert)
            {
                receiptRowToInsert = null;
            }

            if (receiptRow == receiptRowToUpdate)
            {
                receiptRowToUpdate = null;
            }

            if (AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Contains(receiptRow))
            {
                //dbContext.Remove<Order>(order);

                //dbContext.SaveChanges();
                AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Remove(receiptRow);
                await receiptRowsGrid.Reload();
            }
            else
            {
                receiptRowsGrid.CancelEditRow(receiptRow);
                await receiptRowsGrid.Reload();
            }
        }
        async Task SaveRow(WarehouseReceiptRowsDto receiptRow)
        {
            await receiptRowsGrid.UpdateRow(receiptRow);
        }

        void CancelEdit(WarehouseReceiptRowsDto receiptRow)
        {
            if (receiptRow == receiptRowToInsert)
            {
                receiptRowToInsert = null;
            }

            receiptRowToUpdate = null;

            receiptRowsGrid.CancelEditRow(receiptRow);

            var orderExit = AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Where(x=> x.Id == receiptRow.Id);
            
            //TODO load from backup
            if (orderExit.State == EntityState.Modified)
            {
                orderExit.CurrentValues.SetValues(orderExit.OriginalValues);
                orderExit.State = EntityState.Unchanged;
            }
        }
        void OnUpdateRow(WarehouseReceiptRowsDto receiptRow)
        {
            if (receiptRow == receiptRowToInsert)
            {
                receiptRowToInsert = null;
            }

            receiptRowToUpdate = null;

            AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Where(x => x.Id == receiptRow.Id).FirstOrDefault().Quantity = receiptRow.Quantity;
            AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Where(x => x.Id == receiptRow.Id).FirstOrDefault().MeasurementUnitId = receiptRow.MeasurementUnitId;
            AddEditWarehouseReceiptModel.WarehouseReceiptRowsDto.Where(x => x.Id == receiptRow.Id).FirstOrDefault().ProductId = receiptRow.ProductId;

            //dbContext.Update(order);

            //dbContext.SaveChanges();
        }

        void Reset()
        {
            receiptRowToUpdate = null;
            receiptRowToInsert = null;
        }
        */


        private async Task<IEnumerable<int>> SearchProducts(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _productsData.Select(x => x.Id);

            return _productsData.Where(x => x.CodeAndName.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        private IEnumerable<string> ValidateProducts(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                yield return "Ürün seçmeniz zorunludur";
                yield break;
            }

            if (_productsData.Where(x => x.CodeAndName == value).Count() == 0)
            {
                yield return "Ürün bulunamadı";
            }
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
        private IEnumerable<string> ValidateMeasurementUnit(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                yield return "Ölçü birimi zorunludur";
                yield break;
            }

            if (_measurementUnits.Where(x => x.Code == value).Count() == 0)
            {
                yield return "Ölçü birimi bulunamadı";
            }
        }


        private async Task<IEnumerable<int?>> SearchRacks(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _rackListAll.Select(x => (int?)x.Id);

            return _rackListAll.Where(x => x.Code.Contains(value, StringComparison.InvariantCultureIgnoreCase) && x.WarehouseId == AddEditWarehouseExitReceiptModel.WarehouseId)
                .Select(x => (int?)x.Id);
        }
        private IEnumerable<string> ValidateRacks(string value)
        {

            if (_rackListAll.Where(x => x.Code == value).Count() == 0)
            {
                yield return "Raf bulunamadı";
            }
        }
    }
}
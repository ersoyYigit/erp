using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Molds.Commands.AddEdit;
using ArdaManager.Application.Features.Molds.Queries.GetAllPaged;
using ArdaManager.Application.Requests;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Mold;
using ArdaManager.Client.Infrastructure.Managers.Misc.Category;
using ArdaManager.Domain.Enums;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ArdaManager.Client.Infrastructure.Managers.Corporation.Company;
using ArdaManager.Application.Features.Companies.Queries.GetAllPaged;
using ArdaManager.Application.Requests.Corporation;
using ArdaManager.Domain.Enums.Doc;
using System.Linq;
using ArdaManager.Application.Extensions;

namespace ArdaManager.Client.Pages.Catalog
{
    public partial class AddEditMoldModal
    {

        private IEnumerable<GetAllPagedMoldsResponse> _pagedData;
        private IEnumerable<GetAllPagedMoldsResponse> _moldsData;
        private List<GetAllPatternsResponse> _patterns = new();
        private List<GetCategoriesByTypeResponse> _categories = new();
        private List<GetAllPagedCompaniesResponse> _suppliers = new();
        private IEnumerable<GetAllMeasurementUnitsResponse> _measurementUnits;
        //private IList<GetRecipeItemsByOwnerMoldIdResponse> _recipeItems;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";

        [Inject] private IMoldManager MoldManager { get; set; }
        [Inject] private ICategoryManager CategoryManager { get; set; }
        [Inject] private IPatternManager PatternManager { get; set; }
        [Inject] private ICompanyManager CompanyManager { get; set; }
        [Inject] private IMeasurementUnitManager MeasurementUnitManager { get; set; }

        private IJSRuntime JsRuntime;

        [Parameter] public AddEditMoldCommand AddEditMoldModel { get; set; } = new();


        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        //private List<GetCategoriesByTypeResponse> _moldCategories = new();




        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {

            var response = await MoldManager.SaveAsync(AddEditMoldModel);
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
            await LoadDataAsync();

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



            await LoadImageAsync();
            await LoadPatternsAsync();

            Console.WriteLine("patterns loaded");
            await LoadCategoriesAsync();
            await LoadMeasurementUnitsAsync();
            await LoadCompaniesAsync();

            if (AddEditMoldModel.CompanyId != null) { 
                compID = AddEditMoldModel.CompanyId.Value;
                StateHasChanged();
            }
            Console.WriteLine("moldrecies loaded");

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

        private int compID;
        private async Task LoadCompaniesAsync()
        {
            var request = new GetAllPagedCompaniesRequest() { PageSize = 10000, PageNumber = 1, SearchString = "", Orderby = null };
            var data = await CompanyManager.GetCompaniesAsync(request);
            if (data.Succeeded)
            {
                _suppliers = data.Data;
            }
            StateHasChanged();
        }
        private async Task LoadImageAsync()
        {
            var data = await MoldManager.GetMoldImageAsync(AddEditMoldModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditMoldModel.ImageDataURL = imageData;
                }
                /*
                else
                {
                    AddEditMoldModel.ImageDataURL = "Files\\Images\\Static\\placeholder-image.png";
                }*/
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
            AddEditMoldModel.ImageDataURL = null;
            AddEditMoldModel.UploadRequest = new UploadRequest();
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
                AddEditMoldModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditMoldModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Mold, Extension = extension };
            }
        }






        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                JsRuntime.InvokeVoidAsync("blazorjs.dragable");

            return base.OnAfterRenderAsync(firstRender);
        }

        private async void CompanyChanged(IEnumerable<int> values)
        {
            var type = values.FirstOrDefault();
            AddEditMoldModel.CompanyId = compID;
            
        }

    }
}
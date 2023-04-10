using ArdaManager.Application.Extensions;
using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Application.Features.Templates.Commands.AddEdit;
using ArdaManager.Application.Features.Templates.Queries.GetAllPaged;
using ArdaManager.Application.Requests;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Template;
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

namespace ArdaManager.Client.Pages.Catalog
{
    public partial class AddEditTemplateModal
    {

        private IEnumerable<GetAllPagedTemplatesResponse> _pagedData;
        private IEnumerable<GetAllPagedTemplatesResponse> _templatesData;
        private List<GetAllPatternsResponse> _patterns = new();
        private List<GetCategoriesByTypeResponse> _categories = new();
        private IEnumerable<GetAllMeasurementUnitsResponse> _measurementUnits;
        //private IList<GetRecipeItemsByOwnerTemplateIdResponse> _recipeItems;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        
        [Inject] private ITemplateManager TemplateManager { get; set; }
        [Inject] private ICategoryManager CategoryManager { get; set; }
        [Inject] private IPatternManager PatternManager { get; set; }
        [Inject] private IMeasurementUnitManager MeasurementUnitManager { get; set; }

        private IJSRuntime JsRuntime;

        [Parameter] public AddEditTemplateCommand AddEditTemplateModel { get; set; } = new();

        
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        //private List<GetCategoriesByTypeResponse> _templateCategories = new();
        

        

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {

            var response = await TemplateManager.SaveAsync(AddEditTemplateModel);
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

            Console.WriteLine("templaterecies loaded");

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
            var data = await TemplateManager.GetTemplateImageAsync(AddEditTemplateModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditTemplateModel.ImageDataURL = imageData;
                    StateHasChanged();
                }
                /*
                else
                {
                    AddEditTemplateModel.ImageDataURL = "Files\\Images\\Static\\placeholder-image.png";
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
            AddEditTemplateModel.ImageDataURL = null;
            AddEditTemplateModel.UploadRequest = new UploadRequest();
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
                AddEditTemplateModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditTemplateModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Template, Extension = extension };
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
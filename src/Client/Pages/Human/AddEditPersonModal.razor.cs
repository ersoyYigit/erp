using ArdaManager.Application.Requests;
using ArdaManager.Client.Extensions;
using ArdaManager.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using ArdaManager.Client.Infrastructure.Managers.Misc.Category;
using ArdaManager.Application.Features.Categories.Queries.GetCategoriesByType;
using ArdaManager.Domain.Enums;
using ArdaManager.Application.Features.Persons.Commands.AddEdit;
using ArdaManager.Client.Infrastructure.Managers.Human.Person;


namespace ArdaManager.Client.Pages.Human
{
    public partial class AddEditPersonModal
    {
        [Inject] private IPersonManager PersonManager { get; set; }
        [Inject] private ICategoryManager CategoryManager { get; set; }

        [Parameter] public AddEditPersonCommand AddEditPersonModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        //private List<GetCategoriesByTypeResponse> _personCategories = new();

        private List<GetCategoriesByTypeResponse> _categories = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await PersonManager.SaveAsync(AddEditPersonModel);
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
        }

        private async Task LoadDataAsync()
        {
            await LoadImageAsync();
            await LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            GetCategoriesByTypeQuery query = new GetCategoriesByTypeQuery() { Type = (int)CategoryType.Person };
            var data = await CategoryManager.GetByTypeAsync(query);
            //var data = await CategoryManager.GetAllAsync();

            if (data.Succeeded)
            {
                _categories = data.Data;
            }
        }
     

        private async Task LoadImageAsync()
        {
            var data = await PersonManager.GetPersonImageAsync(AddEditPersonModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditPersonModel.ImageDataURL = imageData;
                }
            }
        }

        private void DeleteAsync()
        {
            AddEditPersonModel.ImageDataURL = null;
            AddEditPersonModel.UploadRequest = new UploadRequest();
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
                AddEditPersonModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditPersonModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Person, Extension = extension };
            }
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
    }
}

using ArdaManager.Application.Features.Patterns.Queries.GetAll;
using ArdaManager.Client.Extensions;
using ArdaManager.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ArdaManager.Application.Features.Patterns.Commands.AddEdit;
using ArdaManager.Client.Infrastructure.Managers.Catalog.Pattern;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using ArdaManager.Client.Infrastructure.Managers.Catalog;

namespace ArdaManager.Client.Pages.Catalog
{
    public partial class Patterns
    {
        [Inject] private IPatternManager PatternManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllPatternsResponse> _patternList = new();
        private GetAllPatternsResponse _pattern = new();
        private string _searchString = "";
        

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePatterns;
        private bool _canEditPatterns;
        private bool _canDeletePatterns;
        private bool _canExportPatterns;
        private bool _canSearchPatterns;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePatterns = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Patterns.Create)).Succeeded;
            _canEditPatterns = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Patterns.Edit)).Succeeded;
            _canDeletePatterns = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Patterns.Delete)).Succeeded;
            _canExportPatterns = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Patterns.Export)).Succeeded;
            _canSearchPatterns = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Patterns.Search)).Succeeded;

            await GetPatternsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetPatternsAsync()
        {
            var response = await PatternManager.GetAllAsync();
            if (response.Succeeded)
            {
                _patternList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
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
                var response = await PatternManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await PatternManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Patterns).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Patterns exported"]
                    : _localizer["Filtered Patterns exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _pattern = _patternList.FirstOrDefault(c => c.Id == id);
                if (_pattern != null)
                {
                    parameters.Add(nameof(AddEditPatternModal.AddEditPatternModel), new AddEditPatternCommand
                    {
                        Id = _pattern.Id,
                        Name = _pattern.Name,
                        Description = _pattern.Description,
                        Tax = _pattern.Tax
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditPatternModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _pattern = new GetAllPatternsResponse();
            await GetPatternsAsync();
        }

        private bool Search(GetAllPatternsResponse pattern)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (pattern.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (pattern.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
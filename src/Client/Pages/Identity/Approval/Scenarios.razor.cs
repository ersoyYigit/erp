
using ArdaManager.Client.Infrastructure.Managers.Catalog;
using ArdaManager.Client.Pages.Catalog;
using ArdaManager.Shared.Constants.Application;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using ArdaManager.Client.Infrastructure.Managers.Approval.Scenario;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using ArdaManager.Client.Infrastructure.Routes;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Domain.Enums.Doc;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;
using ArdaManager.Application.Responses.Approval;

namespace ArdaManager.Client.Pages.Identity.Approval
{
    public partial class Scenarios
    {
        [Inject] private IScenarioManager ScenarioManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<ApprovalScenarioResponse> _scenarioList = new();
        private ApprovalScenarioResponse _scenario = new();
        private string _searchString = "";


        private ClaimsPrincipal _currentUser;
        private bool _canCreateScenarios;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateScenarios = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Approval.Scenaios)).Succeeded;
            

            await GetScenariosAsync();
            _loaded = true;
        }

        private async Task GetScenariosAsync()
        {
            var response = await ScenarioManager.GetAllScenariosAsync();
            if (response.Succeeded)
            {
                _scenarioList = response.Data.ToList();
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
            string deleteContent = "içeriği sil";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Sil", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ScenarioManager.DeleteScenarioAsync(id);
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

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _scenario = _scenarioList.FirstOrDefault(c => c.Id == id);
                if (_scenario != null)
                {
                    parameters.Add(nameof(AddEditScenarioModal.AddEditScenarioModel), new ApprovalScenarioResponse
                    {
                        Id = _scenario.Id,
                        DocType = _scenario.DocType,
                        ApprovalSteps = _scenario.ApprovalSteps
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var optionsEx = new DialogOptionsEx
            {
                CloseButton = true,
                MaximizeButton = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = false,
                FullScreen = false,
                DisableBackdropClick = true,
                FullHeight = false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = DialogPosition.Center,
                Resizeable = true
            };
            //var dialog = await _dialogService.ShowEx<AddEditScenarioModal>(id == 0 ? "Yeni Senaryo" : "Senaryo Güncelle", parameters, optionsEx);
            var dialog = _dialogService.Show<AddEditScenarioModal>(id == 0 ? "Yeni Senaryo" : "Senaryo Güncelle", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

    
        /*
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var optionsEx = new DialogOptionsEx
            {
                CloseButton = true,
                MaximizeButton = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = false,
                FullScreen = false,
                DisableBackdropClick = true,
                FullHeight = false,
                DragMode = MudDialogDragMode.WithoutBounds,
                Animations = new[] { AnimationType.Pulse },
                Position = DialogPosition.Center,
                Resizeable = true
            };
            //var dialog = await _dialogService.ShowEx<Test>(id == 0 ? "Yeni Senaryo" : "Senaryo Güncelle", parameters, optionsEx);
            var dialog = _dialogService.Show<AddEditScenarioModal>(id == 0 ? "Yeni Senaryo" : "Senaryo Güncelle", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        */


        private async Task Reset()
        {
            _scenario = new ApprovalScenarioResponse();
            await GetScenariosAsync();
        }

        
    }
}

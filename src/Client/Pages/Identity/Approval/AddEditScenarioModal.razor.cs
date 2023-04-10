using ArdaManager.Application.Features.Docs.PurchaseRequests.Commands.AddEdit;
using ArdaManager.Application.Responses.Approval;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Infrastructure.Managers.Approval.Scenario;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.Identity.Roles;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArdaManager.Client.Pages.Identity.Approval
{

    public partial class AddEditScenarioModal
    {
        [Inject] private IScenarioManager ScenarioManager { get; set; }
        [Inject] private IRoleManager RoleManager { get; set; }

        [Parameter] public ApprovalScenarioResponse AddEditScenarioModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private List<RoleResponse> _roleList = new();



        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await ScenarioManager.UpsertScenarioAsync(AddEditScenarioModel);
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
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (AddEditScenarioModel.Id == 0)
                AddEditScenarioModel.ApprovalSteps = new List<ApprovalStepResponse>();

            await GetRolesAsync();
            await Task.CompletedTask;
        }

        private async Task DeleteRow(ApprovalStepResponse rowCtx)
        {
            if (rowCtx != null)
            {
                AddEditScenarioModel.ApprovalSteps.Remove(rowCtx);
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task AddRow()
        {
            ApprovalStepResponse newRow = new ApprovalStepResponse
            {
                StepNumber = AddEditScenarioModel.ApprovalSteps.Count + 1,
                UserRoleId = ""
            };

            AddEditScenarioModel.ApprovalSteps.Add(newRow);
            await InvokeAsync(StateHasChanged);
        }

        private async Task GetRolesAsync()
        {
            var response = await RoleManager.GetRolesAsync();
            if (response.Succeeded)
            {
                _roleList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
    }





}

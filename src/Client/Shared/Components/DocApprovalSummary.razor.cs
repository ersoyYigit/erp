using ArdaManager.Application.Responses.Approval;
using ArdaManager.Client.Infrastructure.Managers.Approval;
using ArdaManager.Domain.Enums.Doc;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArdaManager.Client.Shared.Components
{
    public partial class DocApprovalSummary
    {
        [Parameter] public int DocumentId { get; set; }
        [Inject] private IApproveManager ApproveManager { get; set; }

        private List<DocumentApprovalStatusResponse> _statuses = new();
        private DocState docState;// = DocState.Waiting;
        public bool _isOpen = false;



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadApprovalStatusesAsync();

                await Task.CompletedTask;
                //await Table.SetFocusAsync();
            }
        }




        private async Task LoadApprovalStatusesAsync()
        {
            var response = await ApproveManager.GetDocumentApprovalStatusAsync(DocumentId);
            if (response.Succeeded)
            {
                _statuses = response.Data.ToList();
                docState = _statuses.FirstOrDefault().DocState;
                await InvokeAsync(StateHasChanged); 
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        public void ToggleOpen()
        {
            if (_isOpen)
                _isOpen = false;
            else
                _isOpen = true;
        }
    }
}

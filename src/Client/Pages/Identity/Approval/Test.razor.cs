using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ArdaManager.Client.Pages.Identity.Approval
{
    public partial class Test
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}

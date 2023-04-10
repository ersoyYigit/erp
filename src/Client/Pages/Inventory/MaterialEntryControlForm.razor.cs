using ArdaManager.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;


namespace ArdaManager.Client.Pages.Warehouse
{
    public partial class MaterialEntryControlForm : ComponentBase
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        //private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            /*
            _loaded = true;
            {
                await LoadDataAsync();
                StateHasChanged();
            };
            */
            await HubConnection.StartAsync();
        }

        private async Task LoadDataAsync()
        {
            await Task.Delay(500);
        }
    }
}

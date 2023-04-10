using ArdaManager.Application.Features.Categories.Queries.GetAll;
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
using ArdaManager.Application.Features.Categories.Commands.AddEdit;
using ArdaManager.Client.Infrastructure.Managers.Misc.Category;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Polly;

namespace ArdaManager.Client.Pages.Misc
{
    public partial class Categories
    {
        [Inject] private ICategoryManager CategoryManager { get; set; }


        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllCategoriesResponse> _productCategoryList = new();
        private GetAllCategoriesResponse _productCategory = new();
        private string _searchString = "";
        IEnumerable<GetAllCategoriesResponse> rootCategories;

        bool? allGroupsExpanded;
        RadzenDataGrid<GetAllCategoriesResponse> grid;

        IList<GetAllCategoriesResponse> selectedCategories;
        private ClaimsPrincipal _currentUser;
        private bool _canCreateCategories;
        private bool _canEditCategories;
        private bool _canDeleteCategories;
        private bool _canExportCategories;
        private bool _canSearchCategories;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Create)).Succeeded;
            _canEditCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Edit)).Succeeded;
            _canDeleteCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Delete)).Succeeded;
            _canExportCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Export)).Succeeded;
            _canSearchCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Categories.Search)).Succeeded;

            await GetCategoriesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            selectedCategories = rootCategories.Take(1).ToList();
        }

        private async Task GetCategoriesAsync()
        {
            var response = await CategoryManager.GetAllAsync();
            if (response.Succeeded)
            {
                _productCategoryList = response.Data.ToList();
                rootCategories = _productCategoryList.Where(x => x.ParentCategoryId == null).AsEnumerable();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }


        void RowRender(RowRenderEventArgs<GetAllCategoriesResponse> args)
        {
            args.Expandable = _productCategoryList.Where(c => c.ParentCategoryId == args.Data.Id).Any();
            //args.Expandable = dbContext.Employees.Where(e => e.ReportsTo == args.Data.EmployeeID).Any();
        }

        void LoadChildData(DataGridLoadChildDataEventArgs<GetAllCategoriesResponse> args)
        {
            args.Data = _productCategoryList.Where(c => c.ParentCategoryId == args.Item.Id);
            //args.Data = dbContext.Employees.Where(e => e.ReportsTo == args.Item.EmployeeID);
            Console.WriteLine("load child");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                await grid.ExpandRow(rootCategories.FirstOrDefault());
            }

        }

        void OnCellContextMenu(DataGridCellMouseEventArgs<GetAllCategoriesResponse> args)
        {
            //selectedEmployees = new List<GetAllCategoriesResponse>() { args.Data };
            Console.WriteLine("context menu");


  
            ContextMenuService.Open(args,
                new List<ContextMenuItem> {
                    new ContextMenuItem(){ Text = "Kök Kategori Ekle", Value = 1 },
                    new ContextMenuItem(){ Text = "Alt Kategori Ekle", Value = 2 },
                    new ContextMenuItem(){ Text = "Düzenle", Value = 3 },
                    new ContextMenuItem(){ Text = "Sil", Value = 4 },
                },
                (e) => {
                    switch ((int)e.Value)
                    {
                        case 1:

                            InvokeModal(0);
                            break;
                        case 2:
                            InvokeModal(0,args.Data.Id);
                            break;
                        case 3:
                            InvokeModal(args.Data.Id,0);
                            break;
                        case 4:
                            Delete(args.Data.Id);
                            break;
                        
                    }
                    ContextMenuService.Close();

                    Console.WriteLine($"Menu item with Value={e.Value} clicked. Column: {args.Column.Property}, EmployeeID: {args.Data.Id}");
                }
             );
        }

        void ToggleGroups(bool? value)
        {
            allGroupsExpanded = value;
        }
        void OnGroupRowRender(GroupRowRenderEventArgs args)
        {
            if (args.FirstRender && args.Group.Data.Key == "Vice President, Sales" || allGroupsExpanded != null)
            {
                args.Expanded = allGroupsExpanded != null ? allGroupsExpanded : false;
            }
        }
        void OnGroupRowExpand(Group group)
        {
            Console.WriteLine($"Group row with key: {group.Data.Key} expanded");
        }

        void OnGroupRowCollapse(Group group)
        {
            Console.WriteLine($"Group row with key: {group.Data.Key} collapsed");
        }

        void OnGroup(DataGridColumnGroupEventArgs<GetAllCategoriesResponse> args)
        {
            Console.WriteLine($"DataGrid {(args.GroupDescriptor != null ? "grouped" : "ungrouped")} by {args.Column.GetGroupProperty()}");
        }




        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new MudBlazor.DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await CategoryManager.DeleteAsync(id);
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
            await Task.Delay(5);
            /*
            var response = await CategoryManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Categories).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Categories exported"]
                    : _localizer["Filtered Categories exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            */
        }

        private async Task InvokeModal(int id = 0, int parentCategoryId = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _productCategory = _productCategoryList.FirstOrDefault(c => c.Id == id);
                if (_productCategory != null)
                {
                    parameters.Add(nameof(AddEditCategoryModal.AddEditCategoryModel), new AddEditCategoryCommand
                    {
                        Id = _productCategory.Id,
                        Name = _productCategory.Name,
                        Code = _productCategory.Code,
                        Type = _productCategory.Type,
                        ParentCategoryId = _productCategory.ParentCategoryId,
                        Description = _productCategory.Description
                    });
                }
            }
            else if(parentCategoryId != 0)
            {
                var tempProductCategory = _productCategoryList.FirstOrDefault(c => c.Id == parentCategoryId);
                parameters.Add(nameof(AddEditCategoryModal.AddEditCategoryModel), new AddEditCategoryCommand
                {
                    ParentCategoryId = parentCategoryId,
                    Type = tempProductCategory.Type
                });
            }
            var options = new MudBlazor.DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditCategoryModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _productCategory = new GetAllCategoriesResponse();
            await GetCategoriesAsync();
        }

        private bool Search(GetAllCategoriesResponse productCategory)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (productCategory.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (productCategory.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}

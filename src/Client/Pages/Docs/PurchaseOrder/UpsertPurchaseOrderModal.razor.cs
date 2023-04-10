using ArdaManager.Application.Features.Docs.PurchaseOrders.Commands;
using ArdaManager.Application.Features.Docs.PurchaseOrders.Queries;
using ArdaManager.Application.Features.MeasurementUnits.Queries.GetAll;
using ArdaManager.Application.Features.Products.Queries.Search;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Client.Extensions;
using ArdaManager.Client.Infrastructure.Managers.Catalog.MeasurementUnit;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using ArdaManager.Client.Shared.Dialogs;
using ArdaManager.Shared.Constants.Application;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Extensions.Options;
using MudBlazor.Extensions;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using ArdaManager.Client.Infrastructure.Managers.Docs.PurchaseOrder;
using ArdaManager.Application.Features.Companies.Queries.Search;
using ArdaManager.Client.Shared.Dialogs.RelationPickers.Purchase;
using ArdaManager.Application.Features.Docs.PurchaseRequests.Queries;
using ArdaManager.Application.Models.Chat;
using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Application.Features.Taxes.Queries;
using System.Linq;
using System;
using ArdaManager.Client.Pages.Catalog;
using Newtonsoft.Json.Linq;
using Nextended.Core.COM;
using ArdaManager.Application.Features.Docs.PurchaseOffers.Queries;
using ArdaManager.Application.Requests.Approval;
using ArdaManager.Client.Infrastructure.Managers.Approval;
using ArdaManager.Domain.Enums.Doc;

namespace ArdaManager.Client.Pages.Docs.PurchaseOrder
{
    public partial class UpsertPurchaseOrderModal
    {
        [Inject] private IPurchaseOrderManager PurchaseOrderManager { get; set; }
        //[Inject] private IMeasurementUnitManager MeasurementUnitManager { get; set; }
        [Inject] private IGlobalVariableService GlobalVarService { get; set; }
        [Inject] private IApproveManager ApproveManager { get; set; }
        private List<UserResponse> _userList = new();

        [Parameter] public UpsertPurchaseOrderCommand UpsertPurchaseOrderModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllMeasurementUnitsResponse> _measurementUnits = new();
        private List<GetAllCurrenciesResponse> _currencies = new();
        private List<GetAllTaxesResponse> _taxes = new();

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private ValidationResult _validationResult;
        private int selectedCurrencyId = 1;
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {

            var response = await PurchaseOrderManager.SaveAsync(UpsertPurchaseOrderModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                try
                {
                    if (UpsertPurchaseOrderModel.PurchaseOfferId.HasValue)
                    {
                        UpdateDocStateRequest closeOrd = new UpdateDocStateRequest()
                        {
                            BaseDocId = UpsertPurchaseOrderModel.PurchaseOfferId.Value,
                            NewDocState = DocState.Finished
                        };
                        await ApproveManager.UpdateDocStateAsync(closeOrd);
                    }
                }
                catch (Exception ex)
                {
                    _snackBar.Add(ex.Message, Severity.Error);
                }
                
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

            if (UpsertPurchaseOrderModel.Id == 0)
            {
                UpsertPurchaseOrderModel.CurrencyId = 1;
                UpsertPurchaseOrderModel.ExchangeRate = 1;
                UpsertPurchaseOrderModel.CurrencyName = _currencies.Where(x => x.Id == 1).FirstOrDefault()?.Name;
                UpsertPurchaseOrderModel.CurrencyCode = _currencies.Where(x => x.Id == 1).FirstOrDefault()?.Code;
                //UpsertPurchaseOrderModel.ExchangeDate = DateTime.UtcNow;
                AddRow();
            }
        }

        private async Task LoadDataAsync()
        {
            _measurementUnits = GlobalVarService.MeasurementUnits;
            _currencies = GlobalVarService.Currencies;
            _taxes = GlobalVarService.Taxes;
            //await SetCurrentUser();
            await Task.CompletedTask;
        }



        private void AddRow()
        {
            UpsertPurchaseOrderModel.PurchaseOrderRowResponse ??= new List<PurchaseOrderRowResponse>();
            UpsertPurchaseOrderModel.PurchaseOrderRowResponse.Add(new PurchaseOrderRowResponse()
            {
                CurrencyId = UpsertPurchaseOrderModel.CurrencyId,
                CurrencyCode = UpsertPurchaseOrderModel.CurrencyCode,
                CurrencyName = UpsertPurchaseOrderModel.CurrencyName,
                ExchangeRate = UpsertPurchaseOrderModel.ExchangeRate,
                TaxId = 4
            });
        }


        private async Task DeleteRow(PurchaseOrderRowResponse row)
        {
            UpsertPurchaseOrderModel.PurchaseOrderRowResponse.Remove(row);
            await Task.CompletedTask;
        }


        private void OnDescriptionFieldBlur()
        {
            //AddRow();
        }

        private void CurrencyChanged(IEnumerable<int> values)
        {
            var currencyId = values.FirstOrDefault();
            selectedCurrencyId = currencyId;
            if (UpsertPurchaseOrderModel.PurchaseOrderRowResponse?.Count > 0)
            {
                foreach (var item in UpsertPurchaseOrderModel.PurchaseOrderRowResponse)
                {
                    item.CurrencyId = currencyId;
                }
            }
        }

        private async Task OpenCompanyDialogOnF4Code(KeyboardEventArgs e, UpsertPurchaseOrderCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenCompanySearchDialog(context.RequesterName, context);
            }
        }

        private async Task OpenCompanySearchDialog(string searchStr, UpsertPurchaseOrderCommand context)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(CompanySearchDialog.SearchQuery), new CompanySearchQuery
            {
                SearchTerm = searchStr,
                Type = Domain.Enums.CompanyType.Supplier
            });


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

            var dialog = await _dialogService.ShowEx<CompanySearchDialog>("Ara", parameters, optionsEx);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedCompany = result.Data as CompanySearchResultDto;
                context.CompanyId = selectedCompany.CompanyId;
                context.CompanyName = selectedCompany.Name;


            }
        }
        private async Task OpenProductSearchDialogOnF4Code(KeyboardEventArgs e, PurchaseOrderRowResponse rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductCode, rowCtx);
            }
        }
        private async Task OpenProductSearchDialogOnF4Name(KeyboardEventArgs e, PurchaseOrderRowResponse rowCtx)
        {
            if (e.Code == "F4")
            {
                await OpenSearchDialog(rowCtx.ProductName, rowCtx);
            }
        }
        private async Task OpenRequestPickerDialogOnF4Code(KeyboardEventArgs e, UpsertPurchaseOrderCommand context)
        {
            if (e.Code == "F4")
            {
                await OpenRequestPickerDialog(context.RequesterName, context);
            }
        }
        private async Task OpenRequestPickerDialog(string searchStr, UpsertPurchaseOrderCommand context)
        {
            var parameters = new DialogParameters();
            parameters.Add("SearchTerm", searchStr);


            var optionsEx = new DialogOptionsEx { CloseButton = true, MaximizeButton = true, MaxWidth = MaxWidth.Large, FullWidth = false, FullScreen = false, DisableBackdropClick = true, FullHeight = false, DragMode = MudDialogDragMode.WithoutBounds, Animations = new[] { AnimationType.Pulse }, Position = DialogPosition.Center, Resizeable = true };

            var dialog = await _dialogService.ShowEx<PurchaseOfferPicker>("Ara", parameters, optionsEx);

            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedPurchaseRequest = result.Data as PurchaseOfferResponse;

                FillDataFromPrevDoc(selectedPurchaseRequest);
            }
        }
        private void FillDataFromPrevDoc(PurchaseOfferResponse selectedPurchaseOffer)
        {
            UpsertPurchaseOrderModel.PrevDocId = selectedPurchaseOffer.Id;
            UpsertPurchaseOrderModel.PrevDocNo = selectedPurchaseOffer.DocNo;
            UpsertPurchaseOrderModel.RequesterId = selectedPurchaseOffer.RequesterId;
            UpsertPurchaseOrderModel.RequesterName = selectedPurchaseOffer.RequesterName;
            UpsertPurchaseOrderModel.RequesterDepartment = selectedPurchaseOffer.RequesterDepartment;
            UpsertPurchaseOrderModel.CurrencyId = selectedPurchaseOffer.CurrencyId;
            UpsertPurchaseOrderModel.CurrencyCode = selectedPurchaseOffer.CurrencyCode;
            UpsertPurchaseOrderModel.CurrencyName = selectedPurchaseOffer.CurrencyName;
            UpsertPurchaseOrderModel.ExchangeDate = selectedPurchaseOffer.ExchangeDate;
            UpsertPurchaseOrderModel.PurchaseRequestId = selectedPurchaseOffer.PurchaseRequestId;
            UpsertPurchaseOrderModel.PurchaseOfferId = selectedPurchaseOffer.Id;
            UpsertPurchaseOrderModel.CompanyId = selectedPurchaseOffer.CompanyId;
            UpsertPurchaseOrderModel.CompanyName = selectedPurchaseOffer.CompanyName;

            UpsertPurchaseOrderModel.PurchaseOrderRowResponse.Clear();
            foreach (PurchaseOfferRowResponse item in selectedPurchaseOffer.PurchaseOfferRowResponse)
            {

                UpsertPurchaseOrderModel.PurchaseOrderRowResponse.Add(new PurchaseOrderRowResponse()
                {
                    Description = item.Description,
                    MeasurementUnitCode = item.MeasurementUnitCode,
                    MeasurementUnitId = item.MeasurementUnitId,
                    MeasurementUnitName = item.MeasurementUnitName,
                    MeasurementUnitSystem = item.MeasurementUnitSystem,
                    Price = item.Price,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    RowNo = item.RowNo,
                    PurchaseRequestRowId = item.PurchaseRequestRowId,
                    //PrevRowId = item.Id,
                    PurchaseOfferRowId= item.Id,
                    CurrencyCode= item.CurrencyCode,
                    CurrencyName = item.CurrencyName,
                    DiscountAmount= item.DiscountAmount,
                    DiscountPercentage= item.DiscountPercentage,
                    TaxAmount = item.TaxAmount,
                    TaxCode = item.TaxCode,
                    TotalAmount= item.TotalAmount,
                    CurrencyId = item.CurrencyId,
                    ExchangeRate = item.ExchangeRate,
                    TaxId = item.TaxId
                    
                });
            }
            _snackBar.Add("Teklif siparişe dönüştürüldü", Severity.Success, config => { config.ShowTransitionDuration = 5000; });
            //_snackBar.Add("Lütfen teklif aldığınız firmayı seçin.", Severity.Warning, config => { config.ShowTransitionDuration = 5000; });
            //_snackBar.Add("Lütfen ürün fiyatlarını doldurun", Severity.Warning, config => { config.ShowTransitionDuration = 5000; });
        }

        private async Task OpenSearchDialog(string searchStr, PurchaseOrderRowResponse rowCtx)
        {
            var parameters = new DialogParameters();

            parameters.Add("SearchTerm", searchStr);

            //parameters.Add("QrCodeText", $"/catalog/warehouseEntryReceipts/{25}");

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
            var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await _dialogService.ShowEx<ProductSearchDialog>("Ara", parameters, optionsEx);
            //var dialog = await _dialogService.ShowAsync<Barcode>("QR Kod", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var selectedProduct = result.Data as ProductSearchResultDto;
                rowCtx.ProductId = selectedProduct.ProductId;
                rowCtx.ProductCode = selectedProduct.Code;
                rowCtx.ProductName = selectedProduct.Name;
                rowCtx.MeasurementUnitSystem = selectedProduct.MeasurementUnitSystem;
                rowCtx.MeasurementUnitId = selectedProduct.MeasurementUnitId;


            }
        }

    }
}

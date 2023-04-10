using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Enums.Doc
{

    public enum DocType
    {

        [Description(@"Genel")]
        Empty = 0,



        #region DEPO 

        [Description(@"Depo Giriş Fişi")]
        WarehouseEntrance = 10,
        [Description(@"Depo Çıkış Fişi")]
        WarehouseExit = 11,
        [Description(@"Depo Transfer Çıkış Fişi")]
        WarehouseTransferExit = 12,
        [Description(@"Depo Transfer Giriş Fişi")]
        WarehouseTransferEntrance = 13,
        [Description(@"Depo Sayım Fişi")]
        WarehouseCount = 14,
        #endregion DEPO 


        #region SATINALMA

        [Description(@"Satın Alma Talebi")]
        PurchaseRequest = 21,
        [Description(@"Satın Alma Teklifi")]
        PurchaseOffer = 22,
        [Description(@"Satın Alma Siparişi")]
        PurchaseOrder = 23,
        #endregion SATINALMA




        #region IS_EMIRLERI

        [Description(@"Kalıp İş Emri")]
        TemplateWorkOrder = 40,
        #endregion IS_EMIRLERI

    }

}

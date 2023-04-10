using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Enums.Doc
{
    public enum TemplateWorkOrderState
    {

        [Description(@"Bekliyor")]
        Waiting = 0,
        [Description(@"Tamamlandı")]
        Done = 1,

        /*
        [Description(@"Onaylandı")]
        Approved = 2,
        [Description(@"Onay Bekliyor")]
        WaitingApproval = 3,
        */

        [Description(@"Planlandı")]
        Planned = 5,
        [Description(@"Çalışılıyor")]
        Working = 6,
        [Description(@"İptal Edildi")]
        Cancelled = 7


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Enums
{
    public enum TemplateState
    {

        [Description(@"Seçiniz")]
        Empty = 0,
        [Description(@"Hazır")]
        Hazir = 1,
        [Description(@"Revize")]
        Revize = 2,
        [Description(@"Arıza")]
        Ariza = 3,
        [Description(@"İptal")]
        Iptal = 4

    }
}

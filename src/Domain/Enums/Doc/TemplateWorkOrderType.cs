using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Enums.Doc
{
    public enum TemplateWorkOrderType
    {

        [Description(@"Tamir/Bakım")]
        Repair = 0,
        [Description(@"Yeniden İşleme")]
        ReWork = 1,
        [Description(@"Polisaj")]
        Polishing = 2

    }
}

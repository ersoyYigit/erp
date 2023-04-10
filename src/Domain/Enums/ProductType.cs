using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Enums
{
    public enum ProductType
    {

        [Description(@"Ürün")]
        Raw = 0,
        [Description(@"Mamül/Yarı Mamül")]
        Producible = 1,
        [Description(@"Sade Ürün")]
        Basic = 2,
        [Description(@"Kalıp")]
        Template = 3,
        [Description(@"Döküm")]
        Mold = 4,

    }


    
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Enums
{
    public enum ProductKind
    {

        [Description(@"Seçiniz")]
        Empty = 0,
        [Description(@"Savurma")]
        Savurma = 1,
        [Description(@"Şiller")]
        Siller = 2,
        [Description(@"Üfleme")]
        Ufleme = 3,
        [Description(@"Sıvama")]
        Sivama = 4,
        [Description(@"Feeder")]
        Feeder = 5,
        [Description(@"Press")]
        Press = 6

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Domain.Enums
{
    public enum MeasurementUnitEnum
    {
        [Description(@"Adet")]
        Unit = 1,
        [Description(@"Gram")]
        Gram = 2,
        
    }
}

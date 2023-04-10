using System.ComponentModel;

namespace ArdaManager.Domain.Enums
{
    
    public enum CategoryType
    {
        
        [Description(@"Kurum")]
        Company = 1,
        [Description(@"Ürün")]
        Product = 2,
        [Description(@"İnsan")]
        Person = 3,

    }
}

using System.ComponentModel;

namespace ArdaManager.Domain.Enums.Doc
{

    public enum DocState
    {

        [Description(@"Onay Bekliyor")]
        Waiting = 0,
        [Description(@"Onaylandi")]
        Approved = 1,

        [Description(@"Red Edildi")]
        Rejected = 7,

        [Description(@"Kapandı")]
        Finished = 10





    }

    
}

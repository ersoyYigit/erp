using System.ComponentModel;

namespace ArdaManager.Application.Enums
{
    public enum UploadType : byte
    {
        [Description(@"Images\Products")]
        Product,
        [Description(@"Images\Templates")]
        Template,

        [Description(@"Images\ProfilePictures")]
        ProfilePicture,

        [Description(@"Documents")]
        Document,

        [Description(@"Images\Persons")]
        Person,
        
        [Description(@"Images\Moldss")]
        Mold,
    }
}
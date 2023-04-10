using System;
using System.ComponentModel;

namespace ArdaManager.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionStringVapp(this Enum val)
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0
                ? attributes[0].Description
                : val.ToString();
        }
    }
}
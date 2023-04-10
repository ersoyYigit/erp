using AutoFilterer.Types;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.IO;

namespace ArdaManager.Application.Extensions
{
    public static class ImageExtensions
    {
        private static string readPath;
        public static void Initialize(IConfiguration Configuration)
        {
            readPath = Configuration["FileReadPath"];
        }

        public static string ToRelativePath(this string val)
        {

            if (string.IsNullOrWhiteSpace(val))
                return Path.Combine(readPath, "/Files/Images/Static/placeholder-image.png");
            else
            {
                string retval = Path.Combine(readPath, val);
                var d = retval;
                return d;
            }
            
        }
    }
}
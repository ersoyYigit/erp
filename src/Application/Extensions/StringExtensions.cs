using System;
using System.ComponentModel;

namespace ArdaManager.Application.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceTurkishChars(this string val)
        {
            string source = "ığüşöçĞÜŞİÖÇ";
            string destination = "igusocGUSIOC";

            string result = val;

            for (int i = 0; i < source.Length; i++)
            {
                result = result.Replace(source[i], destination[i]);
            }

            return result;
        }
    }
}
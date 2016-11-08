using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Backtory.core 
{
    public static class Utils
    {
        public static T checkNotNull<T>(T Object, string message)
        {
            if (Object == null)
            {
                throw new NullReferenceException(message);
            }
            return Object;
        }
    }

    public static class EmptyStringExtension
    {
        public static bool IsEmpty(this string s)
        {
            return s == null || s.All(char.IsWhiteSpace); //string.IsNullOrEmpty(s.Trim() ?? null);
        }
    }
}

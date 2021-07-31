#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace GGHS_Todo
{
    static class Extension
    {
        public static bool IsNullOrEmpty(this TextBox tb)
        {
            return string.IsNullOrEmpty(tb.Text);
        }

        public static bool IsNullOrWhiteSpace(this TextBox tb)
        {
            return string.IsNullOrWhiteSpace(tb.Text);
        }

        public static bool IsNullOrEmpty<T>(this List<T>? list) => list is null || !list.Any();

        public static string putS(this string str, int count) => count is 0 or 1 ? str : str + "s";
    }
}

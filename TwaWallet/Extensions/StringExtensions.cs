using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TwaWallet.Extensions
{
    public static class StringExtensions
    {
        public static bool CaseContains(this string baseString, string textToSearch, StringComparison comparisonMode, bool ignoreDiacritics = false)
        {
            //System.Globalization.CompareOptions.IgnoreNonSpace
            bool result = false;
            if (ignoreDiacritics)
            {
                result = (baseString.IndexOf(textToSearch, comparisonMode) != -1);
            }
            else
            {
                result = (baseString.RemoveDiacritics().IndexOf(textToSearch.RemoveDiacritics(), comparisonMode) != -1);
            }

            return result;
        }

        public static string RemoveDiacritics(this string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            string result = sb.ToString().Normalize(NormalizationForm.FormC);
            return result;
        }


    }
}
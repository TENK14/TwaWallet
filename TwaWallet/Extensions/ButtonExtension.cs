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
    public static class ButtonExtension
    {
        //public static T Tag<T>(this Button b) { get; set; }

        static readonly System.Runtime.CompilerServices.ConditionalWeakTable<Button, IntObject> Flags = new System.Runtime.CompilerServices.ConditionalWeakTable<Button, IntObject>();

        public static int GetFlags(this Button button) { return Flags.GetOrCreateValue(button).Value; }

        public static void SetFlags(this Button button, int newFlags) { Flags.GetOrCreateValue(button).Value = newFlags; }

        class IntObject
        {
            public int Value;
        }
    }
}
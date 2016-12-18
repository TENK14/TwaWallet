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
using SQLite;
using Android.Util;

namespace Database.POCO
{
    public class Owner : Base
    {
        private const string TAG = "X:" + nameof(Owner);

        public string Name { get; set; }
        public bool IsDefault { get; set; } = false;

        public override string ToString()
        {
            Log.Debug(TAG, nameof(ToString));

            return $"{nameof(Id)}: {Id}, \r"
                    + $"{nameof(Name)}: {Name}, \r"
                    + $"{nameof(IsDefault)}: {IsDefault}, \r"
                    ;
        }

    }
}
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
    [Table(nameof(PaymentType))]
    public class PaymentType : BaseWithDescriptionAndDefault
    {
        private const string TAG = "X:" + nameof(PaymentType);

        //public override string ToString()
        //{
        //    Log.Debug(TAG, nameof(ToString));

        //    return $"{nameof(Id)}: {Id}, \r"
        //            + $"{nameof(Description)}: {Description}, \r"
        //            + $"{ nameof(IsDefault)}: {IsDefault.ToString()}\r"
        //            ;
        //}

        public override string ToString()
        {
            Log.Debug(TAG, nameof(ToString));

            return Description;
        }
    }
}
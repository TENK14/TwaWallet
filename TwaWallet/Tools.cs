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

namespace TwaWallet
{
    public static class Tools
    {
        private const string TAG = "X:" + nameof(Tools);


        //public void Log()
        //{
        //    Log.Debug(TAG, string.Empty);

        //}



        //System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }
    }
}
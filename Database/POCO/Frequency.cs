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
using Android.Util;

namespace Database.POCO
{
    // TODO: Dopl� �etnost (int), a� d� vypo��t�vat po kolika ?dnech? se to m� p�i��tat.
    // denn�, t�dn�, m�s��n�, ro�n�
    public class Frequency : BaseWithDescriptionAndDefault
    {
        private const string TAG = "X:" + nameof(Frequency);

        public override string ToString()
        {
            Log.Debug(TAG, nameof(ToString));

            return $"{nameof(Id)}: {Id}, \r"
                    + $"{nameof(Description)}: {Description}, \r"
                    + $"{ nameof(IsDefault)}: {IsDefault.ToString()}\r"
                    ;
        }
    }
}
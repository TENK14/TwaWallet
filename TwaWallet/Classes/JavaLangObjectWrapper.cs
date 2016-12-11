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

namespace TwaWallet.Classes
{
    class JavaLangObjectWrapper<T> : Java.Lang.Object
    {
        private T _value;
        public JavaLangObjectWrapper(T managedValue)
        {
            _value = managedValue;
        }

        public T Value { get { return _value; } }
    }
}
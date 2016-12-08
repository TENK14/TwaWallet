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

namespace Database.POCO
{
    public class BaseWithDescriptionAndDefault : Base
    {
        public string Description { get; set; }
        public bool Default { get; set; } = false;
    }
}
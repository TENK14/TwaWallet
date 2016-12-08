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

namespace Database.POCO
{
    public class Owner : Base
    {
        public string Name { get; set; }
        public bool Default { get; set; } = false;
    }
}
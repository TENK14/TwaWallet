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
    public class RecurringPayment : Base
    {
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int PaymentTypeId { get; set; }
        public int OwnerId { get; set; }
        public float Cost { get; set; }

        /// <summary>
        /// Pøíjem
        /// </summary>
        public bool Earnings { get; set; } = false;
        public int Warranty { get; set; }
        public DateTime DateCreated  { get; set; }
        public string Frequency { get; set; }
        public DateTime LastUpdate { get; set; }
        /// <summary>
        /// Do kdy se budou trvalé platby generovat
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
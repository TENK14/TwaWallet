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
    public class RecurringPayment : Base
    {
        private const string TAG = "X:" + nameof(RecurringPayment);

        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int PaymentTypeId { get; set; }
        public int OwnerId { get; set; }
        public float Cost { get; set; }

        /// <summary>
        /// P��jem
        /// </summary>
        public bool Earnings { get; set; } = false;
        public int Warranty { get; set; }
        public DateTime DateCreated  { get; set; }
        public string Frequency { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsActive { get; set; } = false;

        /// <summary>
        /// Do kdy se budou trval� platby generovat
        /// </summary>
        public DateTime EndDate { get; set; }

        public string ToString(string dateFormat)
        {
            Log.Debug(TAG, nameof(ToString));

            return $"{nameof(Description)}: {Description}, \r"
                    + $"{nameof(Cost)}: {Cost}, \r"
                    + $"{ nameof(CategoryId)}: {CategoryId}, \r"
                    + $"{ nameof(Warranty)}: {Warranty}, \r"
                    + $"{ nameof(OwnerId)}: {OwnerId}, \r"
                    + $"{ nameof(PaymentTypeId)}: {PaymentTypeId}, \r"
                    + $"{ nameof(Earnings)}: {Earnings}, \r"
                    + $"{ nameof(DateCreated)}: {DateCreated.ToString(dateFormat)}\r"
                    + $"{ nameof(Frequency)}: {Frequency}, \r"                    
                    + $"{ nameof(EndDate)}: {EndDate.ToString(dateFormat)}\r"
                    + $"{ nameof(LastUpdate)}: {LastUpdate.ToString(dateFormat)}\r"
                    + $"{ nameof(IsActive)}: {IsActive.ToString()}\r"
                    ;
        }

        public override string ToString()
        {
            Log.Debug(TAG, nameof(ToString));

            return $"{nameof(Id)}: {Id}, \r"
                    + $"{nameof(Description)}: {Description}, \r"
                    + $"{nameof(Cost)}: {Cost}, \r"
                    + $"{ nameof(CategoryId)}: {CategoryId}, \r"
                    + $"{ nameof(Warranty)}: {Warranty}, \r"
                    + $"{ nameof(OwnerId)}: {OwnerId}, \r"
                    + $"{ nameof(PaymentTypeId)}: {PaymentTypeId}, \r"
                    + $"{ nameof(Earnings)}: {Earnings}, \r"
                    + $"{ nameof(DateCreated)}: {DateCreated.ToString()}\r"
                    + $"{ nameof(Frequency)}: {Frequency}, \r"                    
                    + $"{ nameof(EndDate)}: {EndDate.ToString()}\r"
                    + $"{ nameof(LastUpdate)}: {LastUpdate.ToString()}\r"
                    + $"{ nameof(IsActive)}: {IsActive.ToString()}\r"
                    ;
        }
    }
}
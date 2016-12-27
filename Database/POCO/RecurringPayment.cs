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
using SQLite;

namespace Database.POCO
{
    public class RecurringPayment : Base
    {
        private const string TAG = "X:" + nameof(RecurringPayment);

        public string Description { get; set; }
        public int CategoryId { get; set; }
        [Ignore]
        public Category Category { get; set; }

        public int PaymentTypeId { get; set; }
        [Ignore]
        public PaymentType PaymentType { get; set; }
        
        public int OwnerId { get; set; }
        [Ignore]
        public Owner Owner { get; set; }

        public float Cost { get; set; }

        public int IntervalId { get; set; }
        /// <summary>
        /// YYMMDD
        /// </summary>
        [Ignore]
        public Interval Interval { get; set; }

        /// <summary>
        /// Pøíjem
        /// </summary>        
        public bool Earnings { get; set; } = false;
        public int Warranty { get; set; }
        public string Tag { get; set; }
        public DateTime DateCreated  { get; set; } = DateTime.Now;
        
        public DateTime LastUpdate { get; set; } = DateTime.Now.AddDays(-60);
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Do kdy se budou trvalé platby generovat
        /// </summary>
        public DateTime EndDate { get; set; }

        public RecurringPayment IncludeObjects(IDataContext db)
        {
            Log.Debug(TAG, nameof(IncludeObjects));

            this.Owner = db.Select<Owner, int>((o) => o.Id == this.OwnerId, (o) => o.Id).Result.FirstOrDefault();
            this.PaymentType = db.Select<PaymentType, int>((o) => o.Id == this.PaymentTypeId, (o) => o.Id).Result.FirstOrDefault();
            this.Category = db.Select<Category, int>((o) => o.Id == this.CategoryId, (o) => o.Id).Result.FirstOrDefault();
            this.Interval = db.Select<Interval, int>((o) => o.Id == this.IntervalId, (o) => o.Id).Result.FirstOrDefault();
            return this;
        }

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
                    + $"{ nameof(IntervalId)}: {IntervalId}, \r"                    
                    + $"{ nameof(EndDate)}: {EndDate.ToString(dateFormat)}\r"
                    + $"{ nameof(LastUpdate)}: {LastUpdate.ToString(dateFormat)}\r"
                    + $"{ nameof(IsActive)}: {IsActive.ToString()}\r"
                    ;
        }

        //public override string ToString()
        //{
        //    Log.Debug(TAG, nameof(ToString));

        //    return $"{nameof(Id)}: {Id}, \r"
        //            + $"{nameof(Description)}: {Description}, \r"
        //            + $"{nameof(Cost)}: {Cost}, \r"
        //            + $"{ nameof(CategoryId)}: {CategoryId}, \r"
        //            + $"{ nameof(Warranty)}: {Warranty}, \r"
        //            + $"{ nameof(OwnerId)}: {OwnerId}, \r"
        //            + $"{ nameof(PaymentTypeId)}: {PaymentTypeId}, \r"
        //            + $"{ nameof(Earnings)}: {Earnings}, \r"
        //            + $"{ nameof(DateCreated)}: {DateCreated.ToString()}\r"
        //            + $"{ nameof(Frequency)}: {Frequency}, \r"                    
        //            + $"{ nameof(EndDate)}: {EndDate.ToString()}\r"
        //            + $"{ nameof(LastUpdate)}: {LastUpdate.ToString()}\r"
        //            + $"{ nameof(IsActive)}: {IsActive.ToString()}\r"
        //            ;
        //}

        public override string ToString()
        {
            Log.Debug(TAG, nameof(ToString));

            return Description;
        }
    }
}
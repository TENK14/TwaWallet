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
using Java.Sql;

namespace Database.POCO
{
    [Table(nameof(Record))]
    public class Record : Base
    {
        private const string TAG = "X:" + nameof(Record);

        public float Cost { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// ForingKey
        /// </summary>
        [Indexed]
        public int CategoryId { get; set; }   
        [Ignore]
        public Category Category { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Warranty { get; set; }
        /// <summary>
        /// ForingKey
        /// </summary>
        [Indexed]
        public int OwnerId { get; set; }
        [Ignore]
        public Owner Owner { get; set; }
        /// <summary>
        /// ForingKey
        /// </summary>
        [Indexed]
        public int PaymentTypeId { get; set; }
        [Ignore]
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Možnost filtrování (Ebay, Norsko, Francie,...)
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// true - pøíjem
        /// false - výdej
        /// </summary>
        public bool Earnings { get; set; } = false;

        //public Timestamp DateCreated { get; set; }


        public Record IncludeObjects(IDataContext db)
        {
            Log.Debug(TAG, nameof(IncludeObjects));
            
            this.Owner = db.Select<Owner, int>((o) => o.Id == this.OwnerId, (o) => o.Id).Result.FirstOrDefault();
            this.PaymentType = db.Select<PaymentType, int>((o) => o.Id == this.PaymentTypeId, (o) => o.Id).Result.FirstOrDefault();
            this.Category = db.Select<Category, int>((o) => o.Id == this.CategoryId, (o) => o.Id).Result.FirstOrDefault();
            return this;
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
        //            + $"{ nameof(Tag)}: {Tag}, \r"
        //            + $"{ nameof(Date)}: {Date.ToString()}\r"
        //            ;
        //}

        public override string ToString()
        {
            Log.Debug(TAG, nameof(ToString));

            return Description;
        }

        public string ToString(char delimiter, string dateFormat)
        {
            Log.Debug(TAG, $"{nameof(ToString)} - {nameof(delimiter)}:{delimiter}, {nameof(dateFormat)}:{dateFormat}");

            //"01.10.2016"; "-170.0"; "obcerstveni"; "Vylety"; "0"; "Hotovost"; "Tom"
            return 
                $"{Date.ToString(dateFormat)}{delimiter}"
                + $"{Cost}{delimiter}"
                + $"{Description}{delimiter}"
                //+ $"{CategoryId}{delimiter}"
                + $"{Category?.Description ?? string.Empty}{delimiter}"
                + $"{Warranty}{delimiter}"
                //+ $"{PaymentTypeId}{delimiter}"
                + $"{PaymentType.Description}{delimiter}"
                //+ $"{OwnerId}{delimiter}"
                + $"{Owner.Name}{delimiter}"
                + $"{Tag}{delimiter}"
                ;
        }
    }
}
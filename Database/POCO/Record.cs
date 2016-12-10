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
    public class Record : Base
    {
        public float Cost { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// ForingKey
        /// </summary>
        public int CategoryId { get; set; }        
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int Warranty { get; set; }
        /// <summary>
        /// ForingKey
        /// </summary>
        public int OwnerId { get; set; }
        /// <summary>
        /// ForingKey
        /// </summary>
        public int PaymantTypeId { get; set; }

        /// <summary>
        /// Možnost filtrování (Ebay, Norsko, Francie,...)
        /// </summary>
        public string Tag { get; set; }

        public override string ToString()
        {
            //return string.Format("[Person: ID={0}, FirstName={1}, LastName={2}]", ID, FirstName, LastName);

            return $"{nameof(Description)}: {Description}, \r"
                    + $"{nameof(Cost)}: {Cost}, \r"
                    + $"{ nameof(CategoryId)}: {CategoryId}, \r"
                    + $"{ nameof(Warranty)}: {Warranty}, \r"
                    + $"{ nameof(OwnerId)}: {OwnerId}, \r"
                    + $"{ nameof(PaymantTypeId)}: {PaymantTypeId}, \r"
                    + $"{ nameof(Tag)}: {Tag}, \r"
                    + $"{ nameof(DateCreated)}: {DateCreated.ToString()}\r"
                    ;
        }
    }
}
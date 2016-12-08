
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POCO
{
    public class Report
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public float Cost { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// ForingKey
        /// </summary>
        public string CategoryId { get; set; }


        public DateTime DateCreated { get; set; }

        public int Warranty { get; set; }

        /// <summary>
        /// ForingKey
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// ForingKey
        /// </summary>
        public int PaymantTypeId { get; set; }

        //public override string ToString()
        //{
        //    return string.Format("[Person: ID={0}, FirstName={1}, LastName={2}]", ID, FirstName, LastName);
        //}
    }
}

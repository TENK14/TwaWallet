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
    // TODO: DoplÚ Ëetnost (int), aù d· vypoËÌt·vat po kolika ?dnech? se to m· p¯iËÌtat.
    // dennÏ, t˝dnÏ, mÏsÌËnÏ, roËnÏ
    [Table(nameof(Interval))]
    public class Interval : BaseWithDescriptionAndDefault
    {
        private const string TAG = "X:" + nameof(Interval);

        /// <summary>
        /// YYMMDD
        /// </summary>
        //[StringLength(6)]
        public string IntervalCode { get; set; }

        public DateTime BeforeDateTime(DateTime startDateTime)
        {
            Log.Debug(TAG, $"{nameof(NextDateTime)} - {nameof(startDateTime)}:{startDateTime.ToShortDateString()}");

            DateTime result = startDateTime;

            try
            {
                int years, months, days;

                if (int.TryParse(IntervalCode.Substring(0, 2), out years))
                {
                    result = result.AddYears(-years);
                }
                else
                {
                    throw new FormatException("Years couldn't be parsed.");
                }

                if (int.TryParse(IntervalCode.Substring(2, 2), out months))
                {
                    result = result.AddMonths(-months);
                }
                else
                {
                    throw new FormatException("Months couldn't be parsed.");
                }

                if (int.TryParse(IntervalCode.Substring(4, 2), out days))
                {
                    result = result.AddDays(-days);
                }
                else
                {
                    throw new FormatException("Days couldn't be parsed.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, nameof(NextDateTime), ex.Message);
                throw;
            }

            return result;
        }

        public DateTime NextDateTime(DateTime startDateTime)
        {
            Log.Debug(TAG, $"{nameof(NextDateTime)} - {nameof(startDateTime)}:{startDateTime.ToShortDateString()}");

            DateTime result = startDateTime;

            try
            {
                int years, months, days;

                if (int.TryParse(IntervalCode.Substring(0, 2), out years))
                {
                    result = result.AddYears(years);
                }
                else
                {
                    throw new FormatException("Years couldn't be parsed.");
                }

                if (int.TryParse(IntervalCode.Substring(2, 2), out months))
                {
                    result = result.AddMonths(months);
                }
                else
                {
                    throw new FormatException("Months couldn't be parsed.");
                }

                if (int.TryParse(IntervalCode.Substring(4, 2), out days))
                {
                    result = result.AddDays(days);
                }
                else
                {
                    throw new FormatException("Days couldn't be parsed.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, nameof(NextDateTime), ex.Message);
                throw;
            }

            return result;
        }

        //public override string ToString()
        //{
        //    Log.Debug(TAG, nameof(ToString));

        //    return $"{nameof(Id)}: {Id}, \r"
        //            + $"{nameof(Description)}: {Description}, \r"
        //            + $"{ nameof(IntervalCode)}: {IntervalCode.ToString()}\r"
        //            + $"{ nameof(IsDefault)}: {IsDefault.ToString()}\r"
        //            ;
        //}

        public override string ToString()
        {
            Log.Debug(TAG, nameof(ToString));

            return Description;
        }
    }
}
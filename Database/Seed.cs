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
using System.Threading.Tasks;
using SQLite;
using Database.POCO;
using Android.Util;
using Database.Constants;

namespace Database
{
    public class Seed
    {
        private const string TAG = "X:" + nameof(Seed);

        public async Task<bool> FillDBAsync(IDataContext database, string path)
        {
            Log.Debug(TAG, $"{nameof(FillDBAsync)} - {nameof(database)}:{database}, {nameof(path)}:{path}");

            try
            {                
                await database.InsertAll<Owner>(new List<Owner>()
                {
                    new Owner {Name = "Tom", IsDefault = true },
                    new Owner {Name = "Eli", IsDefault = false }
                },
                path);

                await database.InsertAll<PaymentType>(new List<PaymentType>()
                {
                    new PaymentType {Description = "Hotovost", IsDefault = true },
                    new PaymentType {Description = "Karta", IsDefault = false }
                },
                path);

                await database.InsertAll<Category>(new List<Category>()
                {
                    new Category {Description = "Ob�dy", IsDefault = true },
                    new Category {Description = "V�lety", IsDefault = false },
                    new Category {Description = "Potraviny", IsDefault = false },
                    new Category {Description = "Neza�azen�", IsDefault = false },
                    new Category {Description = "Restaurace,Bary", IsDefault = false },
                    new Category {Description = "Dom�cnost", IsDefault = false },
                    new Category {Description = "Oble�en�,obuv", IsDefault = false },
                    new Category {Description = "Pohonn� hmoty", IsDefault = false },
                    new Category {Description = "Slu�by", IsDefault = false },
                    new Category {Description = "Drogerie", IsDefault = false },
                    new Category {Description = "Mzda", IsDefault = false },
                    new Category {Description = "L�ky", IsDefault = false },
                    new Category {Description = "Z�bava,kultura", IsDefault = false },
                    new Category {Description = "Sport", IsDefault = false },
                    new Category {Description = "Doprava", IsDefault = false },
                },
                path);

                await database.InsertAll<Interval>(new List<Interval>()
                {
                    new Interval {Description = "Denn�", IntervalCode = "000001" , IsDefault = false },
                    new Interval {Description = "T�dn�", IntervalCode = "000007" ,IsDefault = false },
                    new Interval {Description = "M�s��n�", IntervalCode = "000100" ,IsDefault = true },
                    new Interval {Description = "�tvrtletn�", IntervalCode = "000300" ,IsDefault = false },
                    new Interval {Description = "Ro�n�", IntervalCode = "010000" ,IsDefault = false },
                },
                path);


                return true; //"Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return false;// ex.Message;
                throw;
            }
        }

        public Task<bool> FillDB(IDataContext database, Owner[] lstOwner, Category[] lstCategory, PaymentType[] lstPaymentType, Interval[] lstInterval)
        {
            Log.Debug(TAG, $"{nameof(FillDB)} - {nameof(database)}:{database}");

            bool result;

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    #region Seed Owners
                    result = database.InsertAll<Owner>(lstOwner).Result;
                    if (!result)
                    {
                        return result;
                    }
                    #endregion

                    #region Seed PaymentTypes
                    result = database.InsertAll<PaymentType>(lstPaymentType).Result;
                    
                    if (!result)
                    {
                        return result;
                    }
                    #endregion

                    #region Seed Categories
                    result = database.InsertAll<Category>(lstCategory).Result;
                    

                    if (!result)
                    {
                        return result;
                    }
                    #endregion

                    #region Seed Intervals

                    result = database.InsertAll<Interval>(lstInterval/*, path*/).Result;
                    
                    if (!result)
                    {
                        return result;
                    } 
                    #endregion

                    Log.Debug(TAG, $"{nameof(FillDB)} - {nameof(result)}:{result}");
                    
                    return result; //"Single data file inserted or updated";
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(FillDB)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }
    }
}
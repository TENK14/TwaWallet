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
    class Seed
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


                return true; //"Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return false;// ex.Message;
                throw;
            }
        }

        public Task<bool> FillDB(IDataContext database, string path)
        {
            Log.Debug(TAG, $"{nameof(FillDB)} - {nameof(database)}:{database}, {nameof(path)}:{path}");

            bool result;

            return Task.Factory.StartNew(() => 
            {
                try
                {
                    result = database.InsertAll<Owner>(new List<Owner>()
                    {
                        new Owner {Name = OwnerConst.Tom, IsDefault = true },
                        new Owner {Name = OwnerConst.Eli, IsDefault = false }
                    },
                    path).Result;

                    if (!result)
                    {
                        return result;
                    }

                    result = database.InsertAll<PaymentType>(new List<PaymentType>()
                    {
                        new PaymentType {Description = PaymentTypeConst.Money /*"Hotovost"*/, IsDefault = true },
                        new PaymentType {Description = PaymentTypeConst.Card /*"Karta"*/, IsDefault = false }
                    },
                    path).Result;

                    if (!result)
                    {
                        return result;
                    }

                    result = database.InsertAll<Category>(new List<Category>()
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
                    path).Result;

                    if (!result)
                    {
                        return result;
                    }

                    Log.Debug(TAG, $"{nameof(FillDB)} - {nameof(result)}:{result}");
                    
                    return result; //"Single data file inserted or updated";
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(FillDB)} - {nameof(ex)}:{ex.Message}");
                    throw;
                    //return false;// ex.Message;
                }
            });
        }
    }
}
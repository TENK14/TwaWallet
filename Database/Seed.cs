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

namespace Database
{
    class Seed
    {
        private const string TAG = "X:" + nameof(Seed);

        public async Task<bool> FillDBAsync(DataContext database, string path)
        {
            Log.Debug(TAG, $"{nameof(FillDBAsync)} - {nameof(database)}:{database}, {nameof(path)}:{path}");

            try
            {                
                await database.InsertAll<Owner>(new List<Owner>()
                {
                    new Owner {Name = "Tom", Default = true },
                    new Owner {Name = "Eli", Default = false }
                },
                path);

                await database.InsertAll<PaymentType>(new List<PaymentType>()
                {
                    new PaymentType {Description = "Hotovost", Default = true },
                    new PaymentType {Description = "Karta", Default = false }
                },
                path);

                await database.InsertAll<Category>(new List<Category>()
                {
                    new Category {Description = "Obìdy", Default = true },
                    new Category {Description = "Výlety", Default = false },
                    new Category {Description = "Potraviny", Default = false },
                    new Category {Description = "Nezaøazené", Default = false },
                    new Category {Description = "Restaurace,Bary", Default = false },
                    new Category {Description = "Domácnost", Default = false },
                    new Category {Description = "Obleèení,obuv", Default = false },
                    new Category {Description = "Pohonné hmoty", Default = false },
                    new Category {Description = "Služby", Default = false },
                    new Category {Description = "Drogerie", Default = false },
                    new Category {Description = "Mzda", Default = false },
                    new Category {Description = "Léky", Default = false },
                    new Category {Description = "Zábava,kultura", Default = false },
                    new Category {Description = "Sport", Default = false },
                    new Category {Description = "Doprava", Default = false },
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

        public Task<bool> FillDB(DataContext database, string path)
        {
            Log.Debug(TAG, $"{nameof(FillDB)} - {nameof(database)}:{database}, {nameof(path)}:{path}");

            bool result;

            return Task.Factory.StartNew(() => 
            {
                try
                {
                    result = database.InsertAll<Owner>(new List<Owner>()
                    {
                        new Owner {Name = "Tom", Default = true },
                        new Owner {Name = "Eli", Default = false }
                    },
                    path).Result;

                    if (!result)
                    {
                        return result;
                    }

                    result = database.InsertAll<PaymentType>(new List<PaymentType>()
                    {
                        new PaymentType {Description = "Hotovost", Default = true },
                        new PaymentType {Description = "Karta", Default = false }
                    },
                    path).Result;

                    if (!result)
                    {
                        return result;
                    }

                    result = database.InsertAll<Category>(new List<Category>()
                    {
                        new Category {Description = "Obìdy", Default = true },
                        new Category {Description = "Výlety", Default = false },
                        new Category {Description = "Potraviny", Default = false },
                        new Category {Description = "Nezaøazené", Default = false },
                        new Category {Description = "Restaurace,Bary", Default = false },
                        new Category {Description = "Domácnost", Default = false },
                        new Category {Description = "Obleèení,obuv", Default = false },
                        new Category {Description = "Pohonné hmoty", Default = false },
                        new Category {Description = "Služby", Default = false },
                        new Category {Description = "Drogerie", Default = false },
                        new Category {Description = "Mzda", Default = false },
                        new Category {Description = "Léky", Default = false },
                        new Category {Description = "Zábava,kultura", Default = false },
                        new Category {Description = "Sport", Default = false },
                        new Category {Description = "Doprava", Default = false },
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
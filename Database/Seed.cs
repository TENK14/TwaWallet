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

namespace Database
{
    class Seed
    {
        public async Task<bool> FillDB(DataContext database, string path)
        {
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
    }
}
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
                    new Category {Description = "Ob�dy", Default = true },
                    new Category {Description = "V�lety", Default = false },
                    new Category {Description = "Potraviny", Default = false },
                    new Category {Description = "Neza�azen�", Default = false },
                    new Category {Description = "Restaurace,Bary", Default = false },
                    new Category {Description = "Dom�cnost", Default = false },
                    new Category {Description = "Oble�en�,obuv", Default = false },
                    new Category {Description = "Pohonn� hmoty", Default = false },
                    new Category {Description = "Slu�by", Default = false },
                    new Category {Description = "Drogerie", Default = false },
                    new Category {Description = "Mzda", Default = false },
                    new Category {Description = "L�ky", Default = false },
                    new Category {Description = "Z�bava,kultura", Default = false },
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
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
using System.Linq.Expressions;

namespace Database
{
    public class DataContext : IDataContext
    {
        private const string TAG = "X:" + nameof(DataContext);

        public string Path { get; private set; }

        public DataContext(string path)
        {
            Log.Debug(TAG, $"{nameof(DataContext)} - {nameof(path)}:{path}");

            Path = path;
        }

        public Task<bool> CreateDatabase()
        {
            Log.Debug(TAG, $"{nameof(CreateDatabase)}");
            return CreateDatabase(Path);
        }

        public Task<bool> CreateDatabase(string path)
        {
            Log.Debug(TAG, $"{nameof(CreateDatabase)} - {nameof(path)}:{path}");

            return Task.Factory.StartNew( () =>
            {
                try
                {
                    var connection = new SQLiteAsyncConnection(path);

                    var r = connection.CreateTablesAsync(typeof(Owner), typeof(Category), typeof(PaymentType), typeof(Interval), typeof(Record), typeof(RecurringPayment)).Result;
                    
                    return true;
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(CreateDatabase)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            }
            );
        }

        public Task<bool> InsertUpdateData<T>(T data)
        {
            Log.Debug(TAG, $"{nameof(InsertUpdateData)} - {nameof(data)}:{data}");

            return InsertUpdateData(data, Path);
        }

        public Task<bool> InsertUpdateData<T>(T data, string path)
        {
            Log.Debug(TAG, $"{nameof(InsertUpdateData)} - {nameof(data)}:{data}, {nameof(path)}:{path}");

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if (db.InsertAsync(data).Result != 0)
                    {
                        if ( db.UpdateAsync(data).Result != 0)
                        {
                            return true;  //"Single data file inserted or updated";
                        }
                    }
                    return false;  //"Single data file wasnt inserted or updated";
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(InsertUpdateData)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }

        public Task<bool> InsertUpdateAllData<T>(IEnumerable<T> data)
        {
            Log.Debug(TAG, $"{nameof(InsertUpdateAllData)} - {nameof(data)}.Count:{data.Count()}");

            return InsertUpdateAllData(data);
        }

        public Task<bool> InsertUpdateAllData<T>(IEnumerable<T> data, string path)
        {
            Log.Debug(TAG, $"{nameof(InsertUpdateAllData)} - {nameof(data)}.Count:{data.Count()}, {nameof(path)}:{path}");

            return Task.Factory.StartNew( () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if ( db.InsertAllAsync(data).Result != 0)
                    {                        
                        if ( db.UpdateAllAsync(data).Result != 0)
                        {
                            return true; // "List of data inserted or updated";
                        }
                    }
                    return false;  //"List of data wasnt inserted or updated";
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(InsertUpdateAllData)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }

        public Task<bool> Insert<T>(T data)
        {
            Log.Debug(TAG, $"{nameof(Insert)} - {nameof(data)}:{data}");

            return Insert(data, Path);
        }

        public Task<bool> Insert<T>(T data, string path)
        {
            Log.Debug(TAG, $"{nameof(Insert)} - {nameof(data)}:{data}, {nameof(path)}:{path}");

            return Task.Factory.StartNew( () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if ( db.InsertAsync(data).Result != 0)
                    {
                        return true; // "Single data file inserted";
                    }
                    else
                    {
                        return false;  //"Single data file wasnt inserted";
                    }
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(Insert)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }

        public Task<bool> InsertAll<T>(IEnumerable<T> data)
        {
            Log.Debug(TAG, $"{nameof(InsertAll)} - {nameof(data)}.Count:{data.Count()}");

            return InsertAll(data, Path);
        }

        public Task<bool> InsertAll<T>(IEnumerable<T> data, string path)
        {
            Log.Debug(TAG, $"{nameof(InsertAll)} - {nameof(data)}.Count:{data.Count()}, {nameof(path)}:{path}");

            return Task.Factory.StartNew( () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if ( db.InsertAllAsync(data).Result != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(InsertAll)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }

        public Task<bool> Update<T>(T data)
        {
            Log.Debug(TAG, $"{nameof(Update)} - {nameof(data)}:{data}");

            return Update(data, Path);
        }

        public Task<bool> Update<T>(T data, string path)
        {
            Log.Debug(TAG, $"{nameof(Update)} - {nameof(data)}:{data}, {nameof(path)}:{path}");

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if ( db.UpdateAsync(data).Result != 0)
                    {
                        return true;  //"Single data file updated";
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(Update)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }

        public Task<bool> UpdateAll<T>(IEnumerable<T> data)
        {
            Log.Debug(TAG, $"{nameof(UpdateAll)} - {nameof(data)}:{data}");

            return UpdateAll(data, Path);
        }

        public Task<bool> UpdateAll<T>(IEnumerable<T> data, string path)
        {
            Log.Debug(TAG, $"{nameof(DataContext)} - {nameof(data)}.Count:{data?.Count()}, {nameof(path)}:{path}");

            return Task.Factory.StartNew( () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if ( db.UpdateAllAsync(data).Result != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //return "List of data inserted";
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(UpdateAll)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }
                        
        public Task<List<T>> Select<T, U>(Expression<Func<T, bool>> whereClause, Expression<Func<T, U>> orderClause, bool ascending=true) where T : new()
        {
            Log.Debug(TAG, $"{nameof(Select)} - {nameof(whereClause)}:{whereClause}, {nameof(orderClause)}:{orderClause}");
            
            var result = Select<T, U>(whereClause, orderClause, ascending, Path);
            return result;
        }
        
        public Task<List<T>> Select<T,U>(Expression<Func<T, bool>> whereClause, Expression<Func<T, U>> orderClause, bool ascending, string path) where T:new()
        {
            Log.Debug(TAG, $"{nameof(Select)} - {nameof(whereClause)}:{whereClause}, {nameof(orderClause)}:{orderClause}, {nameof(path)}:{path}");

            Task<List<T>> result;

            var db = new SQLiteAsyncConnection(path);
            
            if (ascending)
            {
                result = db.Table<T>().Where(whereClause).OrderBy(orderClause).ToListAsync();
            }
            else
            {
                result = db.Table<T>().Where(whereClause).OrderByDescending(orderClause).ToListAsync();
            }
            
            return result;
        }

        public Task<bool> Delete<T> (T data)
        {
            Log.Debug(TAG, $"{nameof(Delete)} - {nameof(data)}:{data}");

            return Delete(data, Path);
        }

        public Task<bool> Delete<T>(T data, string path)
        {
            Log.Debug(TAG, $"{nameof(Delete)} - {nameof(data)}:{data}, {nameof(path)}:{path}");

            return Task.Factory.StartNew( () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if ( db.DeleteAsync(data).Result != 0)
                    {
                        return true;  //"Single data file updated";
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(Delete)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }

        public Task<bool> SetAllDefault<T>(bool isDefault)
        {
            Log.Debug(TAG, $"{nameof(SetAllDefault)} - {nameof(isDefault)}:{isDefault}");

            return SetAllDefault<T>(isDefault, Path);
        }

        public Task<bool> SetAllDefault<T>(bool isDefault, string path)
        {
            Log.Debug(TAG, $"{nameof(SetAllDefault)} - {nameof(isDefault)}:{isDefault}, {nameof(path)}:{path}");

            return Task.Factory.StartNew( () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);

                    string query = $"UPDATE {typeof(T).Name} SET {nameof(BaseWithDescriptionAndDefault.IsDefault)} = {(isDefault ? 1 : 0)}";
                    
                    var r = db.ExecuteAsync(query).Result;
                    if (r != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(InsertUpdateAllData)} - {nameof(ex)}:{ex.Message}");
                    throw;
                }
            });
        }
    }
}
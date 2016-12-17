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

        //public async Task<string> CreateDatabase()
        public Task<bool> CreateDatabase()
        {
            Log.Debug(TAG, $"{nameof(CreateDatabase)}");
            return CreateDatabase(Path);
        }
        
        public /*async*/ Task<bool> CreateDatabase(string path)
        {
            Log.Debug(TAG, $"{nameof(CreateDatabase)} - {nameof(path)}:{path}");

            return Task.Factory.StartNew(/*async*/ () =>
            {
                try
                {
                    var connection = new SQLiteAsyncConnection(path);


                    //await connection.CreateTableAsync<Owner>();
                    //await connection.CreateTableAsync<Category>();
                    //await connection.CreateTableAsync<PaymentType>();
                    //await connection.CreateTableAsync<Record>();
                    //await connection.CreateTableAsync<RecurringPayment>();

                    //connection.CreateTables(typeof(Owner), typeof(Category), typeof(PaymentType), typeof(Record), typeof(RecurringPayment));
                    var r = /*await*/ connection.CreateTablesAsync(typeof(Owner), typeof(Category), typeof(PaymentType), typeof(Record), typeof(RecurringPayment)).Result;

                    //if (r.Results.ContainsValue())
                    var r2 = /*await*/ (new Seed()).FillDB(this, path);
                    if (r2.Result)
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
                    Log.Error(TAG, $"{nameof(CreateDatabase)} - {nameof(ex)}:{ex.Message}");
                    throw;
                    //return ex.Message;
                }
            }
            );
        }

        public /*async*/ Task<bool> InsertUpdateData<T>(T data)
        {
            Log.Debug(TAG, $"{nameof(InsertUpdateData)} - {nameof(data)}:{data}");

            return /*await*/ InsertUpdateData(data, Path);
        }

        public /*async*/ Task<bool> InsertUpdateData<T>(T data, string path)
        {
            Log.Debug(TAG, $"{nameof(InsertUpdateData)} - {nameof(data)}:{data}, {nameof(path)}:{path}");

            return Task.Factory.StartNew(/*async*/ () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if (db.InsertAsync(data).Result != 0)
                    {
                        if (/*await*/ db.UpdateAsync(data).Result != 0)
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
                    //return ex.Message;
                }
            });
        }

        public /*async*/ Task<bool> InsertUpdateAllData<T>(IEnumerable<T> data)
        {
            Log.Debug(TAG, $"{nameof(InsertUpdateAllData)} - {nameof(data)}.Count:{data.Count()}");

            return /*await*/ InsertUpdateAllData(data);
        }

        public /*async*/ Task<bool> InsertUpdateAllData<T>(IEnumerable<T> data, string path)
        {
            Log.Debug(TAG, $"{nameof(InsertUpdateAllData)} - {nameof(data)}.Count:{data.Count()}, {nameof(path)}:{path}");

            return Task.Factory.StartNew(/*async*/ () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if (/*await*/ db.InsertAllAsync(data).Result != 0)
                    {
                        if (/*await*/ db.UpdateAllAsync(data).Result != 0)
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
                    //return ex.Message;
                }
            });
        }

        public /*async*/ Task<bool> Insert<T>(T data)
        {
            Log.Debug(TAG, $"{nameof(Insert)} - {nameof(data)}:{data}");

            return /*await*/ Insert(data, Path);
        }

        public /*async*/ Task<bool> Insert<T>(T data, string path)
        {
            Log.Debug(TAG, $"{nameof(Insert)} - {nameof(data)}:{data}, {nameof(path)}:{path}");

            return Task.Factory.StartNew(/*async*/ () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if (/*await*/ db.InsertAsync(data).Result != 0)
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
                    //return ex.Message;
                }
            });
        }

        public /*async*/ Task<bool> InsertAll<T>(IEnumerable<T> data)
        {
            Log.Debug(TAG, $"{nameof(InsertAll)} - {nameof(data)}.Count:{data.Count()}");

            return /*await*/ InsertAll(data, Path);
        }

        public /*async*/ Task<bool> InsertAll<T>(IEnumerable<T> data, string path)
        {
            Log.Debug(TAG, $"{nameof(InsertAll)} - {nameof(data)}.Count:{data.Count()}, {nameof(path)}:{path}");

            return Task.Factory.StartNew(/*async*/ () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if (/*await*/ db.InsertAllAsync(data).Result != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                    //var conn = db.GetConnection();
                    //conn.InsertAll(data);


                    //return "List of data inserted";
                }
                catch (SQLiteException ex)
                {
                    Log.Error(TAG, $"{nameof(InsertAll)} - {nameof(ex)}:{ex.Message}");
                    throw;
                    //return ex.Message;
                }
            });
        }

        public /*async*/ Task<bool> Update<T>(T data)
        {
            Log.Debug(TAG, $"{nameof(Update)} - {nameof(data)}:{data}");

            return /*await*/ Update(data, Path);
        }

        public /*async*/ Task<bool> Update<T>(T data, string path)
        {
            Log.Debug(TAG, $"{nameof(Update)} - {nameof(data)}:{data}, {nameof(path)}:{path}");

            return Task.Factory.StartNew(/*async*/ () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if (/*await*/ db.UpdateAsync(data).Result != 0)
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
                    //return ex.Message;
                }
            });
        }

        public /*async*/ Task<bool> UpdateAll<T>(IEnumerable<T> data)
        {
            Log.Debug(TAG, $"{nameof(UpdateAll)} - {nameof(data)}:{data}");

            return /*await*/ UpdateAll(data, Path);
        }

        public Task<bool> UpdateAll<T>(IEnumerable<T> data, string path)
        {
            Log.Debug(TAG, $"{nameof(DataContext)} - {nameof(data)}.Count:{data?.Count()}, {nameof(path)}:{path}");

            return Task.Factory.StartNew(/*async*/ () =>
            {
                try
                {
                    var db = new SQLiteAsyncConnection(path);
                    if (/*await*/ db.UpdateAllAsync(data).Result != 0)
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
                    //return ex.Message;
                }
            });
        }
                        
        public Task<List<T>> Select<T, U>(Expression<Func<T, bool>> whereClause, Expression<Func<T, U>> orderClause, bool ascending=true) where T : new()
        {
            Log.Debug(TAG, $"{nameof(Select)} - {nameof(whereClause)}:{whereClause}, {nameof(orderClause)}:{orderClause}");

            //var result = await Select<T>(whereClause, orderClause, Path);
            var result = Select<T, U>(whereClause, orderClause, ascending, Path);
            return result;
        }
        
        public Task<List<T>> Select<T,U>(Expression<Func<T, bool>> whereClause, Expression<Func<T, U>> orderClause, bool ascending, string path) where T:new()
        {
            Log.Debug(TAG, $"{nameof(Select)} - {nameof(whereClause)}:{whereClause}, {nameof(orderClause)}:{orderClause}, {nameof(path)}:{path}");

            Task<List<T>> result;

            var db = new SQLiteAsyncConnection(path);
            //string command = $"SELECT * FROM {nameof(T)} WHERE {whereClause} ORDER BY {orderClause}";
            //string command = $"SELECT * FROM {typeof(T).Name} WHERE {whereClause} ORDER BY {orderClause}";
            //var result = await db.ExecuteScalarAsync<IEnumerable<T>>(command);
            //var result = db.ExecuteScalarAsync<IEnumerable<T>>(command);

            //conn.Table<Stock>
            
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
    }
}
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
    public class DataContext
    {
        public string Path { get; private set; }

        public DataContext(string path)
        {
            Path = path;
        }

        public async Task<string> CreateDatabase()
        {
            return await CreateDatabase(Path);
        }

        public async Task<string> CreateDatabase(string path)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                await connection.CreateTableAsync<Owner>();
                await connection.CreateTableAsync<Category>();
                await connection.CreateTableAsync<PaymentType>();
                await connection.CreateTableAsync<Record>();
                await connection.CreateTableAsync<RecurringPayment>();
                await (new Seed()).FillDB(this, path);
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> InsertUpdateData<T>(T data)
        {
            return await InsertUpdateData(data, Path);
        }

        public async Task<string> InsertUpdateData<T>(T data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                if (await db.InsertAsync(data) != 0)
                {
                    await db.UpdateAsync(data);
                }
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> InsertUpdateAllData<T>(IEnumerable<T> data)
        {
            return await InsertUpdateAllData(data);
        }

        public async Task<string> InsertUpdateAllData<T>(IEnumerable<T> data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                if (await db.InsertAllAsync(data) != 0)
                {
                    await db.UpdateAllAsync(data);
                }
                return "List of data inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> Insert<T>(T data)
        {
            return await Insert(data, Path);
        }

        public async Task<string> Insert<T>(T data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                await db.InsertAsync(data);
                return "Single data file inserted";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> Update<T>(T data)
        {
            return await Update(data, Path);
        }

        public async Task<string> Update<T>(T data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                await db.InsertAsync(data);
                return "Single data file updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> InsertAll<T>(IEnumerable<T> data)
        {
            return await InsertAll(data, Path);
        }

        public async Task<string> InsertAll<T>(IEnumerable<T> data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                await db.InsertAllAsync(data);
                return "List of data inserted";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateAll<T>(IEnumerable<T> data)
        {
            return await UpdateAll(data, Path);
        }

        public async Task<string> UpdateAll<T>(IEnumerable<T> data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                await db.UpdateAllAsync(data);
                return "List of data inserted";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<IEnumerable<T>> Select<T>(string whereClause, string orderClause)
        {
            return await Select<T>(whereClause, orderClause, Path);
        }

        public async Task<IEnumerable<T>> Select<T>(string whereClause, string orderClause, string path)
        {
            var db = new SQLiteAsyncConnection(path);

            var result = await db.ExecuteScalarAsync<IEnumerable<T>>($"SELECT * FROM {nameof(T)} WHERE {whereClause} ORDER BY {orderClause}");

            return result;
        }
    }
}
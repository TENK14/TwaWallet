using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Database
{
    public interface IDataContext
    {
        string Path { get; }

        Task<bool> CreateDatabase();
        Task<bool> CreateDatabase(string path);
        Task<bool> Insert<T>(T data);
        Task<bool> Insert<T>(T data, string path);
        Task<bool> InsertAll<T>(IEnumerable<T> data);
        Task<bool> InsertAll<T>(IEnumerable<T> data, string path);
        Task<bool> InsertUpdateAllData<T>(IEnumerable<T> data);
        Task<bool> InsertUpdateAllData<T>(IEnumerable<T> data, string path);
        Task<bool> InsertUpdateData<T>(T data);
        Task<bool> InsertUpdateData<T>(T data, string path);
        Task<List<T>> Select<T, U>(Expression<Func<T, bool>> whereClause, Expression<Func<T, U>> orderClause, bool ascending = true) where T : new();
        Task<List<T>> Select<T, U>(Expression<Func<T, bool>> whereClause, Expression<Func<T, U>> orderClause, bool ascending, string path) where T : new();
        Task<bool> Update<T>(T data);
        Task<bool> Update<T>(T data, string path);
        Task<bool> UpdateAll<T>(IEnumerable<T> data);
        Task<bool> UpdateAll<T>(IEnumerable<T> data, string path);
        Task<bool> Delete<T>(T data);
        Task<bool> Delete<T>(T data, string path);
        Task<bool> SetAllDefault<T>(bool isDefault);
        Task<bool> SetAllDefault<T>(bool isDefault, string path);
    }
}
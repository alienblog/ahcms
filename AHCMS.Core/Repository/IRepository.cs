using System;
using System.Collections.Generic;
using System.Linq;

namespace AHCMS.Core.Repository
{
    public interface IRepository : IDisposable
    {
        object Save<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        void Delete<T>(object pk) where T : class;

        void SaveOrUpdate<T>(T entity) where T : class;

        T Get<T>(object pk) where T : class;

        T Load<T>(object pk) where T : class;

        IQueryable<T> Query<T>() where T : class;

        ITransaction BeginTransaction();
    }
}

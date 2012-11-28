using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NH = NHibernate;
using NHibernate.Linq;
using AHCMS.Core.NHibernate;

namespace AHCMS.Core.Repository
{
    public class Repository : IRepository
    {
        private NH.ISession session;
        private bool useNHibernateManager = true;

        public Repository()
        {
            session = NHibernateManager.GetCurrentSession();
            useNHibernateManager = false;
        }

        public Repository(NH.ISession session)
        {
            this.session = session;
            useNHibernateManager = false;
        }

        public NH.ISession Session
        {
            get
            {
                if (useNHibernateManager)
                {
                    return session;
                }
                else
                {
                    return NHibernateManager.GetCurrentSession();
                }
            }
        }

        public object Save<T>(T entity) where T : class
        {
            NH.ISession session = Session;
            object obj = session.Save(entity);
            session.Flush();
            return obj;
        }

        public void Update<T>(T entity)where T : class
        {
            NH.ISession session = Session;
            session.Update(entity);
            session.Flush();
        }

        public void Delete<T>(T entity)where T : class
        {
            NH.ISession session = Session;
            session.Delete(entity);
            session.Flush();
        }

        public void Delete<T>(object pk)where T : class
        {
            NH.ISession session = Session;
            var t = Get<T>(pk);
            Delete(t);
        }

        public void SaveOrUpdate<T>(T entity)where T : class
        {
            NH.ISession session = Session;
            session.SaveOrUpdate(entity);
            session.Flush();
        }

        public T Get<T>(object pk)where T : class
        {
            NH.ISession session = Session;
            return session.Get<T>(pk);
        }

        public T Load<T>(object pk)where T : class
        {
            NH.ISession session = Session;
            return session.Load<T>(pk);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return session.Query<T>();
        }

        public ITransaction BeginTransaction()
        {
            NH.ISession session = Session;
            return new Transaction(session);
        }

        public void Dispose()
        {
            if (this.session != null)
            {
                this.session.Flush();
                CloseSession();
            }
        }

        private void CloseSession()
        {
            session.Close();
            session.Dispose();
            session = null;
        }
    }
}

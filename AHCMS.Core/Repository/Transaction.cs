using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NH = NHibernate;

namespace AHCMS.Core.Repository
{
    public class Transaction:ITransaction
    {
        NH.ITransaction transaction;

        bool isOriginator = true;

        public Transaction(NH.ISession session)
        {
            transaction = session.Transaction;

            if (transaction.IsActive)
                isOriginator = false; // The method that first opened the transaction should also close it
            else
                transaction.Begin();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (isOriginator && !transaction.WasCommitted && !transaction.WasRolledBack)
                transaction.Commit();
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBack()
        {
            if (!transaction.WasCommitted && !transaction.WasRolledBack)
                transaction.Rollback();
        }

        public void Dispose()
        {
            if (isOriginator)
            {
                RollBack();
                transaction.Dispose();
            }
        }
    }
}

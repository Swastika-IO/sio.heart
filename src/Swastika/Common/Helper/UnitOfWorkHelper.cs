using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swastika.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Swastika.Common.Helper
{
    public class UnitOfWorkHelper<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <returns></returns>
        public static TDbContext InitContext()
        {
            Type classType = typeof(TDbContext);
            ConstructorInfo classConstructor = classType.GetConstructor(new Type[] { });
            TDbContext context = (TDbContext)classConstructor.Invoke(new object[] { });
            return context;
        }

        public static void HandleTransaction(bool isSucceed, bool isRoot, IDbContextTransaction transaction)
        {
            if (isSucceed)
            {
                if (isRoot)
                {
                    //if current transaction is root transaction
                    transaction.Commit();
                }
            }
            else
            {
                if (isRoot)
                {
                    //if current transaction is root transaction
                    transaction.Rollback();
                }
            }
        }

        public static RepositoryResponse<TResult> HandleException<TResult>(Exception ex, bool isRoot, IDbContextTransaction transaction)
            where TResult : class
        {
            // LogErrorMessage(ex);
            if (isRoot)
            {
                //if current transaction is root transaction
                transaction.Rollback();
            }
            List<string> errors = new List<string>();
            errors.Add(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            return new RepositoryResponse<TResult>()
            {
                IsSucceed = false,
                Data = null,
                Exception = (ex.InnerException != null ? ex.InnerException : ex),
                Errors = errors
            };
        }

        public static RepositoryResponse<TResult> HandleObjectException<TResult>(Exception ex, bool isRoot, IDbContextTransaction transaction)
            where TResult : IConvertible
        {
            // LogErrorMessage(ex);
            if (isRoot)
            {
                //if current transaction is root transaction
                transaction.Rollback();
            }
            List<string> errors = new List<string>();
            errors.Add(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            return new RepositoryResponse<TResult>()
            {
                IsSucceed = false,
                Data = default(TResult),
                Exception = (ex.InnerException != null ? ex.InnerException : ex),
                Errors = errors
            };
        }

        public static void InitTransaction(TDbContext _context, IDbContextTransaction _transaction, out TDbContext context, out IDbContextTransaction transaction, out bool isRoot)
        {
            isRoot = _context == null;
            context = _context ?? InitContext();
            transaction = _transaction ?? context.Database.BeginTransaction();
        }
    }
}

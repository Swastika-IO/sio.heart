using Microsoft.EntityFrameworkCore;
using Swastika.Infrastructure.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace Swastika.Infrastructure.Data.Repository
{
    public class DefaultRepository<TContext, TModel>: ModelRepositoryBase<TContext, TModel>         
        where TContext: DbContext
        where TModel : class
    {
        private static volatile DefaultRepository<TContext, TModel> instance;
        private static object syncRoot = new Object();

        private DefaultRepository() { }

        public static DefaultRepository<TContext, TModel> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DefaultRepository<TContext, TModel>();
                    }
                }

                return instance;
            }
        }
    }

    public class DefaultRepository<TDbContext, TModel, TView> : 
        Swastika.Infrastructure.Data.Repository.ViewRepositoryBase<TDbContext, TModel, TView>
        where TDbContext : DbContext
        where TModel : class
        where TView : Swastika.Infrastructure.Data.ViewModels.ViewModelBase<TDbContext, TModel, TView>
    {
        private static volatile DefaultRepository<TDbContext, TModel, TView> instance;
        private static object syncRoot = new Object();

        private DefaultRepository() { }

        public static DefaultRepository<TDbContext, TModel, TView> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DefaultRepository<TDbContext, TModel, TView>();
                    }
                }

                return instance;
            }
        }        
    }
}

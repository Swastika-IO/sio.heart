using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Swastika.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Swastika.Domain.Core.ViewModels
{
    public abstract class ExpandViewModelBase<TDbContext, TModel>
    where TDbContext : DbContext
    where TModel : class
    {
        //[JsonIgnore]
        //public TModel Model { get; set; }
        [JsonIgnore]
        public List<SupportedCulture> ListSupportedCulture { get; set; }
        [JsonIgnore]
        public List<string> errors = new List<string>();
        [JsonIgnore]
        public bool IsValid = true;
        public virtual void ExpandView() { }
        public virtual void ExpandModel(TModel model) { }
        public virtual void Validate()
        {
        }

        public virtual Task<bool> RemoveModelAsync(TModel model, TDbContext _context, IDbContextTransaction _transaction)
        {
            return default(Task<bool>);
        }

        public virtual Task<bool> SaveModelAsync(TModel model, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return default(Task<bool>);
        }
        public virtual Task<bool> SaveSubModelsAsync(TModel model, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return default(Task<bool>);
        }

        public virtual bool SaveModel(TModel model, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return false;
        }
        public virtual bool SaveSubModels(TModel model, TDbContext _context = null, IDbContextTransaction _transaction = null)
        {
            return false;
        }
    }
}

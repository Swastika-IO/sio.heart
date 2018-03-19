using Newtonsoft.Json;
using Swastika.Domain.Core.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Swastika.Domain.Core.ViewModels
{
    public abstract class ExpandViewModelBase<TDbContext, TModel>
    where TDbContext : DbContext
    where TModel : class
    {
        [JsonIgnore]
        public TModel Model { get; set; }
        [JsonIgnore]
        public List<SupportedCulture> ListSupportedCulture { get; set; }
        [JsonIgnore]
        public string ResponseKey { get; set; }

        [JsonIgnore]
        public List<string> errors = new List<string>();
        [JsonIgnore]
        public bool IsValid = true;
        public virtual void ExpandView(TDbContext _context = null, DbContextTransaction _transaction = null) { }
        public virtual void ExpandModel(TModel model) { }

        public virtual void Validate()
        {
        }

        public virtual Task<bool> RemoveModelAsync(TModel model, TDbContext _context, DbContextTransaction _transaction)
        {
            return default(Task<bool>);
        }

        public abstract Task<bool> SaveModelAsync(bool isSaveSubModels = false, TDbContext _context = null, DbContextTransaction _transaction = null);
        

        public virtual Task<bool> SaveSubModelsAsync(TModel parent, TDbContext _context = null, DbContextTransaction _transaction = null)
        {
            return default(Task<bool>);
        }

        public abstract bool SaveModel(bool isSaveSubModels = false, TDbContext _context = null, DbContextTransaction _transaction = null);

        public virtual bool SaveSubModels(TModel parent, TDbContext _context = null, DbContextTransaction _transaction = null)
        {
            return false;
        }
    }
}

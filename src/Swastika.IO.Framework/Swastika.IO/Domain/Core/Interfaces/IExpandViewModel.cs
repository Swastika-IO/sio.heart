using Swastika.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.Domain.Core.Interfaces
{
    public interface IExpandViewModel<TModel> where TModel : class
    {
        void ExpandView();
        void ExpandModel(TModel model);
        bool Validate(out List<string> errors);
        
    }
}

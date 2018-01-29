using System.Collections.Generic;

namespace Swastika.Domain.Core.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IExpandViewModel<TModel> where TModel : class
    {
        /// <summary>
        /// Expands the model.
        /// </summary>
        /// <param name="model">The model.</param>
        void ExpandModel(TModel model);

        /// <summary>
        /// Expands the view.
        /// </summary>
        void ExpandView();

        /// <summary>
        /// Validates the specified errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        bool Validate(out List<string> errors);
    }
}
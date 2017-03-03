using System.Collections.Generic;
using FluentValidation.Results;

namespace DiscoverValidation.Model.Data.Interface
{
    public interface IData<T>
    {
        T Entity { get; set; }

        bool? IsValid();

        IList<ValidationFailure> GetValidationFailures();
    }
}
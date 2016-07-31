namespace SPipeline.Core.Interfaces
{
    using SPipeline.Core.Models;
    using System.Collections.Generic;

    public interface IResponse : ITranslatable
    {
        bool HasError { get; }

        IEnumerable<MessageError> Errors { get; }

        bool CanContinue { get; }

        IResponse AddError(MessageError error);

        IResponse AddErrors(IEnumerable<MessageError> errors);
    }
}

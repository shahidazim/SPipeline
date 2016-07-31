namespace SPipeline.Pipeline
{
    using SPipeline.Core.Interfaces;
    using SPipeline.Core.Models;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ResponseBase : IResponse
    {
        protected readonly List<MessageError> _errors = new List<MessageError>();

        public IEnumerable<MessageError> Errors => _errors;

        public bool HasError => _errors.Count > 0;

        public bool CanContinue
        {
            get { return _errors.Count == 0 || Errors.All(x => x.CanContinue); }
        }

        public IResponse AddError(MessageError error)
        {
            _errors.Add(error);
            return this;
        }

        public IResponse AddErrors(IEnumerable<MessageError> errors)
        {
            _errors.AddRange(errors);
            return this;
        }
    }
}

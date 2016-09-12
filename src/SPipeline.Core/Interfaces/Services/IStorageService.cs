namespace SPipeline.Core.Interfaces.Services
{
    using System;
    using System.Collections.Generic;

    public interface IStorageService
    {
        Uri Uplaod(string content, string reference);

        string Download(string reference);

        void Delete(string reference);

        IEnumerable<string> GetAllReferences();
    }
}

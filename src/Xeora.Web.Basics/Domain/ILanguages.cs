﻿using System;
using System.Collections.Generic;

namespace Xeora.Web.Basics.Domain
{
    public interface ILanguages : IEnumerable<string>, IDisposable
    {
        ILanguage Current { get; }
        void Use(string languageID);

        ILanguageDefinition this[string languageID] { get; }
    }
}

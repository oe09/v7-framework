﻿using System;
using Xeora.Web.Basics.Mapping;

namespace Xeora.Web.Basics.Domain
{
    public interface ISettings : IDisposable
    {
        IConfigurations Configurations { get; }
        IServices Services { get; }
        IURL Mappings { get; }
    }
}

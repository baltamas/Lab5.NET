﻿using System;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Options;

namespace UnitTests
{
    public class OperationalStoreOptionsForTests : IOptions<OperationalStoreOptions>
    {
        public OperationalStoreOptions Value => new OperationalStoreOptions();
    }
}
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#if !WINDOWS_PHONE_APP
using Serilog;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IdentityModel.HttpSigning.Tests
{
    public class Setup
    {
        [Fact]
        public void logging()
        {
#if !WINDOWS_PHONE_APP
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Trace()
                .CreateLogger();
#endif
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities.Helpers
{
    public static class ConfigStaticHelper
    {
        #if Release
                private const string DefaultEnvironmentName = "Release";
        #else
                private const string DefaultEnvironmentName = "Development";
        #endif

        public static string GetEnvironmentName()
        {
            return DefaultEnvironmentName;
        }
    }
}

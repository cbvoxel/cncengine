using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CncEngine.Common;

namespace CncEngine.Server
{
    public class ModulesConfig
    {
        internal static IDictionary<Type, ModuleConfiguration> ModuleConfigurations = new Dictionary<Type, ModuleConfiguration> ();

        public static void RegisterModule(IModule module)
        {
            var config = new ModuleConfiguration();
            module.Configure(config);
            ModuleConfigurations[module.GetType()] = config;

            RoutesConfig.AddRoute(config.ModuleHttpRoute, module.GetType());
        }
    }
}

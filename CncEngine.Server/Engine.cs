using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using CncEngine.Common;
using log4net;

namespace CncEngine.Server
{
    public class Engine
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(Engine));

        public Engine()
        {
            var doc = XDocument.Load(@"Modules/Modules.xml");
            var modules = doc.Root.Descendants("Module");
            var dllFiles = modules.Select(e => string.Format(@"Modules/{0}/{0}.dll", e.Value));
                
            var loadedAssemblies = dllFiles.Select(Assembly.LoadFrom);

            var allModules = loadedAssemblies
                .SelectMany(a => a.GetTypes())
                .Where( a => a.GetInterface("IModule") != null);

            Logger.DebugFormat("Found Modules:");
            Logger.Debug(allModules);

            var instanciatedModules = allModules
                .Select(Activator.CreateInstance);

            instanciatedModules
                .Cast<IModule>()
                .ToList()
                .ForEach(ModulesConfig.RegisterModule);

            Logger.DebugFormat("Configured Modules:");
            Logger.Debug(ModulesConfig.ModuleConfigurations.Keys);
        }

        public void Run()
        {
            var httpServer = new HttpServer();
            Logger.Info("Engine Running.");
            httpServer.RunServer(new CancellationToken()).Wait();
        }
    }
}

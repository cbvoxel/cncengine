using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
            var doc = XDocument.Load(@"Modules\Modules.xml");
            var modules = doc.Root.Descendants("Module");
            var dllFiles = modules.Select(e => @"Modules\" + e.Value + ".dll");
                
            var loadedAssemblies = dllFiles.Select(Assembly.LoadFrom);

            var instanciatedModules = loadedAssemblies
                .Select(dll => dll.CreateInstance(dll.GetName().Name + ".Module"))
                .Where(m => m != null);

            instanciatedModules
                .Cast<IModule>()
                .ToList()
                .ForEach(ModulesConfig.RegisterModule);
        }

        public void Run()
        {
            var httpServer = new HttpServer();
            httpServer.RunServer(new CancellationToken()).Wait();
        }
    }
}

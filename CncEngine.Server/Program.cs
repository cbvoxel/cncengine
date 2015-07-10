using System.IO;
using log4net;

namespace CncEngine.Server
{
    class Program
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(Engine));

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("Engine Startup");
            var engine = new Engine();
            engine.Run();
        }
    }
}

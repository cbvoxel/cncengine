using System.IO;
using System.Timers;
using CncEngine.Common.Exceptions;
using CncEngine.Common.Http;

namespace CncEngine.Common
{
    public class ModuleConfiguration
    {
        protected int ConfigSet;

        public HttpRoute ModuleHttpRoute { get; private set; }
        public Timer PollingTimer { get; private set; }
        public DirectoryInfo ListenDirectory { get; set; }

        public ModuleConfiguration()
        {
            ModuleHttpRoute = new HttpRoute();
            PollingTimer = new Timer();
        }

        public void ConfigureRoute(string host, string path, int port)
        {
            if(ConfigSet != 0 && ConfigSet != 1)
                throw new AlreadyConfiguredException();
            ModuleHttpRoute.Host = host;
            ModuleHttpRoute.Path = path;
            ModuleHttpRoute.Port = port;
            ConfigSet = 1;
        }

        public void ConfigurePollingTime(double pollingTime)
        {
            if (ConfigSet != 0 && ConfigSet != 2)
                throw new AlreadyConfiguredException();
            PollingTimer.Interval = pollingTime;
            PollingTimer.Enabled = true;
            PollingTimer.AutoReset = true;
            ConfigSet = 2;
        }

        public void AddDirectoryChangeListener(string path)
        {
            if (ConfigSet != 0 && ConfigSet != 3)
                throw new AlreadyConfiguredException();
            ListenDirectory = new DirectoryInfo(path);
            ConfigSet = 3;
        }
    }
}

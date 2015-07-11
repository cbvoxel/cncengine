/**
    Copyright (C) 2015  Carsten Blank

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CncEngine.Common.Http
{
    public class HttpListenerConfiguration
    {
        public string Host { get; set; }
        public string BasePath { get; set; }
        public int Port { get; set; }
    }
}

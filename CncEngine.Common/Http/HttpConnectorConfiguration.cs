using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CncEngine.Common.Http
{
    public class HttpConnectorConfiguration
    {
        public string Host { get; set; }
        public string BasePath { get; set; }
        public int Port { get; set; }
        public IDictionary<string,string> Query { get; set; }
        public IDictionary<string, string> Headers { get; set; }

        public HttpConnectorConfiguration()
        {
            Query = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
        }
    }
}

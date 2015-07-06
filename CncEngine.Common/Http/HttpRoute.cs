using System;

namespace CncEngine.Common.Http
{
    public class HttpRoute
    {
        public string Host { get; set; }
        public string Path { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return String.Format("http://{0}:{1}{2}", Host, Port, Path);
        }

    }
}

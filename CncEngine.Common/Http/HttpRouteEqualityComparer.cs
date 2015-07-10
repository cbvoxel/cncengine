using System.Collections.Generic;
using log4net;

namespace CncEngine.Common.Http
{
    public class HttpRouteEqualityComparer : IEqualityComparer<HttpRoute>
    {
        public bool Equals(HttpRoute x, HttpRoute y)
        {
            var result =  (x.Host == "+" || y.Host == "+" || x.Host == y.Host) && x.Port == y.Port && x.Path == y.Path;
            return result;
        }

        public int GetHashCode(HttpRoute obj)
        {
            var result = obj.Host.GetHashCode() ^ obj.Port.GetHashCode() ^ obj.Path.GetHashCode();
            return result;
        }
    }
}

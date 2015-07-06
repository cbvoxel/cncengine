using System.Collections.Generic;

namespace CncEngine.Common.Http
{
    public class HttpRouteEqualityComparer : IEqualityComparer<HttpRoute>
    {
        public bool Equals(HttpRoute x, HttpRoute y)
        {
            return x.Host == y.Host && x.Port == y.Port && x.Path == y.Path;
        }

        public int GetHashCode(HttpRoute obj)
        {
            return obj.Host.GetHashCode() ^ obj.Port.GetHashCode() ^ obj.Path.GetHashCode();
        }
    }
}

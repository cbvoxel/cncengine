using System;
using System.Collections.Generic;
using CncEngine.Common;
using CncEngine.Common.Http;

namespace CncEngine.Server
{
    public class RoutesConfig
    {
        public static IDictionary<HttpRoute, Type> ConfiguredRoutes { get; internal set; }

        static RoutesConfig()
        {
            ConfiguredRoutes = new Dictionary<HttpRoute, Type>(new HttpRouteEqualityComparer());
        }

        public static void AddRoute(HttpRoute httpRoute, Type type)
        {
            if(httpRoute == null) return;
            ConfiguredRoutes[httpRoute] = type;
        }
    }
}

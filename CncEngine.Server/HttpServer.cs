using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CncEngine.Common;
using CncEngine.Common.Http;
using log4net;

namespace CncEngine.Server
{
    public class HttpServer
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(HttpServer));

        private readonly HttpListener _httpListener;

        public HttpServer()
        {
            _httpListener = new HttpListener();
        }

        public async Task RunServer(CancellationToken token)
        {
            _httpListener.Prefixes.Clear();
            RoutesConfig.ConfiguredRoutes
                .Select(r => r.Key.ToString())
                .ToList()
                .ForEach(_httpListener.Prefixes.Add);

            _httpListener.Start();
            while (!token.IsCancellationRequested)
            {
                var context = await _httpListener.GetContextAsync();
                var message = context.Request.GetMessage();

                // Reflection
                var type = RoutesConfig.ConfiguredRoutes[new HttpRoute { Host = context.Request.Url.Host, Path = context.Request.Url.LocalPath, Port = context.Request.Url .Port}];
                var ctor = type.GetConstructor(new Type[0]);
                var module = (IModule)ctor.Invoke(new object[0]);

                // Process
                module.Process(message);
                message.CreateResponse(context.Response);

                // Return Response
                context.Response.Close();
            }

            _httpListener.Stop();
        }
    }
}

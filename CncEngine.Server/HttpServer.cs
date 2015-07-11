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

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CncEngine.Common;
using CncEngine.Common.Exceptions;
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

                ExecuteProcess(context, message);

                // Return Response
                message.CreateResponse(context.Response);
                context.Response.Close();
            }

            _httpListener.Stop();
        }

        private static void ExecuteProcess(HttpListenerContext context, Message message)
        {
            Logger.DebugFormat("Entry Url={0}", context.Request.Url);
            IModule module = null;
            try
            {
                // Reflection
                var plusRoute = new HttpRoute
                {
                    Host = "+",
                    Path = context.Request.Url.LocalPath,
                    Port = context.Request.Url.Port
                };
                var route = new HttpRoute
                {
                    Host = context.Request.Url.Host,
                    Path = context.Request.Url.LocalPath,
                    Port = context.Request.Url.Port
                };
                Type type;
                if (!RoutesConfig.ConfiguredRoutes.TryGetValue(plusRoute, out type))
                    if (!RoutesConfig.ConfiguredRoutes.TryGetValue(route, out type))
                    {
                        throw new ModuleConfigurationException(String.Format("Could not find module for route={0}", route), null);
                    }

                Logger.DebugFormat("Configured Module={0}", type.FullName);
                var ctor = type.GetConstructor(new Type[0]);
                module = (IModule) ctor.Invoke(new object[0]);

                // Process
                Logger.DebugFormat("Message process executing.");
                module.Process(message);
                Logger.DebugFormat("Message process performed.");
            }
            catch (Exception e)
            {
                Logger.Error(message.ToString(), e);
                if (module != null)
                    module.ExceptionHandling(e, message);
                else
                    message.SetPayload(e.ToString());
            }
        }
    }
}

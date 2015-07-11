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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CncEngine.Common.Http
{
    public static class HttpListenerExtensions
    {
        private static ILog Logger = LogManager.GetLogger(typeof(HttpListenerExtensions));

        public static ModuleConfiguration HttpListener(this ModuleConfiguration moduleConfig, HttpListenerConfiguration config, string path)
        {
            Logger.DebugFormat("Step");

            moduleConfig.ConfigureRoute(config.Host, config.BasePath + path, config.Port);

            return moduleConfig;
        }

        public static Message GetMessage(this HttpListenerRequest request)
        {
            Logger.DebugFormat("Step");

            var message = new Message();
            var bytes = new byte[request.InputStream.Length];
            var task = request.InputStream.ReadAsync(bytes, 0, bytes.Length);

            message.Variables.Add("Headers", new WebHeaderCollection { request.Headers });
            message.Variables.Add("ContentEncoding", request.ContentEncoding);
            message.Variables.Add("ContentType", request.ContentType);

            task.Wait();
            message.SetPayload(Encoding .UTF8.GetString(Encoding.Convert(request.ContentEncoding, Encoding.UTF8, bytes )));
            return message;
        }

        public static void CreateResponse(this Message message, HttpListenerResponse response)
        {
            Logger.DebugFormat("Step");

            object value;
            
            message.Variables.TryGetValue("ContentEncoding", out value);
            var encoding = (Encoding) value ?? Encoding.UTF8;
            response.ContentEncoding = encoding;

            message.Variables.TryGetValue("ContentType", out value);
            response.ContentType = (string) value ?? "text/plain";

            message.Variables.TryGetValue("Headers", out value);
            response.Headers = (WebHeaderCollection) value ?? new WebHeaderCollection();

            response.StatusCode = 200;

            var bytes = message.Payload.ToBytes();
            bytes = Encoding.Convert(Encoding.UTF8, encoding, bytes);
            response.OutputStream.Write(bytes, 0, bytes.Length);
        }
    }
}

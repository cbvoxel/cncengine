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
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CncEngine.Common.Http
{
    public static class HttpConnectorExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HttpConnectorExtensions));

        public static Message HttpPostMessage(this Message message, HttpConnectorConfiguration config, string relativePath, string contentType = "text/xml")
        {
            Logger.DebugFormat("Step");

            var client = new System.Net.WebClient();
            var url = String.Format("http://{0}:{1}{2}", config.Host, config.Port, config.BasePath);

            config.Headers.ToList().ForEach(h => client.Headers.Add(h.Key, h.Value));
            message.Variables.Where(v => v.Key.StartsWith("HttpHeader")).ToList().ForEach(h => client.Headers.Add(h.Key.Replace("HttpHeader", ""), h.Value.ToString()));
            config.Query.ToList().ForEach(h => client.QueryString.Add(h.Key, h.Value));
            message.Variables.Where(v => v.Key.StartsWith("HttpQuery")).ToList().ForEach(h => client.QueryString.Add(h.Key.Replace("HttpQuery", ""), h.Value.ToString()));
            client.Encoding = Encoding.UTF8;

            try
            {
                message.SetPayload(client.UploadString(url, message.Payload.FirstNode.ToString()));
                message.Variables["ContentType"] = contentType;
            }
            catch (WebException ex)
            {
                message.SetPayload(ex.Response.GetResponseStream());
                message.Variables["ContentType"] = "text/plain";
            }
           
            return message;
        }
    }
}
;
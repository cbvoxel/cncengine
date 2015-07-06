﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CncEngine.Common.Http
{
    public static class HttpConnectorExtensions
    {
        public static Message HttpPostMessage(this Message message, HttpConnectorConfiguration config, string relativePath)
        {
            var client = new System.Net.WebClient();
            var url = String.Format("http://{0}:{1}{2}", config.Host, config.Port, config.BasePath);

            config.Headers.ToList().ForEach(h => client.Headers.Add(h.Key, h.Value));
            message.Variables.Where(v => v.Key.StartsWith("HttpHeader")).ToList().ForEach(h => client.Headers.Add(h.Key.Replace("HttpHeader", ""), h.Value.ToString()));
            config.Query.ToList().ForEach(h => client.QueryString.Add(h.Key, h.Value));
            message.Variables.Where(v => v.Key.StartsWith("HttpQuery")).ToList().ForEach(h => client.QueryString.Add(h.Key.Replace("HttpQuery", ""), h.Value.ToString()));
            client.Encoding = Encoding.UTF8;

            try
            {
                message.SetPayload(client.UploadString(url, message.Payload.ToString()));
            }
            catch (WebException ex)
            {
                message.SetPayload(ex.Response.GetResponseStream());
            }
           
            return message;
        }
    }
}
;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CncEngine.Common.Http
{
    public static class HttpListenerExtensions
    {
        public static ModuleConfiguration HttpListener(this ModuleConfiguration moduleConfig, HttpListenerConfiguration config, string path)
        {
            moduleConfig.ConfigureRoute(config.Host, config.BasePath + path, config.Port);

            return moduleConfig;
        }

        public static Message GetMessage(this HttpListenerRequest request)
        {
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
            var encoding = (Encoding)message.Variables["ContentEncoding"];

            var bytes = message.Payload.ToBytes();
            bytes = Encoding.Convert(Encoding.UTF8, encoding, bytes);
            var task = response.OutputStream.WriteAsync(bytes, 0, bytes.Length);

            response.ContentEncoding = encoding;
            response.ContentType = (String)message.Variables["ContentType"];
            response.Headers = (WebHeaderCollection)message.Variables["Headers"];

            response.StatusCode = 200;

            task.Wait();
        }
    }
}

using System.IO;
using CncEngine.Common;
using CncEngine.Common.Http;

namespace CncEngine.Example.GetToken
{
    public static class GetTokenExtensions
    {
        public static Message GetToken(this Message message, HttpConnectorConfiguration config, string username, string password)
        {
            var xml = File.ReadAllText(@"Modules\GetToken\GetTokenRequest.xml");
            message.SetPayload(xml.Replace("%username%", username).Replace("%password%", password));
            message.SetVariable("HttpHeaderSoapAction", "GetAuthentificationToken");
            message.LogDebug();
            message.HttpPostMessage(config, "");
            return message;
        }
    }
}

using CncEngine.Common;
using CncEngine.Common.Http;
using CncEngine.Example.GetToken;

namespace CncEngine.Example
{
    public class Module : IModule
    {
        private readonly HttpConnectorConfiguration _plentyConnector = new HttpConnectorConfiguration
        {
            Host = "",
            Port = 80,
            BasePath = "/"
        };

        public void Configure(ModuleConfiguration context)
        {
            var httpListenerConfig = new HttpListenerConfiguration
            {
                Host = "127.0.0.1",
                BasePath = "/",
                Port = 8081
            };
            context.HttpListener(httpListenerConfig, "test/");
        }

        public void Process(Message message)
        {
            message.GetToken(_plentyConnector, "", "");
            message.ExtractVariable("//Token", "Token"); // TODO
        }
    }
}

using System;
using CncEngine.Common;
using CncEngine.Common.Http;
using CncEngine.Common.Log;
using CncEngine.Common.Xml;
using log4net;

namespace CncEngine.Example.GetToken
{
    public static class GetTokenExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GetTokenExtensions));

        public static Message LoadToken(this Message message, HttpConnectorConfiguration plentyConfig)
        {
            return message
                .LoadTempToken()
                .If(m => m.Variables.ContainsKey("Token")
                         && !string.IsNullOrEmpty((string) m.Variables["Token"]))
                .Then(Message.NoChange)
                .Else(m =>
                    m
                        .SetPayload(Resources.LoadModuleResourceTextFile<Module>(@"PlentyConfig.xml"))
                        .ClearVariables()
                        .XslTransformFromModuleResource<Module>(@"GetToken/GetTokenTransform.xsl")
                        .SetVariable("HttpHeaderSoapAction", "GetAuthentificationToken")
                        .LogDebug(me => String.Format("Configured Request:\r\n{0}", me))
                        .HttpPostMessage(plentyConfig, "")
                        .LogDebug(me => String.Format("Received Response:\r\n{0}", me))
                        .ClearVariables()
                        .ExtractVariable("//Token", "Token")
                        .SaveTempToken()
                )
                .EndIf();
        }

        public static Message LoadTempToken(this Message message)
        {
            Logger.Debug("Step");
            var tempTokenFileInfo = Resources.ResourceFileInfo<Module>("GetToken/Token.temp");
            if (tempTokenFileInfo.Exists && tempTokenFileInfo.CreationTime.Day == DateTime.Now.Day)
            {
                var token =  Resources.LoadModuleResourceTextFile<Module>("GetToken/Token.temp");
                message.SetVariable("Token", token);
                Logger.DebugFormat("Token loaded from temp file. Token={0}", token);
            }
            return message;
        }

        public static Message SaveTempToken(this Message message)
        {
            Logger.Debug("Step");
            Resources.SaveModuleResourceTextFile<Module>("GetToken/Token.temp", 
                (string)message.Variables["Token"]);
            Logger.DebugFormat("Token saved to temp file. Token={0}", (string)message.Variables["Token"]);
            return message;
        }
    }
}

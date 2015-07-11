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
using CncEngine.Common;
using CncEngine.Common.Http;
using CncEngine.Common.Log;
using CncEngine.Common.Xml;
using CncEngine.Common.Xml.Xslt;
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
                         && !string.IsNullOrEmpty((string)m.Variables["Token"]))
                .Then(Message.NoChange)
                .Else(m =>
                    m
                        .SetPayload(Resources.LoadModuleResourceXml<Module>(@"PlentyConfig.xml"))
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
                .EndIf()
                ;
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

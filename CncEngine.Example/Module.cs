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
using System.Xml.Linq;
using System.Xml.XPath;
using CncEngine.Common;
using CncEngine.Common.Db;
using CncEngine.Common.Db.MsSql;
using CncEngine.Common.Exceptions;
using CncEngine.Common.Http;
using CncEngine.Common.Log;
using CncEngine.Common.Xml;
using CncEngine.Common.Xml.Xslt;
using CncEngine.Example.ItemAttributes;

namespace CncEngine.Example
{
    public class Module : IModule
    {
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static string Path { get; set; }

        static Module()
        {
            try
            {
                var xml = Resources.LoadModuleResourceXml<Module>(@"PlentyConfig.xml");
                Username = xml.Descendants("Username").First().Value;
                Password = xml.Descendants("Password").First().Value;
                Host = xml.Descendants("Host").First().Value;
                Port = Int32.Parse(xml.Descendants("Port").First().Value);
                Path = xml.Descendants("Path").First().Value;
            }
            catch (Exception e)
            {
                throw new ModuleConfigurationException("Error Initializing Module.", e);
            }
        }

        private readonly HttpConnectorConfiguration _plentyConnector = new HttpConnectorConfiguration
        {
            Host = Host,
            Port = Port,
            BasePath = Path
        };

        public void Configure(ModuleConfiguration context)
        {
            var httpListenerConfig = new HttpListenerConfiguration
            {
                Host = "+",
                BasePath = "/",
                Port = 8081
            };
            context.HttpListener(httpListenerConfig, "test/");
        }

        public void Process(Message message)
        {
            message
                .GetItemAttributes(_plentyConnector)
                .ExtractVariable("//Attributes", "ItemAttributes", XmlExtensions.NodeType.Node)
                .LoadModuleResourceXml<Module>("PlentyConfig.xml")
                .ExtractVariable("//PlentyConfig", "PlentyConfig", XmlExtensions.NodeType.Node)
                .ExtractHost("//LeoveServer1/Host")
                .ExtractPort("//LeoveServer1/Port")
                .ExtractUsername("//LeoveServer1/Username")
                .ExtractPassword("//LeoveServer1/Password")
                .ExtractDatabase("//LeoveServer1/Database")
                .MsSqlSelect("SELECT DISTINCT TOP(1) Type FROM [CncEngine].[dbo].[TranslationTable] WHERE Type LIKE 'Size.%' ORDER BY Type")
                .SplitMessages("//Type")
                .ForEach(m => m
                    .SetVariable("CurrentType", m.Payload.Value)
                    .MsSqlSelect(String.Format("SELECT DISTINCT Term FROM [CncEngine].[dbo].[TranslationTable] WHERE Type = '{0}' ORDER BY Term", m.Payload.Value))
                    .SplitMessages("//Term")
                    .ForEach(mm => mm
                        .VariableToPayload("CurrentType")
                        .AddToPayload(mm.Variables["PlentyConfig"])
                        .AddToPayload(mm.Variables["ItemAttributes"].XmlXPath("//item[BackendName='" + mm.Variables["CurrentType"] + "']").FirstOrDefault())
                        .XslTransformFromModuleResource<Module>("ItemAttributes/Login2AddItemAttribute.xsl")
                    )
                    .Combine()
                //.VariableToPayload("CurrentType")
                //.Combine(mm => m.SetPayload(XElement.Parse(mm.Variables["PlentyConfig"].ToString())))
                //.Combine(mm => m.SetPayload(mm.Variables["ItemAttributes"].ToString().ExtractXPath("//item[BackendName='" + mm.Variables["currentType"] + "']", XmlExtensions.NodeType.Node)))
                )
                .Combine()
                .SetVariable("ContentType", "text/xml")
                ;
        }

        public void ExceptionHandling(Exception ex, Message message)
        {
            //message.SetVariable("ExceptionPayload", message.Payload.ToString());
            message.SetPayload("<Exception></Exception>");
        }
    }
}

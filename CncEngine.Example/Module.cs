﻿using System;
using System.Linq;
using CncEngine.Common;
using CncEngine.Common.Exceptions;
using CncEngine.Common.Http;
using CncEngine.Common.Log;
using CncEngine.Common.Xml;
using CncEngine.Example.GetToken;
using CncEngine.Example.ItemAttributes;
using log4net;

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
                Username = xml.Root.Descendants("Username").First().Value;
                Password = xml.Root.Descendants("Password").First().Value;
                Host = xml.Root.Descendants("Host").First().Value;
                Port = Int32.Parse(xml.Root.Descendants("Port").First().Value);
                Path = xml.Root.Descendants("Path").First().Value;
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
                .SetVariable("ContentType", "text/xml")
                .SetPayload("<Root>Test</Root>");
        }

        public void ExceptionHandling(Exception ex, Message message)
        {
            message.SetVariable("ExceptionPayload", message.Payload.ToString());
            message.SetPayload(ex.ToString());
        }
    }
}

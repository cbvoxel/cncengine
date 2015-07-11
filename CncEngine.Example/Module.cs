﻿/**
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
using CncEngine.Common;
using CncEngine.Common.Db;
using CncEngine.Common.Db.MsSql;
using CncEngine.Common.Exceptions;
using CncEngine.Common.Http;

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
                .LoadModuleResourceTextFile<Module>("PlentyConfig.xml")
                .ExtractHost("//LeoveServer1/Host")
                .ExtractPort("//LeoveServer1/Port")
                .ExtractUsername("//LeoveServer1/Username")
                .ExtractPassword("//LeoveServer1/Password")
                .ExtractDatabase("//LeoveServer1/Database")
                .MsSqlSelect("SELECT TOP(10) * FROM TranslationTable [TT]; SELECT TOP(10) * FROM ActionReports [AR];")
                //.SetPayload("<Root>Test</Root>")
                ;
        }

        public void ExceptionHandling(Exception ex, Message message)
        {
            message.SetVariable("ExceptionPayload", message.Payload.ToString());
            message.SetPayload(ex.ToString());
        }
    }
}

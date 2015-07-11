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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using log4net;

namespace CncEngine.Common.Xml
{
    public static class XsltExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(XsltExtensions));

        public static Message XslTransform(this Message message, Stream xslt)
        {
            Logger.Debug("Step");
            var xsltDoc = new XPathDocument(xslt);
            var payloadDoc = new XPathDocument(message.Payload.ToStream());
            var output = new StringBuilder();

            var transform = new XslCompiledTransform();
            transform.Load(xsltDoc);

            transform.Transform(payloadDoc, XmlWriter.Create(output));

            return message.SetPayload(output.ToString());
        }

        public static Message XslTransform(this Message message, byte[] xslt)
        {
            return message.XslTransform(new MemoryStream(xslt));
        }

        public static Message XslTransform(this Message message, string xslt)
        {
            return message.XslTransform(Encoding.UTF8.GetBytes(xslt));
        }

        public static Message XslTransform(this Message message, FileInfo file)
        {
            using (var stream = file.OpenRead())
                return message.XslTransform(stream);
        }

        public static Message XslTransformFromVariable(this Message message, string variableName)
        {
            var xslt = message.Variables[variableName];
            if (xslt is string)
                return message.XslTransform((string) xslt);
            if (xslt is Stream)
                return message.XslTransform((Stream)xslt);
            if (xslt is byte[])
                return message.XslTransform((byte[])xslt);
            
            throw new InvalidCastException("Variable not string, byte[] or Stream.");
        }

        public static Message XslTransformFromMessage(this Message message, Func<Message, string> xsltSelector)
        {
            return message.XslTransform(xsltSelector(message));
        }

        public static Message XslTransformFromMessage(this Message message, Func<Message, byte[]> xsltSelector)
        {
            return message.XslTransform(xsltSelector(message));
        }

        public static Message XslTransformFromMessage(this Message message, Func<Message, Stream> xsltSelector)
        {
            return message.XslTransform(xsltSelector(message));
        }

        public static Message XslTransformFromModuleResource<T>(this Message message, string relativePath) where T : IModule
        {
            var fileInfo = Resources.ResourceFileInfo<T>(relativePath);
            return message.XslTransform(fileInfo);
        }
    }
}

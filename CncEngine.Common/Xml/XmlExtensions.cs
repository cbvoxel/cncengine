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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CncEngine.Common.Xml
{
    public static class XmlExtensions
    {
        public enum NodeType
        {
            Node,
            NodeList,
            Value
        }

        public static string XmlBeautify(this XDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        public static IEnumerable<XElement> XmlXPath(this string input, string xPath)
        {
            var value = input.ToXml().XPathSelectElements(xPath);
            return value;
        }

        public static IEnumerable<XElement> XmlXPath(this object input, string xPath)
        {
            try
            {
                var value = XElement.Parse(input.ToString()).XPathSelectElements(xPath);
                return value;
            }
            catch (Exception e)
            {
                return new XElement[0];
            }
        }

        public static string ExtractXPath(this string input, string xPath, NodeType nodeType = NodeType.Value)
        {
            var value = input.XmlXPath(xPath);

            switch (nodeType)
            {
                case NodeType.Node:
                    return value.Select(x => x.ToString()).FirstOrDefault();
                case NodeType.NodeList:
                    return string.Join("\r\n", value.Select(x => x.ToString()));
                case NodeType.Value:
                    return value.Select(x => x.Value).FirstOrDefault();
                default:
                    return value.Select(x => x.ToString()).FirstOrDefault();
            }
        }

        public static string ToXml<T>(this IDictionary<string, T> dict)
        {
            var doc = new XElement("Dictionary");
            foreach (KeyValuePair<string, T> entry in dict)
            {
                doc.Add(new XElement(entry.Key, entry.Value));
            }
            return doc.ToString();
        }

        public static string ToXml<T>(this IList<T> list)
        {
            var doc = new XElement("List");
            foreach (object entry in list)
            {
                doc.Add(new XElement("Entry", entry));
            }
            return doc.ToString();
        }

        public static string ToXml<T>(this T[] array)
        {
            var doc = new XElement("List");
            foreach (object entry in array)
            {
                doc.Add(new XElement("Entry", entry));
            }
            return doc.ToString();
        }

        public static XElement ToXml(this string input)
        {
            return XElement.Parse(input);
        }
    }
}

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
using CncEngine.Common.Ctrl.ForEach;
using CncEngine.Common.Ctrl.IfThenElse;
using CncEngine.Common.Xml;
using log4net;

namespace CncEngine.Common
{
    public static class MessageExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageExtensions));

        public static Message ExtractVariable(this Message msg, string xPath, string variableName, XmlExtensions.NodeType nodeType = XmlExtensions.NodeType.Value)
        {
            Logger.Debug("Step");
            var value = msg.Payload.ToString();
            msg.Variables[variableName] = value.ExtractXPath(xPath, nodeType);
            return msg;
        }

        public static MessageCollection SplitMessages(this Message msg, string xPath)
        {
            return new MessageCollection(msg, xPath);
        }

        public static IfExecutor If(this Message message, Func<Message, Boolean> expressionEvaluator)
        {
            Logger.Debug("Step");
            return new IfExecutor(message, expressionEvaluator);
        }

        public static Message Combine(this Message message, Func<Message, Message> otherMessage)
        {
            var oMessage = otherMessage(message.Clone());
            return message.Combine(oMessage);
        }

        public static Message Combine(this Message message, Message oMessage)
        {
            Logger.Debug("Step");

            var payloadCollectionRoot = message.Payload.Element("PayloadCollection");
            if(payloadCollectionRoot == null)
            {
                payloadCollectionRoot = new XElement("PayloadCollection", message.Payload);
                message.SetPayload(payloadCollectionRoot);
            }

            payloadCollectionRoot.Add(oMessage.Payload);

            return message;
        }

        public static Message LoadModuleResourceTextFile<T>(this Message message, string relativePath) where T : IModule
        {
            return message.SetPayload(Resources.LoadModuleResourceTextFile<T>(relativePath));
        }

        public static Message LoadModuleResourceXml<T>(this Message message, string relativePath) where T : IModule
        {
            return message.SetPayload(Resources.LoadModuleResourceXml<T>(relativePath).ToString());
        }

        public static Message VariableToPayload(this Message message, string variableName)
        {
            message.Payload.Add(new XElement(variableName, message.Variables[variableName].ToString()));
            return message;
        }
    }
}

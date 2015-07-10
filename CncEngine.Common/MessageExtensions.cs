using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using CncEngine.Common.Ctrl;
using log4net;

namespace CncEngine.Common
{
    public static class MessageExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (MessageExtensions));

        public static Message ExtractVariable(this Message msg, string xPath, string variableName)
        {
            Logger.Debug("Step");
            var doc = msg.Payload.ToXDocument();
            if(doc == null)
                throw new InvalidOperationException("Not an xml document.");
            if (doc.Root == null)
                throw new InvalidOperationException("No root in xml document.");
            var value = doc.Root.XPathSelectElements(xPath).Select(x => x.Value).FirstOrDefault();
            msg.Variables[variableName] = value;
            return msg;
        }

        public static IEnumerable<Message> SplitMessages(this Message msg, string xPath)
        {
            var doc = msg.Payload.ToXDocument();
            if (doc == null)
                throw new InvalidOperationException("Not an xml document.");
            if (doc.Root == null)
                throw new InvalidOperationException("No root in xml document.");
            return doc.Root.Descendants(xPath).Select(n => new Message()
            {
                Variables = msg.Variables,
                Payload = new Payload() { PayloadAsString = n.ToString() }
            });

        }

        public static IfExecutor If(this Message message, Func<Message, Boolean> expressionEvaluator)
        {
            Logger.Debug("Step");
            return new IfExecutor(message, expressionEvaluator);
        }

        public static Message Combine(this Message message, Func<Message, Message> otherMessage)
        {
            var oMessage = otherMessage(new Message());

            if (message.Payload.ToXDocument().Root.Name == "MessageCollection")
            {
                message.Payload.ToXDocument().Root.Add(oMessage.Payload.ToString());
            }
            else
            {
                var thisPayload = message.Payload.ToXDocument().Root;
                var otherPayload = oMessage.Payload.ToXDocument().Root;
                var doc = new XDocument(new XElement("MessageCollection", thisPayload, otherPayload));
                message.SetPayload(doc.ToString());
            }

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
    }
}

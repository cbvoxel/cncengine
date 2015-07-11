using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CncEngine.Common.Ctrl.ForEach
{
    public class MessageCollection
    {
        private readonly Message _originalMessage;
        private IEnumerable<Message> _messages;

        public MessageCollection(Message originalMessage, string xPath)
        {
            _originalMessage = originalMessage;
            _messages = _originalMessage.Payload
                .XPathSelectElements(xPath)
                .Select(n => new Message
                {
                    Variables = originalMessage.Variables,
                    Payload = new Payload(n)
                })
                .ToList();
        }

        public MessageCollection ForEach(Func<Message, Message> foreachFunc)
        {
            _messages = _messages.Select(foreachFunc).ToList();
            return this;
        }

        public Message Combine()
        {
            var payloads = _messages.Select(m => m.Payload);
            _originalMessage.SetPayload(new XElement("PayloadCollection", payloads));
            return _originalMessage;
        }
    }

    public class CombinerHelper
    {
        public static Message Combine(IEnumerable<Message> messages)
        {
            var combineFunc = new Func<Message, Message, Message>((m1, m2) => m1.Combine(m2));
            return Combine(messages, combineFunc);
        }

        public static Message Combine(IEnumerable<Message> messages, Func<Message, Message, Message> combinerFunc)
        {
            if (!messages.Any())
                return new Message();

            var oMessage = messages.First();
            return messages.Skip(1).Aggregate(oMessage, combinerFunc);
        }
    }
}

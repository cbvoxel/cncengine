using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace CncEngine.Common
{
    public static class MessageExtensions
    {
        private static ILog Logger = LogManager.GetLogger(typeof (MessageExtensions));

        public static Message LogInfo(this Message msg)
        {
            return LogInfo(msg, m => m);
        }

        public static Message LogDebug(this Message msg)
        {
            return LogDebug(msg, m => m);
        }

        public static Message LogFatal(this Message msg)
        {
            return LogFatal(msg, m => m);
        }

        public static Message LogWarn(this Message msg)
        {
            return LogWarn(msg, m => m);
        }

        public static Message LogError(this Message msg)
        {
            return LogError(msg, m => m);
        }

        public static Message LogInfo(this Message msg, Func<Message, Object> selector)
        {
            Logger.Info(selector(msg).ToString());
            return msg;
        }

        public static Message LogDebug(this Message msg, Func<Message, Object> selector)
        {
            Logger.Debug(selector(msg).ToString());
            return msg;
        }

        public static Message LogFatal(this Message msg, Func<Message, Object> selector)
        {
            Logger.Fatal(selector(msg).ToString());
            return msg;
        }

        public static Message LogWarn(this Message msg, Func<Message, Object> selector)
        {
            Logger.Warn(selector(msg).ToString());
            return msg;
        }

        public static Message LogError(this Message msg, Func<Message, Object> selector)
        {
            Logger.Error(selector(msg).ToString());
            return msg;
        }

        public static Message ExtractVariable(this Message msg, string xPath, string variableName)
        {
            var doc = msg.Payload.ToXDocument();
            if(doc == null)
                throw new InvalidOperationException("Not an xml document.");
            if (doc.Root == null)
                throw new InvalidOperationException("No root in xml document.");
            var value = doc.Root.Descendants(xPath).Select(n => n.Value).FirstOrDefault();
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
    }
}

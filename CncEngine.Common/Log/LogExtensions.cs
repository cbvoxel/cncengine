using System;
using log4net;

namespace CncEngine.Common.Log
{
    public static class LogExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogExtensions));

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

        public static Message LogInfo(this Message msg, string text)
        {
            return LogError(msg, m => string.Format("{0}\r\n{1}", text, m));
        }

        public static Message LogDebug(this Message msg, string text)
        {
            return LogDebug(msg, m => string.Format("{0}\r\n{1}", text, m));
        }

        public static Message LogFatal(this Message msg, string text)
        {
            return LogFatal(msg, m => string.Format("{0}\r\n{1}", text, m));
        }

        public static Message LogWarn(this Message msg, string text)
        {
            return LogWarn(msg, m => string.Format("{0}\r\n{1}", text, m));
        }

        public static Message LogError(this Message msg, string text)
        {
            return LogError(msg, m => string.Format("{0}\r\n{1}", text, m));
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
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CncEngine.Common
{
    public class Message
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(Message));

        public IDictionary<String, Object> Variables { get; internal set; }

        public Payload Payload { get; internal set; }

        public Message()
        {
            Variables = new Dictionary<String, Object>();
            Payload = new Payload();
        }

        public Message SetPayload(string newPayload)
        {
            Payload.PayloadAsString = newPayload;
            return this;
        }

        public Message SetPayload(Stream newPayload)
        {
            using (var stream = new StreamReader(newPayload, Encoding.UTF8))
            {
                Payload.PayloadAsString = stream.ReadToEnd();
            }
            return this;
        }

        public Message SetVariable(string variableName, object variableValue)
        {
            Variables[variableName] = variableValue;
            return this;
        }

        public override string ToString()
        {
            return String.Format("Message Paylod:\r\n{0}\r\nVariables:\r\n{1}", 
                Payload, 
                String.Join("\r\n", Variables.Select(v => v.Key + "=" + (v.Value ?? "null").ToString()))
                );
        }
    }
}

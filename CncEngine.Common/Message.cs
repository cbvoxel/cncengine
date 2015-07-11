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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using log4net;

namespace CncEngine.Common
{
    public class Message
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(Message));

        public static Message NoChange(Message message)
        {
            return message;
        }

        public IDictionary<String, Object> Variables { get; internal set; }

        public Payload Payload { get; internal set; }

        public Message()
        {
            Variables = new Dictionary<String, Object>();
            Payload = new Payload();
        }

        public Message SetPayload(string newPayload)
        {
            Logger.Debug("Step");
            Payload.PayloadAsString = newPayload;
            return this;
        }

        public Message SetPayload(Stream newPayload)
        {
            Logger.Debug("Step");
            using (var stream = new StreamReader(newPayload, Encoding.UTF8))
            {
                Payload.PayloadAsString = stream.ReadToEnd();
            }
            return this;
        }

        public Message SetPayload(Func<Message, String> setterFunc)
        {
            Logger.Debug("Step");
            Payload.PayloadAsString = setterFunc(this);
            return this;
        }

        public Message ClearVariables()
        {
            Variables.Clear();
            return this;
        }

        public Message SetVariable(string variableName, object variableValue)
        {
            Logger.Debug("Step");
            Variables[variableName] = variableValue;
            return this;
        }

        public override string ToString()
        {
            return String.Format("Variables:\r\n{1}\r\n\r\nMessage Paylod:\r\n{0}", 
                Payload, 
                String.Join("\r\n", Variables.Select(v => v.Key + "=" + (v.Value ?? "null").ToString()))
                );
        }
    }
}

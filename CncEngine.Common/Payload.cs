using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net;

namespace CncEngine.Common
{
    public class Payload
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(Message));

        internal string PayloadAsString;

        public Payload()
        {
            PayloadAsString = "";
        }

        public override string ToString()
        {
            return PayloadAsString;
        }

        public Stream ToStream()
        {
            byte[] bytes = ToBytes();
            return new MemoryStream(bytes);
        }

        public XDocument ToXDocument()
        {
            return XDocument.Load(ToStream());
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(PayloadAsString);
        }
    }
}

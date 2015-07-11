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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CncEngine.Common.Xml;
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
            return ToXDocument().ToString();
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

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
using System.Threading.Tasks;

namespace CncEngine.Common.Http
{
    public class HttpConnectorConfiguration
    {
        public string Host { get; set; }
        public string BasePath { get; set; }
        public int Port { get; set; }
        public IDictionary<string,string> Query { get; set; }
        public IDictionary<string, string> Headers { get; set; }

        public HttpConnectorConfiguration()
        {
            Query = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
        }
    }
}

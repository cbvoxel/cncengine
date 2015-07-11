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

using System.Collections.Generic;
using log4net;

namespace CncEngine.Common.Http
{
    public class HttpRouteEqualityComparer : IEqualityComparer<HttpRoute>
    {
        public bool Equals(HttpRoute x, HttpRoute y)
        {
            var result =  (x.Host == "+" || y.Host == "+" || x.Host == y.Host) && x.Port == y.Port && x.Path == y.Path;
            return result;
        }

        public int GetHashCode(HttpRoute obj)
        {
            var result = obj.Host.GetHashCode() ^ obj.Port.GetHashCode() ^ obj.Path.GetHashCode();
            return result;
        }
    }
}

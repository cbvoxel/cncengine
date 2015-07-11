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

using log4net;

namespace CncEngine.Common.Ctrl
{
    public class ElseExecutor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageExtensions));

        private readonly IfExecutor _ifExecutor;

        public ElseExecutor(IfExecutor ifExecutor)
        {
            _ifExecutor = ifExecutor;
        }

        public Message EndIf()
        {
            Logger.Debug("Step");
            return _ifExecutor.Execute();
        }
    }
}
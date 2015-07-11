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
using System.IO;
using System.Linq;
using log4net;

namespace CncEngine.Server
{
    class Program
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(Engine));

        static void Main(string[] args)
        {
            var licenceLines = File.ReadAllLines("Licence.txt");

            Console.WriteLine(@"CncEngine.Server  Copyright (C) 2015  Carsten Blank
This program comes with ABSOLUTELY NO WARRANTY; for details type `-show=w'.
This is free software, and you are welcome to redistribute it
under certain conditions; type `-show=c' for details.
");

            if (args.Length > 0 && args.Contains("-show=c"))
            {
                Console.WriteLine("Conditions excerpt from GPL Licence:");
                Console.WriteLine(String.Join("\r\n", licenceLines.Skip(69).Take(551)));
            }
            if (args.Length > 0 && args.Contains("-show=w"))
            {
                Console.WriteLine("Warranty excerpt from GPL Licence:");
                Console.WriteLine(String.Join("\r\n", licenceLines.Skip(587).Take(24)));
            }

            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("Engine Startup");
            var engine = new Engine();
            engine.Run();
        }
    }
}

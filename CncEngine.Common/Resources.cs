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
using System.Text;
using System.Xml.Linq;

namespace CncEngine.Common
{
    public class Resources
    {
        public static Boolean ResourceFileExits<T>(string relativePath) where T : IModule
        {
            var moduleLocation = typeof(T).Assembly.Location;
            var moduleFileInfo = new FileInfo(moduleLocation);
            var moduleDirectory = moduleFileInfo.Directory;
            return moduleDirectory != null && System.IO.File.Exists(moduleDirectory.FullName + "/" + relativePath);
        }

        public static FileInfo ResourceFileInfo<T>(string relativePath) where T : IModule
        {
            var moduleLocation = typeof(T).Assembly.Location;
            var moduleFileInfo = new FileInfo(moduleLocation);
            var moduleDirectory = moduleFileInfo.Directory;
            return moduleDirectory != null 
                ? new FileInfo(moduleDirectory.FullName + "/" + relativePath) 
                : null;
        }

        public static string LoadModuleResourceTextFile<T>(string relativePath) where T : IModule
        {
            var moduleLocation = typeof(T).Assembly.Location;
            var moduleFileInfo = new FileInfo(moduleLocation);
            var moduleDirectory = moduleFileInfo.Directory;
            return System.IO.File.ReadAllText(moduleDirectory.FullName + "/" + relativePath);
        }

        public static void SaveModuleResourceTextFile<T>(string relativePath, string text) where T : IModule
        {
            var moduleLocation = typeof(T).Assembly.Location;
            var moduleFileInfo = new FileInfo(moduleLocation);
            var moduleDirectory = moduleFileInfo.Directory;
            System.IO.File.WriteAllText(moduleDirectory.FullName + "/" + relativePath, text, Encoding.UTF8);
        }

        public static XElement LoadModuleResourceXml<T>(string relativePath) where T : IModule
        {
            var moduleLocation = typeof(T).Assembly.Location;
            var moduleFileInfo = new FileInfo(moduleLocation);
            var moduleDirectory = moduleFileInfo.Directory;
            using (var stream = System.IO.File.OpenRead(moduleDirectory.FullName + "/" + relativePath))
            {
                var doc = XElement.Load(stream);
                return doc;
            }
        }

        public static string LoadTextFile(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public static XDocument LoadXmlFile(string path)
        {
            using (var stream = System.IO.File.OpenRead(path))
            {
                var doc = XDocument.Load(stream);
                return doc;
            }
        }
    }
}

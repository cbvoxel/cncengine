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

        public static XDocument LoadModuleResourceXml<T>(string relativePath) where T : IModule
        {
            var moduleLocation = typeof(T).Assembly.Location;
            var moduleFileInfo = new FileInfo(moduleLocation);
            var moduleDirectory = moduleFileInfo.Directory;
            using (var stream = System.IO.File.OpenRead(moduleDirectory.FullName + "/" + relativePath))
            {
                var doc = XDocument.Load(stream);
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

using System;

namespace CncEngine.Common.Exceptions
{
    public class ModuleConfigurationException : Exception
    {
        public ModuleConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}

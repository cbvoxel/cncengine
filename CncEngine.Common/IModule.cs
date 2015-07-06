using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CncEngine.Common
{
    public interface IModule
    {
        void Configure(ModuleConfiguration contextModuleConfiguration);

        void Process(Message message);
    }
}

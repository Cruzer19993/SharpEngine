using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine
{
    public class Component
    {
        public String GetComponentName()
        {
            return GetType().Name;
        }
    }
}

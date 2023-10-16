using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.ECS
{
    public class ECSManager
    {
        public static void InitializeSystems(params ECSSystem[] systemsToInitialize) 
        {
            foreach(ECSSystem systemType in systemsToInitialize)
            {
                systemType.Initialize();
            }
        }
    }
}

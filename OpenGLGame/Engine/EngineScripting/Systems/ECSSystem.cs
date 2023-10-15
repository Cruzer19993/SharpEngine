using SharpEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.ECS
{
    public class ECSSystem
    {
        protected void Initialize() {
            RenderSystem.instance.OnUpdateEvent += (delegate { OnUpdate(); });
        }

        public virtual void OnUpdate() { }
        public virtual void OnStart() { }

    }
}

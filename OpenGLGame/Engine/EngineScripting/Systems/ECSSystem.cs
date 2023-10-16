using SharpEngine.Rendering;
using SharpEngine.SceneManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.ECS
{
    public class ECSSystem
    {
        public List<Entity> attachedEntities;
        public Type[] neededComponents;
        public void Initialize() {
            RenderSystem.instance.OnUpdateEvent += (delegate { OnUpdate(); });
            SceneManager.Instance.GetActiveScene().EntitiesChangedEvent += (delegate { UpdateAttachedEntities(); });
            UpdateAttachedEntities();
            OnAttach();
        }

        public virtual void OnUpdate() { }
        public virtual void OnAttach() { }

        void UpdateAttachedEntities()
        {
            attachedEntities = SceneManager.Instance.GetActiveScene().GetEntitiesWithComponents(neededComponents).ToList();
            Console.WriteLine(attachedEntities.Count);
        }

    }
}

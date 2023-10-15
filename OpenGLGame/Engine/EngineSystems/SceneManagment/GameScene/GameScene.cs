using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using SharpEngine.ECS;

namespace SharpEngine.SceneManagment;

public class GameScene
{
    public List<Entity> SceneEntities = new();
    public event EventHandler EntitiesChangedEvent;
    public void SpawnEntity(Entity entity)
    {
        lock (SceneEntities)
        {
            entity.entityID = SceneEntities.Count;
            SceneEntities.Add(entity);
        }
        EntitiesChangedEvent?.Invoke(this,EventArgs.Empty);
    }
    public void DestroyEntity(int EntityID) {
        lock (SceneEntities)
        {
            SceneEntities.Remove(SceneEntities.Find(x => x.entityID == EntityID));
        }
        EntitiesChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public Entity[] GetEntitiesWithComponents(params Type[] componentTypes)
    {
        lock (SceneEntities)
        {
            return SceneEntities.Where(entity => componentTypes.All(type => entity.HasComponent(type))).ToArray();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine;
using OpenTK.Graphics.OpenGL4;
using OpenGLGame;
using OpenTK.Mathematics;

namespace SharpEngine
{
    public class Entity
    {
        public int EntityID;
        public string EntityName;
        public List<Component> components = new();
        bool isDirty;
        public Entity(string _entityName, bool isStatic = true)
        {
            EntityName = _entityName;
            components.Add(new Transform());
            components.Add(new Material());
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            components.Remove(component);
        }

        public T GetComponent<T>() where T : Component
        {
            return components.Find(c => c is T) as T;
        }

        public bool HasComponent(Type componentType)
        {
            return components.Exists(comp => comp.GetType() == componentType);
        }
    }
}

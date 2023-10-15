using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine;
using OpenTK.Graphics.OpenGL4;
using OpenGLGame;
using OpenTK.Mathematics;

namespace SharpEngine.ECS;

public class Entity
{
    public int entityID;
    public string entityTag;
    public string entityName;
    public List<Component> components = new();
    public Entity(string _entityName, bool CreateWithNoComponents = false)
    {
        entityName = _entityName;
        if (!CreateWithNoComponents)
        {
            components.Add(new Transform());
            components.Add(new Material());
        }
    }

    public void AddComponent(Component component)
    {
        component.ParentEntity = this;
        components.Add(component);
        Console.WriteLine(component.ParentEntity);
    }

    public void RemoveComponent(Component component)
    {
        component.ParentEntity = null;
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

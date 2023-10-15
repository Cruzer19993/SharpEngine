using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.ECS;

public class Component
{
    private Entity parentEntity;
    public Entity ParentEntity
    {
        get { return parentEntity; }
        set
        {
            parentEntity = value;
        }
    }
    public String GetComponentName()
    {
        return GetType().Name;
    }
}

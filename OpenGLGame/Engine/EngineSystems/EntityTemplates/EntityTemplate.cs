using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SharpEngine.ECS;

namespace SharpEngine.EntityTemplates
{
    public static class EntityTemplate
    {
        public static Entity CameraEntity(bool isMain,float cameraSize,Vector2 cameraAspectRatio)
        {
            Entity tempCamera = new Entity("Camera",true);
            tempCamera.AddComponent(new Transform());
            tempCamera.AddComponent(new Camera(cameraSize,cameraAspectRatio));
            return tempCamera;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpEngine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SharpEngine.ECS
{

    public class Camera : Component
    {
        public Vector2 cameraAspectRatio;
        public float frustumWidth;
        public float frustumHeight;
        public float cameraSize;
        public float cameraZoom;
        
        private Matrix4 m_cameraProjectionMatrix;
        public Matrix4 GetCameraViewMatrix()
        {
            Matrix4 viewMatrix = Matrix4.CreateTranslation(ParentEntity.GetComponent<Transform>().Position )*Matrix4.CreateScale(cameraZoom);
            return viewMatrix;
        }
        public Matrix4 GetCameraProjectionMatrix()
        {
            return m_cameraProjectionMatrix;
        }
        public Camera(float size,Vector2 AspectRatio)
        {
            if (ParentEntity != null && ParentEntity.GetComponent<Transform>() == null)
            {
                Console.WriteLine("[ERR] Cannot have camera component without transform. Adding one.");
                ParentEntity.AddComponent(new Transform());
            }
            else
            {
                m_cameraProjectionMatrix = Matrix4.CreateOrthographic(16f * size, 9f * size, 0.1f, 1000f);
                frustumWidth = 16 * size;
                frustumHeight = 9 * size;
                cameraSize = size;
                cameraZoom = 1f;
                cameraAspectRatio = AspectRatio;
            }
        }
        public void SetCameraZoom(float value)
        {
            cameraZoom = value;
        }
    }
}

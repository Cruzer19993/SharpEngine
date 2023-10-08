using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpEngine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace SharpEngine
{

    internal class Camera
    {
        public Transform m_transform;
        private Matrix4 m_cameraProjectionMatrix;
        private Matrix4 m_cameraViewMatrix;

        public Matrix4 GetCameraViewMatrix()
        {
            if (m_transform == null) return Matrix4.Zero;
            else
            {
                Matrix4 viewMatrix = Matrix4.CreateTranslation(m_transform.Position);
                return viewMatrix;
            }
        }
        public Matrix4 GetCameraProjectionMatrix()
        {
            return m_cameraProjectionMatrix;
        }
        public Camera(float size)
        {
            m_transform = new Transform(0f,0f,0f);
            m_cameraProjectionMatrix = Matrix4.CreateOrthographic(16f*size, 9f*size, 0.1f, 1000f);
        }
    }
}

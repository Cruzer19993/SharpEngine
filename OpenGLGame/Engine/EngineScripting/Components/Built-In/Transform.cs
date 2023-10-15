using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SharpEngine.ECS
{
    public class Transform : Component
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Quaternion Rotation;
        public Transform()
        {
            Position = new();
            Rotation = new();
        }
        
        public Transform(float x, float y, float z)
        {
            Vector3 temp = new Vector3(x,y,z);
            Position = temp;
            Rotation = new();
        }
        public Transform(Vector3 position, Vector3 scale , Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}

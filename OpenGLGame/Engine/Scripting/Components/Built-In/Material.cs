using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine
{
    public class Material : Component
    {
        public Shader materialShader;
        public Texture materialTexture;
        public Vector2 textureCoordsOffset;
        public Material(Shader materialShader = null, Texture materialTexture = null)
        {
            this.materialShader = materialShader;
            this.materialTexture = materialTexture;
        }
    }
}

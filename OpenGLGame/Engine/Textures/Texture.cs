using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StbImageSharp;
using OpenTK.Graphics.OpenGL4;

namespace SharpEngine
{
    public class Texture
    {
        int Handle;
        string pathToImage;
        public Texture(string _pathToImage)
        {
            Handle = GL.GenTexture();
            pathToImage = _pathToImage;
            GL.TextureParameter(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        }

        public void Use()
        {
            StbImage.stbi_set_flip_vertically_on_load(1); //stbi loads textures from top-left where OpenGL loads them from bottom-left, it causes the image to look flipped, so we make stbi flip the loaded images.
            ImageResult texture = ImageResult.FromStream(File.OpenRead(pathToImage), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}

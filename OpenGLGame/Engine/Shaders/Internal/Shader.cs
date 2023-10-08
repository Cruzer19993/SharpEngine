using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace SharpEngine
{
    public class Shader : IDisposable
    {
        private bool disposedValue = false;
        public int m_shaderHandle;
        public Shader(string vertexPath, string fragmentPath)
        {
            //handles.
            int VertexShader;
            int FragmentShader;
            //Get Source Code to compile shader
            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);
            //Generate Shaders
            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader (ShaderType.FragmentShader);
            GL.ShaderSource (FragmentShader, FragmentShaderSource);
            //Compile shader and check for errors.
            int success = -1;
            GL.CompileShader(VertexShader);
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }
            GL.CompileShader(FragmentShader);
            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
            if(success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            //Create GLSL Program.
            m_shaderHandle = GL.CreateProgram();

            GL.AttachShader(m_shaderHandle, VertexShader);
            GL.AttachShader(m_shaderHandle, FragmentShader);

            GL.LinkProgram(m_shaderHandle);

            GL.GetProgram(m_shaderHandle, GetProgramParameterName.LinkStatus, out success);
            if(success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(m_shaderHandle);
                Console.WriteLine(infoLog);
            }

            //Cleanup
            GL.DetachShader(m_shaderHandle, VertexShader);
            GL.DetachShader(m_shaderHandle, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(m_shaderHandle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                GL.DeleteProgram(m_shaderHandle);
                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (!disposedValue)
            {
                Console.WriteLine("GPU Resource Leak Detected. Did you forget to call Dispose() ?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

using SharpEngine;
using OpenTK.Mathematics;
using OpenGLGame;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Input;
using OpenTK.Windowing.Desktop;

namespace Game
{
    internal class Program
    {
        GameScene testScene;
        static void Main(string[] args)
        {
            Initializer.Start(1280,720,"OpenGL Engine");
        }    
    }
}
using OpenGLGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine
{
    public class Initializer
    {
        public GameScene loadedScene;
        public static void Start(int windowWidth, int windowHeight, string windowName)
        {
            using (RenderSystem renderer = new RenderSystem(windowWidth, windowHeight, windowName))
            {
                renderer.Run();
            }
        }
    }
}

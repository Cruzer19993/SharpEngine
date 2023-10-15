using OpenGLGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using System.ComponentModel.DataAnnotations;
using OpenTK.Graphics;
using OpenTK;
using System.Windows;
using SharpEngine.SceneManagment;
using SharpEngine.Rendering;
using SharpEngine.ECS;

namespace SharpEngine
{
    public class Initializer
    {
        static SceneManager m_sceneManager;
        public static void Start(int windowWidth, int windowHeight, string windowName, AspectRatio aspect)
        {
            InitializePipeline();
            using (RenderSystem renderer = new RenderSystem(windowWidth,windowHeight, windowName,aspect.AspectRatioX,aspect.AspectRatioY))
            {
                Game.Instance.Setup();
                Game.Instance.OnLoad();
                renderer.Run();
            }
        }

        static void InitializePipeline()
        {
            m_sceneManager = new SceneManager();
        }
    }
    public class AspectRatio
    {
        public int AspectRatioX;
        public int AspectRatioY;
        public AspectRatio(int aspectRatioX, int aspectRatioY)
        {
            AspectRatioX = aspectRatioX;
            AspectRatioY = aspectRatioY;
        }
    }

    public class Game
    {
        public static Game Instance;

        public List<ECSSystem> systems = new();
       
        public Game()
        {
            Console.WriteLine("Game Logic Constructor");
            Instance = this;
        }
        public void Setup()
        {
            Console.WriteLine("GameLogic Setup");
            RenderSystem.instance.OnUpdateEvent += (delegate { OnUpdate(); });
            SceneManager.Instance.OnStartEvent += (delegate { OnStart(); });
        }

        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnLoad() { }

    }
}

using SharpEngine;
using OpenTK.Mathematics;
using OpenGLGame;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Input;
using OpenTK.Windowing.Desktop;
using SharpEngine.Rendering;
using SharpEngine.SceneManagment;
using SharpEngine.EntityTemplates;
using SharpEngine.ECS;

namespace Sandbox
{
    public class Sandbox : Game
    {
        GameScene sandboxScene;
        static void Main(string[] args)
        {
            Instance = new Sandbox();
            Initializer.Start(1280, 720, "OpenGL Engine", new AspectRatio(16, 9));
        }

        public override void OnLoad()
        {
            sandboxScene = new GameScene();
            sandboxScene.SpawnEntity(EntityTemplate.CameraEntity(true,1f,new Vector2(16,9)));
            SceneManager.Instance.SetCurrentScene(sandboxScene);
        }
        public override void OnStart()
        {

        }

        public override void OnUpdate()
        {

        }
    }

    public class CameraMovementSystem : ECSSystem
    {

    }

}
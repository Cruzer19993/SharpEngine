using SharpEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.SceneManagment
{
    public class SceneManager
    {
        public static SceneManager Instance { get; private set; }
        private GameScene activeScene;
        public GameScene[] loadedScenes;
        public EventHandler SceneChangedEvent;
        public EventHandler OnStartEvent;
        public SceneManager()
        {
            Instance = this;
        }

        public async void SetCurrentScene(GameScene scene)
        {
            await Task.Run(() =>
            {
                while (!RenderSystem.isDoneLoading)
                {

                }
            });
            activeScene = scene;
            SceneChangedEvent?.Invoke(this,EventArgs.Empty);
            OnStartEvent?.Invoke(this,EventArgs.Empty);
        }
        public async void SetCurrentScene(int index)
        {
            await Task.Run(() =>
            {
                while (!RenderSystem.isDoneLoading)
                {

                }
            });
            activeScene = loadedScenes[index];
            SceneChangedEvent?.Invoke(this, EventArgs.Empty);
            OnStartEvent?.Invoke(this, EventArgs.Empty);
        }

        public GameScene GetActiveScene()
        {
            return activeScene;
        }
    }
}

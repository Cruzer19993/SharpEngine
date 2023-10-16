using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.ComponentModel;
using StbImageSharp;
using OpenTK.Mathematics;
using OpenGLGame;
using System.Linq;
using System.Runtime.InteropServices;
using SharpEngine.SceneManagment;
using SharpEngine.ECS;

namespace SharpEngine.Rendering
{
    internal class RenderSystem : GameWindow
    {
        public static RenderSystem instance;
        public static bool isDoneLoading = false;
        public static bool isRenderingFrame = false;
        public EventHandler OnUpdateEvent;

        Shader shader; //current used shader
        Texture texture; //current used texture;
        Texture defaultTexture; //used when no texutre assigned on Material Component.

        public Entity mainCameraEntity;
        GameScene currentScene;
        List<Entity> objectsToRender = new();
        Vector2 startResolution;
        Vector3 cameraTranslationFactor;
        //BUFFERS
        Matrix4 viewMatrix;
        Matrix4 projMatrix;
        Matrix4[] modelMatrixes;
        DrawElementsIndirectCommand[] drawCommands = new DrawElementsIndirectCommand[0];
        int IndirectBuffer;
        int m_elementBufferObject;
        int ElementBufferObject
        {
            get { return m_elementBufferObject; }
            set { m_elementBufferObject = value; }
        }
        int matrixSSBO;
        int VertexBufferObject;
        int VertexArrayObject;
        int previousFrameObjectCount = -1;
        //QUAD DATA
        float[] vertices = { //quad vertices.
             0.5f,  0.5f, -0.5f, 1.0f, 1.0f,  // top right
             0.5f, -0.5f, -0.5f, 1.0f, 0.0f,// bottom right
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, -0.5f, 0.0f, 1.0f // top left
        };

        uint[] indices =
        {
            0, 1, 3, 1, 2, 3,
        };
        public RenderSystem(int width, int height, string title,int AspectRatioX, int AspectRatioY) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title,AspectRatio = (AspectRatioX,AspectRatioY)}) {
            instance = this;
            startResolution = new Vector2(width, height);
        }

        protected override void OnLoad()
        {
            base.OnLoad();       

            shader = new Shader("D:\\Projekty\\VisualStudio\\OpenGLGame\\OpenGLGame\\Engine\\Shaders\\GLSL\\VertexShaders\\DefaultVertex.vert", "D:\\Projekty\\VisualStudio\\OpenGLGame\\OpenGLGame\\Engine\\Shaders\\GLSL\\FragmentShaders\\DefaultFragment.frag");
            defaultTexture = new Texture("D:\\Projekty\\VisualStudio\\OpenGLGame\\OpenGLGame\\Engine\\DebugAssets\\crateTexture.jpg");

            matrixSSBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, matrixSSBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 4096*Marshal.SizeOf<Matrix4>(),IntPtr.Zero,BufferUsageHint.DynamicDraw);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1,2,VertexAttribPointerType.Float,false, 5 * sizeof(float), 3 * sizeof(float));

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.DynamicDraw);


            //I have not a slightest idea what is this unholy shit.
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            IndirectBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.DrawIndirectBuffer, IndirectBuffer);
            GL.BufferData(BufferTarget.DrawIndirectBuffer, 4096 * Marshal.SizeOf<DrawElementsIndirectCommand>(), IntPtr.Zero, BufferUsageHint.StaticDraw);
            SceneManager.Instance.SceneChangedEvent += (delegate { OnSceneChange(); });
            isDoneLoading = true;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            if (AspectRatio.HasValue && currentScene != null)
            {
                //TODO: Make the translation factor respond to diffirent resolutions
                cameraTranslationFactor = new Vector3((AspectRatio.Value.numerator / startResolution.X) * (1 / mainCameraEntity.GetComponent<Camera>().cameraZoom), (AspectRatio.Value.denominator / startResolution.Y) * (1 / mainCameraEntity.GetComponent<Camera>().cameraZoom), 1f);
                //Console.WriteLine(cameraTranslationFactor.ToString());
            }
            Render();
            GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) =>
            {
                string msg = Marshal.PtrToStringAnsi(message);
                Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", source, type, id, severity, length, msg, userParam);
            }, (IntPtr)0);
        }
        public void Render()
        {
            isRenderingFrame = true;
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, matrixSSBO);
            texture = defaultTexture;
            texture.Use();
            shader.Use();
            GL.MultiDrawElementsIndirect(PrimitiveType.Triangles, DrawElementsType.UnsignedInt, IntPtr.Zero, drawCommands.Length, Marshal.SizeOf<DrawElementsIndirectCommand>());
            SwapBuffers();
            GL.BindVertexArray(0);
            isRenderingFrame = false;
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if (currentScene == null) return;
            UpdateModelMatrixes();
            if (previousFrameObjectCount == -1)
            {
                CreateIndirectBufferCommands();
                previousFrameObjectCount = objectsToRender.Count;
            }else if(previousFrameObjectCount != objectsToRender.Count)
            {
                CreateIndirectBufferCommands();
                previousFrameObjectCount = objectsToRender.Count;
            }
            if (KeyboardState.IsKeyPressed(Keys.C))
            {
                Entity temp = new Entity("Test Entity");
                Random rand = new Random();
                temp.GetComponent<Transform>().Position = new Vector3((float)rand.Next(-6,6)*10,0f,1f);
            }
            OnUpdateEvent.Invoke(this, EventArgs.Empty);
        }

        void CreateIndirectBufferCommands()
        {
            drawCommands = new DrawElementsIndirectCommand[1];

            drawCommands[0] = new DrawElementsIndirectCommand
            {
                Count = 6,
                InstanceCount = (uint)objectsToRender.Count,
                FirstIndex = 0,
                BaseInstance = 0
            };

            GL.BindBuffer(BufferTarget.DrawIndirectBuffer, IndirectBuffer);
            GL.BufferData(BufferTarget.DrawIndirectBuffer, drawCommands.Length * sizeof(uint) * 5, drawCommands, BufferUsageHint.StaticDraw);
        }

        void UpdateModelMatrixes()
        {
            modelMatrixes = new Matrix4[objectsToRender.Count*3];
            uint offset = 0;
            Matrix4 currentProjMatirx = mainCameraEntity.GetComponent<Camera>().GetCameraProjectionMatrix();
            Matrix4 currentViewMatrix = mainCameraEntity.GetComponent<Camera>().GetCameraViewMatrix();
            for(int i=0;i<objectsToRender.Count; i++)
            {
                modelMatrixes[0 + offset] = Matrix4.CreateTranslation(objectsToRender[i].GetComponent<Transform>().Position * cameraTranslationFactor);
                modelMatrixes[1 + offset] = currentViewMatrix;
                modelMatrixes[2 + offset] = currentProjMatirx;
                offset += 3;
            }
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, matrixSSBO);
            GL.BufferSubData(BufferTarget.ShaderStorageBuffer,IntPtr.Zero,Marshal.SizeOf<Matrix4>() * modelMatrixes.Length,modelMatrixes);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
            int storageIndex = GL.GetProgramResourceIndex(shader.m_shaderHandle, ProgramInterface.ShaderStorageBlock, "MatrixBlock");
            GL.ShaderStorageBlockBinding(shader.m_shaderHandle, storageIndex, 3);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        public void OnSceneChange()
        {
            Console.WriteLine("Scene Changed");
            currentScene = SceneManager.Instance.GetActiveScene();
            currentScene.EntitiesChangedEvent += (delegate { UpdateRenderObjects(); });
            mainCameraEntity = currentScene.GetEntitiesWithComponents(typeof(Transform), typeof(Camera)).ToArray()[0];
            UpdateRenderObjects();
        }
        public async void UpdateRenderObjects()
        {
            await Task.Run(() =>
            {
                while (isRenderingFrame)
                {

                }
            });
            lock (objectsToRender)
            {
                objectsToRender.Clear();
                objectsToRender.AddRange(currentScene.GetEntitiesWithComponents(typeof(Transform), typeof(Material)).ToList());

            }
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            shader.Dispose();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //release VRAM on closing.
            base.OnClosing(e);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
        }

    }

    public struct DrawElementsIndirectCommand
    {
        public uint Count;
        public uint InstanceCount;
        public uint FirstIndex;
        public uint BaseInstance;
    }

}


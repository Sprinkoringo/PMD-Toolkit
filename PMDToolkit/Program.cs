// Released to the public domain. Use, modify and relicense at will.

using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using DragonOgg.MediaPlayer;
using PMDToolkit.Graphics;

namespace PMDToolkit {
    public class Game : GameWindow
    {
        public enum GameLoadState
        {
            Unloaded,
            Loading,
            PostLoading,
            Finalizing,
            Loaded,
            Errored,
            Closing,
            Closed
        }

        public static GameLoadState GameLoaded;

        static string loadMessage;
        static object lockObj = new object();

        public static void UpdateLoadMsg(string loadMsg)
        {
            lock (lockObj)
                loadMessage = loadMsg;
        }

        int errorCount;

        static void openGame()
        {
            try
            {
                // The 'using' idiom guarantees proper resource cleanup.
                using (Game game = new Game())
                {
                    game.Icon = PMDToolkit.Properties.Resources.Icon;
                    game.Run(Graphics.TextureManager.FPS_CAP, Graphics.TextureManager.FPS_CAP);
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Logs.Logger.LogError(ex);
                throw ex;
            }
        }

        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Game()
            : base(Graphics.TextureManager.SCREEN_WIDTH, Graphics.TextureManager.SCREEN_HEIGHT, GraphicsMode.Default, "PMD Toolkit") {
            VSync = VSyncMode.On;
            string version = GL.GetString(StringName.Version);
            string vendor = GL.GetString(StringName.Vendor);
            string renderer = GL.GetString(StringName.Renderer);
            string shaderLang = GL.GetString(StringName.ShadingLanguageVersion);
            string extensions = GL.GetString(StringName.Extensions);

            Logs.Logger.LogInfo("Open GL Info:\nVersion= " + version + "\nVendor= " + vendor + "\nRenderer= " + renderer + "\nShaderLang= " + shaderLang + "\nExtensions= " + extensions);

            string[] extensionsList = extensions.Split(' ');


            int major = (int)version[0];
            int minor = (int)version[2];
            if (major < 2 || major == 2 && minor < 1)
                throw new System.Exception("OpenGL 2.1 not supported!  Current version: " + version);
            if (!extensions.Contains("GL_ARB_texture_non_power_of_two")) {
                throw new System.Exception("Non-Power of 2 Textures not supported!");
            }
            WindowBorder = OpenTK.WindowBorder.Fixed;
            loadMessage = "";
            errorCount = 0;
        }

        /// <summary>Load resources here.</summary>
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            Graphics.TextureManager.InitBase();

            GameLoaded = GameLoadState.Loading;

            Thread thread = new Thread(() => {
                AsyncLoad();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        void AsyncLoad()
        {
            try
            {
                UpdateLoadMsg("Loading Paths");
                Data.Paths.Init();

                UpdateLoadMsg("Loading Textures");
                Graphics.TextureManager.Init();

                UpdateLoadMsg("Loading Game Data");
                Data.GameData.Init();
                UpdateLoadMsg("Loading Audio");
                AudioManager.Init();
                Logs.Logger.LogInfo(AudioManager.GetALInfo());

                UpdateLoadMsg("Starting...");
            }
            catch (Exception ex)
            {
                Logs.Logger.LogError(ex);
            }

            GameLoaded = GameLoadState.PostLoading;
        }

        /// <summary>Unload resources here.</summary>
        protected override void OnUnload(EventArgs e)
        {
            while (GameLoaded < GameLoadState.Loaded)
                Thread.Sleep(100);

            AudioManager.Exit();
            Graphics.TextureManager.Exit();

            base.OnUnload(e);
        }

        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            Graphics.TextureManager.SetViewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);

            if (Editors.MainPanel.GameNeedWait)
            {
                Editors.MainPanel.GameWaiting = true;

                while (Editors.MainPanel.GameNeedWait)
                    Thread.Sleep(100);

                Editors.MainPanel.GameWaiting = false;
            }

            if (GameLoaded == GameLoadState.Closing)
                Close();
            else if (GameLoaded == GameLoadState.PostLoading)
                GameLoaded = GameLoadState.Finalizing;
            else if (GameLoaded == GameLoadState.Finalizing)
            {
                Graphics.TextureManager.PostInit();

                Logic.Gameplay.MenuManager.Init();
                Logic.Gameplay.Processor.Init();
                Logic.Display.Screen.Init();
                Logic.Gameplay.Processor.Restart();
                Logic.Display.Screen.ProcessTaskQueue(true);

                GameLoaded = GameLoadState.Loaded;
                while (!Editors.MainPanel.EditorLoaded)
                {
                    Thread.Sleep(100);
                }
            }
            else if (GameLoaded == GameLoadState.Loaded)
            {
                try
                {
                    Graphics.TextureManager.Update();

                    RenderTime elapsedTime = new RenderTime((int)(e.Time * Graphics.TextureManager.FPS_CAP * 1000));

                    //set this frame's input
                    Logic.Gameplay.Input input = new Logic.Gameplay.Input(Keyboard, Mouse);
                    Logic.Gameplay.Processor.SetFrameInput(input, elapsedTime, (int)Math.Round(UpdateFrequency));

                    Logic.Display.Screen.Process(elapsedTime);
                    errorCount--;
                }
                catch (Exception ex)
                {
                    Logs.Logger.LogError(ex);
                    errorCount += 2;
                }
            }
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);
            if (GameLoaded < GameLoadState.Loaded)
            {
                //Clear color AND stencil buffer
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit);

                Graphics.TextureManager.TextureProgram.SetTextureColor(Color4.White);
                //Move to rendering point
                Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                lock (lockObj)
                    Graphics.TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH / 2, TextureManager.SCREEN_HEIGHT / 2,
                        loadMessage, null, Graphics.AtlasSheet.SpriteVOrigin.Center, Graphics.AtlasSheet.SpriteHOrigin.Center, 0);

                SwapBuffers();
            }
            else if (GameLoaded == GameLoadState.Loaded)
            {
                try
                {

                    //Clear color AND stencil buffer
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit);

                    Graphics.TextureManager.TextureProgram.SetTextureColor(Color4.White);
                    //Move to rendering point
                    Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
                    Graphics.TextureManager.TextureProgram.UpdateModelView();

                    Logic.Display.Screen.Draw((int)Math.Round(RenderFrequency));
                    //Update screen
                    SwapBuffers();
                    errorCount--;
                }
                catch (Exception ex)
                {
                    Logs.Logger.LogError(ex);
                    errorCount += 2;
                }

                if (errorCount > 10)
                    GameLoaded = GameLoadState.Errored;
            }
            else if (GameLoaded == GameLoadState.Errored)
            {
                //Clear color AND stencil buffer
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.StencilBufferBit);

                Graphics.TextureManager.TextureProgram.SetTextureColor(Color4.White);
                //Move to rendering point
                Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                lock (lockObj)
                    Graphics.TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH / 2, TextureManager.SCREEN_HEIGHT / 2,
                        "The program has encountered too many errors and needs to close.\nView the Error Logs for more details.",
                        null, Graphics.AtlasSheet.SpriteVOrigin.Center, Graphics.AtlasSheet.SpriteHOrigin.Center, 12);

                SwapBuffers();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //Console.WindowWidth = Console.LargestWindowWidth;
            //Console.WindowHeight = Console.LargestWindowHeight;

            Application.EnableVisualStyles();

            Logs.Logger.Init();
            Logs.Logger.LogInfo("=== " + DateTime.Now.ToLongTimeString() + " Start up ===");

            
#if RUNNING_GAME
            openGame();

#else
            Thread gameThread = new Thread(openGame);
            gameThread.Start();

            Editors.MainPanel panel = new Editors.MainPanel();
            System.Windows.Forms.Application.Run((Form)panel);
#endif
        }
    }
}
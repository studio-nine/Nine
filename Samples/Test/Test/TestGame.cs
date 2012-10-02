namespace Test
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Nine;
    using Nine.Components;
    using Nine.Graphics;
    using Nine.Graphics.Cameras;
    using Nine.Graphics.Materials;

    public interface ITestGame
    {
        Scene CreateTestScene(GraphicsDevice graphics, ContentManager content);
    }
    
    public class TestGame : Microsoft.Xna.Framework.Game
    {
        Scene scene;
        Scene[] testScenes;
        ITestGame[] testGames;
        int nextTest;
        Input input;

        public TestGame()
        {
#if !SILVERLIGHT
            var graphics = new GraphicsDeviceManager(this);

            //graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;

            Window.AllowUserResizing = true;
#endif

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Components.Add(new FrameRate(GraphicsDevice, Content.Load<SpriteFont>("Consolas")));
            Components.Add(new InputComponent(Window.Handle));

            // Find all test games
            testGames = (from type in Assembly.GetExecutingAssembly().GetTypes().OrderBy(type => type.Name)
                         where type.IsClass && typeof(ITestGame).IsAssignableFrom(type)
                         select (ITestGame)Activator.CreateInstance(type)).ToArray();
            //testGames = new ITestGame[] { new CubeStressTest() };
            testScenes = new Scene[testGames.Length];

            // Shows the next scene
            LoadNextScene();

            // Create an event based input handler.
            // Note that you have to explictly keep a strong reference to the Input instance.
            input = new Input();
#if XBOX
            input.ButtonDown += (sender, e) =>
            {
                if (e.Button == Buttons.A)
                    LoadNextScene();
            };
#elif WINDOWS_PHONE
            input.EnabledGestures = Microsoft.Xna.Framework.Input.Touch.GestureType.DoubleTap;
            input.GestureSampled += (sender, e) =>
            {
                if (e.GestureType == Microsoft.Xna.Framework.Input.Touch.GestureType.DoubleTap)
                    LoadNextScene();
            };
#else
            input.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    LoadNextScene();
            };       
#endif

            base.LoadContent();
        }

        /// <summary>
        /// Loads the next scene.
        /// </summary>
        private void LoadNextScene()
        {
            if (testScenes[nextTest] == null)
                testScenes[nextTest] = testGames[nextTest].CreateTestScene(GraphicsDevice, Content);
            scene = testScenes[nextTest];

            // Gets the drawing context to adjust drawing settings.
            var drawingContext = scene.GetDrawingContext(GraphicsDevice);
            drawingContext.BackgroundColor = new Color(0.5f, 0.5f, 0.5f);
            drawingContext.TextureFilter = TextureFilter.Anisotropic;

            Window.Title = testGames[nextTest].GetType().Name;

            nextTest = (nextTest + 1) % testGames.Length;
        }

        protected override void Update(GameTime gameTime)
        {
#if XBOX
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                Exit();
#endif
            scene.Update(gameTime.ElapsedGameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            scene.Draw(GraphicsDevice, gameTime.ElapsedGameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                scene.DrawDebugOverlay(GraphicsDevice, gameTime.ElapsedGameTime);
            base.Draw(gameTime);
        }

#if WINDOWS || XBOX
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TestGame game = new TestGame())
            {
                game.Run();
            }
        }

#endif
    }

#if SILVERLIGHT
    public class App : System.Windows.Application
    {
        public App()
        {
            Startup += (sender, e) =>
            {
                RootVisual = new TestGame();
            };
        }
    }
#endif
}

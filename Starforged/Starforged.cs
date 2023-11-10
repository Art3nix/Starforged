using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Starforged
{
    public class Starforged : Game {
        private SpriteBatch spriteBatch;
        private Scene nextScene;

        /// <summary>
        /// Current scene
        /// </summary>
        public Scene CurrScene;

        /// <summary>
        /// Graphics device
        /// </summary>
        public static GraphicsDevice gDevice;

        /// <summary>
        /// Graphics device manager
        /// </summary>
        public GraphicsDeviceManager gGraphicsMgr;

        /// <summary>
        /// Player profile and stats
        /// </summary>
        public Player Player;

        /// <summary>
        /// Location of the save game file
        /// </summary>
        public string SaveGamePath = "stats.xml";

        /// <summary>
        /// Constructs the game
        /// </summary>
        public Starforged() {
            gGraphicsMgr = new GraphicsDeviceManager(this);
            gGraphicsMgr.PreferredBackBufferWidth = 1800;
            gGraphicsMgr.PreferredBackBufferHeight = 1000;
            gGraphicsMgr.ApplyChanges();
            gDevice = GraphicsDevice;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize() {

            // Load player
            if (File.Exists(SaveGamePath))
                Load();
            else
                Player = new Player();

            base.Initialize();

            ChangeScene(new TitleScene(this));

        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Save();
                Exit();
            }

            if(nextScene != null) {
                // Start transition
                if(CurrScene != null && CurrScene.State == SceneState.Active) CurrScene.State = SceneState.TransitionOff;

                // Progress transitioning
                if(CurrScene != null && CurrScene.State == SceneState.TransitionOff) CurrScene.updateTransitionOff(gameTime);

                // Change scene after completing transition and start transitionOn in new scene
                if (CurrScene == null || CurrScene.State == SceneState.Inactive) {
                    if (CurrScene != null) CurrScene.UnloadContent();

                    CurrScene = nextScene;
                    nextScene = null;

                    // Update screen size if needed
                    gGraphicsMgr.ApplyChanges();
                    CurrScene.Initialize();
                    CurrScene.State = SceneState.TransitionOn;
                }
            } 

            if(CurrScene.State == SceneState.TransitionOn) {
                // Progress transitionOn
                CurrScene.updateTransitionOn(gameTime);
            }

            if(CurrScene != null) {
                // Update current scene
                CurrScene.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime) {
            if (CurrScene != null) {
                gDevice.Clear(Color.White);
                spriteBatch.Begin();

                CurrScene.Draw(gameTime, spriteBatch);

                spriteBatch.End();
            }


            base.Draw(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public void ChangeScene(Scene next) {
            if (CurrScene != next) {
                nextScene = next;
            }

        }

        public void Save() {
            TextWriter writer = new StreamWriter(SaveGamePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Player));
            serializer.Serialize(writer, Player);
            writer.Close();
        }

        public void Load() {
            TextReader reader = new StreamReader(SaveGamePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Player));
            Player = (Player)serializer.Deserialize(reader);
            reader.Close();

        }
    }
}
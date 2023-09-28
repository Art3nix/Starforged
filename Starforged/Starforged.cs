using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace Starforged
{
    public class Starforged : Game {
        private SpriteBatch spriteBatch;
        private Scene currScene;
        private Scene nextScene;

        /// <summary>
        /// Graphics device
        /// </summary>
        public static GraphicsDevice gDevice;

        /// <summary>
        /// Graphics device manager
        /// </summary>
        public GraphicsDeviceManager gGraphicsMgr;

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(nextScene != null) {
                // Start transition
                if(currScene != null && currScene.State == SceneState.Active) currScene.State = SceneState.TransitionOff;

                // Progress transitioning
                if(currScene != null && currScene.State == SceneState.TransitionOff) currScene.updateTransitionOff(gameTime);

                // Change scene after completing transition and start transitionOn in new scene
                if (currScene == null || currScene.State == SceneState.Inactive) {
                    currScene = nextScene;
                    nextScene = null;

                    // Update screen size if needed
                    gGraphicsMgr.ApplyChanges();
                    currScene.Initialize();
                    currScene.State = SceneState.TransitionOn;
                }
            } 

            if(currScene.State == SceneState.TransitionOn) {
                // Progress transitionOn
                currScene.updateTransitionOn(gameTime);
            }

            if(currScene != null) {
                // Update current scene
                currScene.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime) {
            if (currScene != null) {
                gDevice.Clear(Color.White);
                spriteBatch.Begin();

                currScene.Draw(gameTime, spriteBatch);

                spriteBatch.End();
            }


            base.Draw(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public void ChangeScene(Scene next) {
            if (currScene != next) {
                nextScene = next;
            }

        }
    }
}
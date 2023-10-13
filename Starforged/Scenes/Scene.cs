using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace Starforged {

    public enum SceneState {
        Active,
        TransitionOn,
        TransitionOff,
        Inactive
    }

    public class Scene {
        protected Starforged game;

        protected ContentManager Content;


        /// <summary>
        /// Width of the map
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of the map
        /// </summary>
        public int Height;

        /// <summary>
        /// State of this scene
        /// </summary>
        public SceneState State = SceneState.Inactive;
        /// <summary>
        /// How much time has already elapsed during transition
        /// </summary>
        public float transitionTimeElapsed = 0;
        /// <summary>
        /// Length of transition off in seconds
        /// </summary>
        public float timeTransitionOff;
        /// <summary>
        /// Length of transition on in seconds
        /// </summary>
        public float timeTransitionOn;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public Scene(Starforged g) {
            game = g;   
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        public virtual void Initialize() {
            // Initialize content
            Content = new ContentManager(game.Services);
            Content.RootDirectory = game.Content.RootDirectory;

            LoadContent();
        }

        /// <summary>
        /// Loads game content
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Unloads game content
        /// </summary>
        public virtual void UnloadContent() {
            Content.Unload();
            Content = null;
        }

        /// <summary>
        /// updates the game
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        /// <summary>
        /// Effects to do when loading this screen
        /// </summary>
        public virtual void updateTransitionOn(GameTime gameTime) { }

        /// <summary>
        /// Effects to do when unloading this screen 
        /// </summary>
        public virtual void updateTransitionOff(GameTime gameTime) { }
    }
}
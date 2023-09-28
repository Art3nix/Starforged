using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace Starforged {
    public class Scene {
        protected Starforged game;

        protected ContentManager Content;

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
    }
}
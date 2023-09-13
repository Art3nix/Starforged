using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Starforged {

    /// <summary>
    /// Abstract class representing a ship
    /// </summary>
    public abstract class Ship : CollisionObject {
        protected Texture2D texture;

        // Ship constants
        protected int MAXSPEED;
        protected int SIZE;

        /// <summary>
        /// Flying direction of the ship
        /// </summary>
        protected Vector2 Velocity;

        /// <summary>
        /// Angle of the ship
        /// </summary>
        protected float angle;

        /// <summary>
        /// Position of the ship
        /// </summary>
        protected Vector2 position;


        /// <summary>
        /// Abstract method to load the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="textureName">The name of the texture to load</param>
        public void LoadContent(ContentManager content, string textureName) {
            texture = content.Load<Texture2D>(textureName);
        }

        /// <summary>
        /// Abstract method to update the ship
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
        

        /// <summary>
        /// Abstract method to draw the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);


    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Audio;

namespace Starforged {

    /// <summary>
    /// Abstract class representing a ship
    /// </summary>
    public abstract class Ship : CollisionObject {
        protected Texture2D texture;

        // Ship constants
        public int MAXSPEED;
        public int MAXANGSPEED;
        public int SIZE;

        /// <summary>
        /// Flying direction of the ship
        /// </summary>
        public Vector2 ShipVelocity;

        /// <summary>
        /// Angle of the ship
        /// </summary>
        public float Angle;

        /// <summary>
        /// Position of the ship
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Sound of the engines
        /// </summary>
        protected SoundEffect engineSound;


        /// <summary>
        /// Method to load the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="textureName">The name of the texture to load</param>
        public void LoadContent(ContentManager content, string textureName) {
            texture = content.Load<Texture2D>(textureName);
            engineSound = content.Load<SoundEffect>("music/sfx/engine2");
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Starforged {

    /// <summary>
    /// A class representing an asteroid
    /// </summary>
    public class Projectile : CollisionObject {
        // Texture
        private Texture2D texture;

        // Parameters of the projectile
        private int speed = 250;
        private int size;

        /// <summary>
        /// Position of the projectile
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Constructs a new projectile
        /// </summary>
        public Projectile(ContentManager content, Vector2 pos, Vector2 dir) {

            LoadContent(content);

            Position = pos;
            Velocity = dir;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>("projectile");
            size = texture.Width;
            Mass = size * speed;
            bounds = new BoundingCircle(Position + new Vector2(size / 2, size / 2), size / 2);
        }

        /// <summary>
        /// Update the projectile's position
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {

            //Move in the correct direction
            Position += Velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the bounds position
            bounds.Center.X = Position.X;
            bounds.Center.Y = Position.Y;


        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            //Draw the sprite
            spriteBatch.Draw(texture, Position,
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(size / 2, size / 2), 1f, SpriteEffects.None, 0);
        }

    }
}

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
        private int speed = 400;
        private int size;

        /// <summary>
        /// Position of the projectile
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Damage of the projectile
        /// </summary>
        public int Damage;

        private Color[] colors = new Color[] {
            Color.Fuchsia,
            Color.Red,
            Color.Blue,
            Color.Crimson,
            Color.CadetBlue,
            Color.Magenta,
        };

        private int colorIndex = 0;
        private double colorTimer;
        private Color Color;

        /// <summary>
        /// Constructs a new projectile
        /// </summary>
        public Projectile(ContentManager content, Vector2 pos, Vector2 dir, int damage) {

            LoadContent(content);

            Position = pos;
            Velocity = dir;
            Damage = damage;
            Color = colors[colorIndex];
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

            colorTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (colorTimer > 0.1) {
                colorIndex++;
                if (colorIndex >= colors.Length) colorIndex = 0;
                Color = colors[colorIndex];
                colorTimer -= 0.1;
            }

            //Draw the sprite
            spriteBatch.Draw(texture, Position,
                new Rectangle(0, 0, texture.Width, texture.Height), Color, 0f, new Vector2(size / 2, size / 2), 1f, SpriteEffects.None, 0);
        }

    }
}

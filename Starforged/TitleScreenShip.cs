using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Starforged {

    /// <summary>
    /// A class representing a ship
    /// </summary>
    public class TitleScreenShip {
        private Texture2D texture;
        private int speed = 150;
        private int size = 48;

        private double animationTimer;
        private short animationFrame = 1;

        /// <summary>
        /// Flying direction of the ship
        /// </summary>
        Vector2 direction;

        /// <summary>
        /// Angle of the ship
        /// </summary>
        float angle;

        /// <summary>
        /// Position of the ship
        /// </summary>
        Vector2 position;

        public TitleScreenShip () {
            // Choose random angle
            Random r = new Random();
            angle = r.Next(360);
            direction = new Vector2((float)Math.Sin(angle), (float)-Math.Cos(angle));

            // Choose random position based on the direction
            position = getRandomPosition(direction);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>("ships/ship1");
        }

        /// <summary>
        /// Update the ship's position
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            var windowWidth = Starforged.gDevice.Viewport.Width;
            var windowHeight = Starforged.gDevice.Viewport.Height;

            //Move in the correct direction
            position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Return ship back to the screen
            if ((position.X < -size && direction.X < 0) ||
                (position.Y < -size && direction.Y < 0) ||
                (position.X > windowWidth + size && direction.X > 0) ||
                (position.Y > windowHeight + size && direction.Y > 0)) {

                Random r = new Random();
                angle = r.Next(360);
                direction = new Vector2((float)Math.Sin(angle), (float)-Math.Cos(angle));
                position = getRandomPosition(direction);

            }

        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (animationTimer > 0.1) {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 1;
                animationTimer -= 0.1;

            }

            //Draw the sprite
            var source = new Rectangle(animationFrame * size, 0, size, size);
            var textureCenter = new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, position, source, Color.White, angle, textureCenter, 1f, SpriteEffects.None, 0);
        }

        private Vector2 getRandomPosition(Vector2 dir) {
            Random r = new Random();

            var maxX = Starforged.gDevice.Viewport.Width - size;
            var maxY = Starforged.gDevice.Viewport.Height - size;

            var Position = new Vector2(r.Next(maxX), r.Next(maxY));

            if(dir.X < 0) {
                Position.X += maxX;
            } else {
                Position.X -= maxX;
            }
           
            if(dir.Y < 0) {
                Position.Y += maxY;
            } else {
                Position.Y -= maxY;
            }

            return Position;
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Starforged {

    /// <summary>
    /// A class representing a ship
    /// </summary>
    public class Asteroid {
        private Texture2D[,] texture;
        private int textureIndex;
        private int textureSize;
        private int speed = 75;
        private int size;

        /// <summary>
        /// Flying direction of the ship
        /// </summary>
        public Vector2 Direction;

        /// <summary>
        /// Position of the ship
        /// </summary>
        public Vector2 Position;

        public Asteroid (int tIndex, int tSize) {
            // Choose random position
            Position = getRandomPosition();


            // Choose random direction based on the spawn position
            Direction = getRandomDirection(Position);


            textureIndex = tIndex % 4; //prevent index out of bounds

            textureSize = tSize % 2; //prevent index out of bounds


        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content) {
            //TODO put all asteroids into one file
            texture = new Texture2D[,] {
                {
                    content.Load<Texture2D>("asteroids/asteroid1_s"),
                    content.Load<Texture2D>("asteroids/asteroid1_m")
                }, {
                    content.Load<Texture2D>("asteroids/asteroid2_s"),
                    content.Load<Texture2D>("asteroids/asteroid2_m")
                }, {
                    content.Load<Texture2D>("asteroids/asteroid3_s"),
                    content.Load<Texture2D>("asteroids/asteroid3_m")
                }, {
                    content.Load<Texture2D>("asteroids/asteroid4_s"),
                    content.Load<Texture2D>("asteroids/asteroid4_m")
                }
            };

            size = texture[textureIndex, textureSize].Width;
        }

        /// <summary>
        /// Update the ship's position
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            var windowWidth = Starforged.gDevice.Viewport.Width;
            var windowHeight = Starforged.gDevice.Viewport.Height;

            //Move in the correct direction
            Position += Direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Return ship back to the screen
            if ((Position.X < -size && Direction.X < 0) ||
                (Position.Y < -size && Direction.Y < 0) ||
                (Position.X > windowWidth + size && Direction.X > 0) ||
                (Position.Y > windowHeight + size && Direction.Y > 0)) {

                Position = getRandomPosition();
                Direction = getRandomDirection(Position);
               
            }

        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale = 1f) {

            //Draw the sprite
            //spriteBatch.Draw(texture[textureIndex], Position, Color.White);
            spriteBatch.Draw(texture[textureIndex, textureSize], Position,
                new Rectangle(0, 0, texture[textureIndex, textureSize].Width, texture[textureIndex, textureSize].Height),Color.White, 0f, new Vector2(0,0), scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Choose random direction
        /// </summary>
        /// <returns></returns>
        private Vector2 getRandomDirection(Vector2 pos) {
            Random r = new Random();

            var screenWidth = Starforged.gDevice.Viewport.Width;
            var screenHeight = Starforged.gDevice.Viewport.Height;
            var offsetX = screenWidth / 4;
            var offsetY = screenHeight / 4;

            Vector2 targetPos = new Vector2(r.Next(offsetX, screenWidth - offsetX), r.Next(offsetY, screenHeight - offsetY));
            Vector2 dir = targetPos - pos;
            dir = Vector2.Normalize(dir);

            return dir;
        }

        private Vector2 getRandomPosition() {
            Random r = new Random();

            var screenWidth = Starforged.gDevice.Viewport.Width;
            var screenHeight = Starforged.gDevice.Viewport.Height;

            var Position = new Vector2(r.Next(-screenWidth, screenWidth), r.Next(-screenHeight, screenHeight));

            // Move spawn position out of viewport
            if (Position.X > 0) Position.X += screenWidth;
            else Position.X -= size;

            if (Position.Y > 0) Position.Y += screenHeight;
            else Position.Y -= size;

            return Position;
        }

    }
}

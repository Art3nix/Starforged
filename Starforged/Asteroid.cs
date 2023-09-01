using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Starforged {

    /// <summary>
    /// A class representing a ship
    /// </summary>
    public class Asteroid {
        private Texture2D[] texture;
        private int textureIndex;
        private int speed = 75;
        private int size = 16;

        /// <summary>
        /// Flying direction of the ship
        /// </summary>
        public Direction Direction;

        /// <summary>
        /// Position of the ship
        /// </summary>
        public Vector2 Position;

        public Asteroid (int textureIndex) {
            // Choose random direction
            Direction = getRandomDirection();

            // Choose random position based on the direction
            Position = getRandomPosition(Direction);

            this.textureIndex = textureIndex % 4; //prevent index out of bounds

        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content) {
            //TODO put all asteroids into one file
            texture = new Texture2D[] {
                content.Load<Texture2D>("asteroids/asteroid1_s"),
                content.Load<Texture2D>("asteroids/asteroid2_s"),
                content.Load<Texture2D>("asteroids/asteroid3_s"),
                content.Load<Texture2D>("asteroids/asteroid4_s"),
            };
        }

        /// <summary>
        /// Update the ship's position
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            var windowWidth = Starforged.gDevice.Viewport.Width;
            var windowHeight = Starforged.gDevice.Viewport.Height;

            //Move in the correct direction
            switch (Direction) {
                case Direction.Down:
                    Position += new Vector2(0, 1) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Right:
                    Position += new Vector2(1, 0) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Up:
                    Position += new Vector2(0, -1) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Left:
                    Position += new Vector2(-1, 0) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;

            }


            // Return ship back to the screen
            if ((Position.X < -size && Direction == Direction.Left) ||
                (Position.Y < -size && Direction == Direction.Up) ||
                (Position.X > windowWidth + size && Direction == Direction.Right) ||
                (Position.Y > windowHeight + size && Direction == Direction.Down)) {

                Direction = getRandomDirection();
                Position = getRandomPosition(Direction);

            }

        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            //Draw the sprite
            spriteBatch.Draw(texture[textureIndex], Position, Color.White);
        }

        /// <summary>
        /// Choose random direction
        /// </summary>
        /// <returns></returns>
        private Direction getRandomDirection() {
            var vals = Enum.GetValues(typeof(Direction));
            Random r = new Random();
            return (Direction)vals.GetValue(r.Next(vals.Length));
        }

        private Vector2 getRandomPosition(Direction dir) {
            Random r = new Random();

            var maxX = Starforged.gDevice.Viewport.Width - size;
            var maxY = Starforged.gDevice.Viewport.Height - size;

            var Position = new Vector2(r.Next(maxX), r.Next(maxY));

            switch (dir) {
                case Direction.Left:
                    Position.X += maxX;
                    break;
                case Direction.Right:
                    Position.X -= maxX;
                    break;
                case Direction.Up:
                    Position.Y += maxY;
                    break;
                case Direction.Down:
                    Position.Y -= maxY;
                    break;
            }

            return Position;
        }

    }
}

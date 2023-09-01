using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Starforged {

    public enum Direction {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    /// <summary>
    /// A class representing a ship
    /// </summary>
    public class Ship {
        private Texture2D texture;
        private int speed = 300;
        private int size = 48;

        private double animationTimer;
        private short animationFrame = 1;

        /// <summary>
        /// Flying direction of the ship
        /// </summary>
        public Direction Direction;

        /// <summary>
        /// Position of the ship
        /// </summary>
        public Vector2 Position;

        public Ship () {
            // Choose random direction
            Direction = getRandomDirection();

            // Choose random position based on the direction
            Position = getRandomPosition(Direction);
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

            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (animationTimer > 0.1) {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 1;
                animationTimer -= 0.1;

            }

            //Draw the sprite
            var source = new Rectangle(animationFrame * size, (int)Direction * size, size, size);
            spriteBatch.Draw(texture, Position, source, Color.White);
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

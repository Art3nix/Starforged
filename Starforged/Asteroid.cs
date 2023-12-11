using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Starforged {

    /// <summary>
    /// A class representing an asteroid
    /// </summary>
    public class Asteroid : CollisionObject{

        private Starforged game;

        // Texture
        private Texture2D texture;
        private String textureName;

        // Parameters of the asteroid
        private int speed = 75;
        private int size;

        /// <summary>
        /// Position of the asteroid
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Constructs a new asteroid
        /// </summary>
        /// <param name="tIndex">type of the asteroid texture</param>
        /// <param name="tSize">size of the asteroid texture</param>
        public Asteroid (Starforged g, int tIndex, int tSize) {
            game = g;

            Respawn();

            tIndex = tIndex % 4 + 1; //prevent index out of bounds
            tSize = tSize % 3; //prevent index out of bounds

            var textSize = "s";
            switch (tSize) {
                case 2:
                    textSize = "l";
                    break;
                case 1:
                    textSize = "m";
                    break;
                case 0:
                    textSize = "s";
                    break;

            }
            textureName = "asteroids/asteroid" + tIndex + "_" + textSize;



        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>(textureName);
            size = texture.Width;
            Mass = size; // in tons
            Bounds = new BoundingCircle(Position + new Vector2(size / 2, size / 2), size / 2);
        }

        /// <summary>
        /// Update the asteroid's position
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {

            // TODO maybe add class variable
            int sceneWidth, sceneHeight;
            if (game.CurrScene != null) {
                sceneWidth = game.CurrScene.Width;
                sceneHeight = game.CurrScene.Height;
            } else {
                sceneWidth = Starforged.gDevice.Viewport.Width;
                sceneHeight = Starforged.gDevice.Viewport.Height;
            }

            //Move in the correct direction
            Position += Velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Return ship back to the screen
            if ((Position.X < -size && Velocity.X < 0) ||
                (Position.Y < -size && Velocity.Y < 0) ||
                (Position.X > sceneWidth + size && Velocity.X > 0) ||
                (Position.Y > sceneHeight + size && Velocity.Y > 0)) {

                Position = getRandomPosition();
                Velocity = getRandomDirection(Position);
               
            }

            // Update the bounds position
            Bounds.Center.X = Position.X;
            Bounds.Center.Y = Position.Y;


        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale = 1f) {

            //Draw the sprite
            //spriteBatch.Draw(texture[textureIndex], Position, Color.White);
            spriteBatch.Draw(texture, Position,
                new Rectangle(0, 0, texture.Width, texture.Height),Color.White, 0f, new Vector2(size/2, size/2), scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Choose random direction based on the position
        /// </summary>
        /// <returns>Random direction</returns>
        private Vector2 getRandomDirection(Vector2 pos) {
            Random r = new Random();

            int sceneWidth, sceneHeight;
            if (game.CurrScene != null) {
                sceneWidth = game.CurrScene.Width;
                sceneHeight = game.CurrScene.Height;
            } else {
                sceneWidth = Starforged.gDevice.Viewport.Width;
                sceneHeight = Starforged.gDevice.Viewport.Height;
            }

            var offsetX = sceneWidth / 4;
            var offsetY = sceneHeight / 4;

            Vector2 targetPos = new Vector2(r.Next(offsetX, sceneWidth - offsetX), r.Next(offsetY, sceneHeight - offsetY));
            Vector2 dir = targetPos - pos;
            dir = Vector2.Normalize(dir);

            return dir;
        }

        /// <summary>
        /// Choose a random position
        /// </summary>
        /// <returns>Random position</returns>
        private Vector2 getRandomPosition() {
            Random r = new Random();

            int sceneWidth, sceneHeight;
            if (game.CurrScene != null) {
                sceneWidth = game.CurrScene.Width;
                sceneHeight = game.CurrScene.Height;
            } else {
                sceneWidth = Starforged.gDevice.Viewport.Width;
                sceneHeight = Starforged.gDevice.Viewport.Height;
            }

            var Position = new Vector2(r.Next(-sceneWidth, sceneWidth), r.Next(-sceneHeight, sceneHeight));

            // Move spawn position out of viewport
            if (Position.X > 0) Position.X += sceneWidth;
            else Position.X -= size;

            if (Position.Y > 0) Position.Y += sceneHeight;
            else Position.Y -= size;

            return Position;
        }

        public void Respawn() {
            // Choose random position
            Position = getRandomPosition();


            // Choose random direction based on the spawn position
            Velocity = getRandomDirection(Position);

        }

    }
}

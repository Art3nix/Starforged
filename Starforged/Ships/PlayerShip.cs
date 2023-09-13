using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace Starforged {

    /// <summary>
    /// A class representing a ship
    /// </summary>
    public class PlayerShip : Ship {

        // Animation values
        private double animationTimer;
        private short animationFrame = 1;

        // Movement
        private Vector2 velocity;
        private float angVelocity;

        // Ship constants
        private float LIN_ACCELERATION = 70;
        private float ANG_ACCELERATION = 2.5f;



        public PlayerShip() {
            // Choose random angle
            Random r = new Random();
            angle = r.Next(360);
            direction = new Vector2((float)Math.Sin(angle), (float)-Math.Cos(angle));

            // Choose random position based on the direction
            position = new Vector2(Starforged.gDevice.Viewport.Width / 2, Starforged.gDevice.Viewport.Height / 2);

            // Init values
            MAXSPEED = 150;
            SIZE = 48;
    }

        /// <summary>
        /// Update the ship's position
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            var windowWidth = Starforged.gDevice.Viewport.Width;
            var windowHeight = Starforged.gDevice.Viewport.Height;

            //Move in the correct direction
            UpdateMovement(gameTime);


            // Keep ship on the screen
            var viewport = Starforged.gDevice.Viewport;
            if (position.X < 0 && velocity.X < 0) {
                velocity.X = 0;
                position.X = 0;
            }
            if (position.X > viewport.Width && velocity.X > viewport.Width) {
                velocity.X = 0;
                position.X = 0;
            }
            if (position.Y < 0 && velocity.Y < 0) {
                velocity.Y = 0;
                position.Y = 0;
            }
            if (position.Y > viewport.Height && velocity.Y > viewport.Height) {
                velocity.Y = 0;
                position.Y = 0;
            }
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (animationTimer > 0.1) {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 1;
                animationTimer -= 0.1;

            }

            //Draw the sprite
            var source = new Rectangle(animationFrame * base.SIZE, 0, base.SIZE, base.SIZE);
            var textureCenter = new Vector2(SIZE / 2, SIZE / 2);
            spriteBatch.Draw(texture, position, source, Color.White, angle, textureCenter, 1f, SpriteEffects.None, 0);
        }

        public void UpdateMovement(GameTime gameTime) {
            KeyboardState kbState = Keyboard.GetState();
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acc = new Vector2(0, 0);
            float angAcc = 0;

            if (kbState.IsKeyDown(Keys.Left)) {
                angAcc -= ANG_ACCELERATION;
            }
            if (kbState.IsKeyDown(Keys.Right)) {
                angAcc += ANG_ACCELERATION;
            }

            if (kbState.IsKeyDown(Keys.Up)) {
                acc += direction * LIN_ACCELERATION;
            }
            if (kbState.IsKeyDown(Keys.Down)) {
                acc -= direction * LIN_ACCELERATION;
            }

            angVelocity += angAcc * time;
            angle += angVelocity * time;
            direction.X = (float)Math.Sin(angle);
            direction.Y = (float)-Math.Cos(angle);

            velocity += acc * time;
            position += velocity * time;

        }

    }
}

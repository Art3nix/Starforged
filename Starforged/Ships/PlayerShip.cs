using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        private Vector2 direction;
        private float angVelocity;

        // Ship constants
        private float LIN_ACCELERATION = 70;
        private float ANG_ACCELERATION = 2.5f;

        private SoundEffectInstance engineSoundInstance;




        public PlayerShip(ContentManager content, String textureName) {

            // Load textures
            LoadContent(content, textureName);

            // Choose random angle
            Random r = new Random();
            angle = r.Next(360);
            direction = new Vector2((float)Math.Sin(angle), (float)-Math.Cos(angle));

            // Choose random position based on the direction
            position = new Vector2(Starforged.gDevice.Viewport.Width / 2, Starforged.gDevice.Viewport.Height / 2);

            // Init values
            MAXSPEED = 150;
            SIZE = 48;
            Mass = SIZE; // in tons

            bounds = new BoundingCircle(position + new Vector2(SIZE / 2, SIZE / 2), SIZE / 2);

            engineSoundInstance = engineSound.CreateInstance();
            engineSoundInstance.IsLooped = true;
            engineSoundInstance.Volume = 0.5f;
            engineSoundInstance.Play();
        }

        /// <summary>
        /// Update the ship's position
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {

            //Move in the correct direction
            UpdateMovement(gameTime);


            // Keep ship on the screen
            var viewport = Starforged.gDevice.Viewport;
            var r = SIZE / 2;
            if (position.X - r <= 0) {
                ShipVelocity.X = 0;
                position.X = r;
            }
            if (position.X + r >= viewport.Width) {
                ShipVelocity.X = 0;
                position.X = viewport.Width - r;
            }
            if (position.Y - r <= 0) {
                ShipVelocity.Y = 0;
                position.Y = r;
            }
            if (position.Y + r >= viewport.Height) {
                ShipVelocity.Y = 0;
                position.Y = viewport.Height - r;
            }


            // Update the bounds position
            bounds.Center.X = position.X;
            bounds.Center.Y = position.Y;
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


            float pitch = 0.0f;

            if (kbState.IsKeyDown(Keys.Left)) {
                angAcc -= ANG_ACCELERATION;
            }
            if (kbState.IsKeyDown(Keys.Right)) {
                angAcc += ANG_ACCELERATION;
            }

            if (kbState.IsKeyDown(Keys.Up)) {
                acc += direction * LIN_ACCELERATION;
                pitch = .5f;
            }
            if (kbState.IsKeyDown(Keys.Down)) {
                acc -= direction * LIN_ACCELERATION;
                pitch = -.75f;
            }

            angVelocity += angAcc * time;
            angle += angVelocity * time;
            direction.X = (float)Math.Sin(angle);
            direction.Y = (float)-Math.Cos(angle);

            ShipVelocity += acc * time;
            position += ShipVelocity * time;

            engineSoundInstance.Pitch = pitch;

        }

    }
}

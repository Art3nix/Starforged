using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Starforged {

    /// <summary>
    /// A class representing a ship
    /// </summary>
    public class EnemyShip : Ship {

        // Animation values
        private double animationTimer;
        private short animationFrame = 1;

        // Movement
        private Vector2 direction;
        private float angVelocity;

        // Ship constants
        private float LIN_ACCELERATION = 70;
        private float ANG_ACCELERATION = 2.5f;

        public Ship Target;

        public Starforged game;

        public float ShootDelay = 0;
        public bool EnteredBounds = false;


        public EnemyShip() {

            // Choose random angle
            Random r = new Random();
            Angle = r.Next(360);
            direction = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));

            // Choose random position based on the direction
            Position = new Vector2(Starforged.gDevice.Viewport.Width / 2, Starforged.gDevice.Viewport.Height / 2);

            // Init values
            MaxSpeed = 150;
            MaxAngSpeed = 5;
            SIZE = 48;
            Mass = SIZE; // in tons
            Health = 40;
            MaxHealth = 40;
            Damage = 15;
            ProjectileSpeed = 300;


            Bounds = new BoundingCircle(Position + new Vector2(SIZE / 2, SIZE / 2), SIZE / 2);

            Target = null;
        }

        public EnemyShip(ContentManager content, String textureName, Ship target, Starforged game) {

            // Load textures
            if (content != null) {
                LoadContent(content, textureName);
            }

            this.Target = target;
            this.game = game;

            // Init values
            MaxSpeed = 150;
            MaxAngSpeed = 10;
            SIZE = 48;
            Mass = SIZE; // in tons
            Health = 40;
            MaxHealth = 40;
            Damage = 15;
            ProjectileSpeed = 300;

            // Choose random position
            Position = getRandomPosition();


            Bounds = new BoundingCircle(Position + new Vector2(SIZE / 2, SIZE / 2), SIZE / 2);


            // Set angle to target
            Angle = (float)Math.Atan2(Bounds.Center.Y - target.Bounds.Center.Y,
                                             Bounds.Center.X - target.Bounds.Center.X) - MathHelper.PiOver2;
            direction = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));

        }

        /// <summary>
        /// Update the ship's position
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            KeyboardState kbState = Keyboard.GetState();
            //Move in the correct direction
            UpdateMovement(gameTime);

            ShootDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Update the bounds position
            Bounds.Center.X = Position.X;
            Bounds.Center.Y = Position.Y;
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
            spriteBatch.Draw(texture, Position, source, Color.White, Angle, textureCenter, 1f, SpriteEffects.None, 0);
        }

        public void UpdateMovement(GameTime gameTime) {
            KeyboardState kbState = Keyboard.GetState();
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acc = new Vector2(0, 0);
            float angAcc = 0;


            float targetAngle = (float)Math.Atan2(Bounds.Center.Y - Target.Bounds.Center.Y,
                                             Bounds.Center.X - Target.Bounds.Center.X) - MathHelper.PiOver2;

/*          TODO figure out the math
            TargetAngle = targetAngle;

            if ((angVelocity > 0 && Angle > targetAngle) || //slow down
                (angVelocity <= 0 && Angle > targetAngle)) { //speed up
                angAcc -= ANG_ACCELERATION * Math.Abs(Angle - targetAngle);
            } else if ((angVelocity < 0 && Angle < targetAngle) ||  //slow down
                       (angVelocity >= 0 && Angle < targetAngle)) {  //speed up
                angAcc += ANG_ACCELERATION * Math.Abs(Angle - targetAngle);
            }*/


            double distance = Math.Pow(Target.Bounds.Center.X - Bounds.Center.X, 2) +
                              Math.Pow(Target.Bounds.Center.Y - Bounds.Center.Y, 2);
            var range = 90000; // 300px^
            if (distance > range) {
                acc += direction * LIN_ACCELERATION;
            }
            else if (distance < range) {
                acc -= direction * LIN_ACCELERATION;
            } else if (ShipVelocity != Vector2.Zero) {
                // Slow down ship if dampers are on
                acc += (-Vector2.Normalize(ShipVelocity)) * LIN_ACCELERATION;
            }

            angVelocity += angAcc * time;
            ShipVelocity += acc * time;

            // Clamp values
            ShipVelocity.X = Math.Clamp(ShipVelocity.X, -MaxSpeed, +MaxSpeed);
            ShipVelocity.Y = Math.Clamp(ShipVelocity.Y, -MaxSpeed, +MaxSpeed);
            angVelocity = Math.Clamp(angVelocity, -MaxAngSpeed, +MaxAngSpeed);

            if (Math.Abs(angVelocity) < 0.01f) angVelocity = 0f;

            Angle = targetAngle;
            direction.X = (float)Math.Sin(Angle);
            direction.Y = (float)-Math.Cos(Angle);
            Position += ShipVelocity * time;

            if(!EnteredBounds &&
                Position.X > 0 && Position.X < game.CurrScene.Width &&
                Position.Y > 0 && Position.Y < game.CurrScene.Height) {
                EnteredBounds = true;
            } 


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
            else Position.X -= SIZE;

            if (Position.Y > 0) Position.Y += sceneHeight;
            else Position.Y -= SIZE;
            
            return Position;
        }

        public void Respawn() {
            // Choose random position
            Position = getRandomPosition();


            // Set angle to target
            Angle = (float)-Math.Atan2(Target.Bounds.Center.Y - Bounds.Center.Y,
                                             Target.Bounds.Center.X - Bounds.Center.X);
            direction = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));

        }

        public bool TargetInRange() {
            return (Math.Pow(Target.Bounds.Center.X - Bounds.Center.X, 2) +
                   Math.Pow(Target.Bounds.Center.Y - Bounds.Center.Y, 2)) < 160000; //400px^2
        }

        public void Despawn() {
            EnteredBounds = false;
            Position = new Vector2(-1000, -1000);
        }
    }
}

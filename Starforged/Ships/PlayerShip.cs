using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Starforged {


    /// <summary>
    /// A class representing a ship
    /// </summary>
    [DataContract]
    public class PlayerShip : Ship {

        // Animation values
        private double animationTimer;
        private short animationFrame = 1;

        // Movement
        private Vector2 direction;
        private float angVelocity;

        // Ship constants
        private float ANG_FCONSUMPTION = 0.1f;
        private float LIN_FCONSUMPTION = 0.3f;
        private int HEALTH_INC = 20;
        private int SPEED_INC = 40;
        private int LIN_ACC_INC = 25;
        private int ANG_SPEED_INC = 2;
        private float ANG_ACC_INC = 0.2f;
        private int DAMAGE_INC = 10;
        private int PROJ_SPEED_INC = 100;

        // Ship parameters
        public float LinAcceleration;
        public float AngAcceleration;


        private SoundEffectInstance engineSoundInstance;

        // Whether inertia dampers are turned on or off
        public bool InertiaDampers = true;

        private Starforged game;


        public PlayerShip() {

            game = null;

            // Choose random angle
            Random r = new Random();
            Angle = r.Next(360);
            direction = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));

            // Choose random position based on the direction
            Position = new Vector2(Starforged.gDevice.Viewport.Width / 2, Starforged.gDevice.Viewport.Height / 2);

            // Init values
            SIZE = 48;
            Mass = SIZE; // in tons
            MaxHealth = 100 + HEALTH_INC * game.Player.ShipUpgradeLevels[UpgradeType.Health];
            Health = MaxHealth;
            MaxSpeed = 150 + SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Speed];
            LinAcceleration = 70 + LIN_ACC_INC * game.Player.ShipUpgradeLevels[UpgradeType.Speed];
            MaxAngSpeed = 50 + ANG_SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Agility];
            AngAcceleration = 2.5f + ANG_ACC_INC * game.Player.ShipUpgradeLevels[UpgradeType.Agility];
            Damage = 20 + DAMAGE_INC * game.Player.ShipUpgradeLevels[UpgradeType.Damage];
            ProjectileSpeed = 400 + PROJ_SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.ProjSpeed];

            Bounds = new BoundingCircle(Position + new Vector2(SIZE / 2, SIZE / 2), SIZE / 2);

            engineSoundInstance = engineSound.CreateInstance();
            engineSoundInstance.IsLooped = true;
            engineSoundInstance.Volume = 0.1f;
            engineSoundInstance.Play();

        }

        public PlayerShip(Starforged g, ContentManager content = null, String textureName = null) {

            game = g;

            // Load textures
            if (content != null) {
                LoadContent(content, textureName);
            }

            // Choose random angle
            Random r = new Random();
            Angle = r.Next(360);
            direction = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));

            // Choose random position based on the direction
            Position = new Vector2(Starforged.gDevice.Viewport.Width / 2, Starforged.gDevice.Viewport.Height / 2);

            // Init values
            SIZE = 48;
            Mass = SIZE; // in tons
            MaxHealth = 100 + HEALTH_INC * game.Player.ShipUpgradeLevels[UpgradeType.Health];
            Health = MaxHealth;
            MaxSpeed = 150 + SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Speed];
            LinAcceleration = 70 + SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Speed];
            MaxAngSpeed = 50 + ANG_SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Agility];
            AngAcceleration = 2.5f + ANG_SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Agility];
            Damage = 20 + DAMAGE_INC * game.Player.ShipUpgradeLevels[UpgradeType.Damage];
            ProjectileSpeed = 400 + PROJ_SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.ProjSpeed];

            Bounds = new BoundingCircle(Position + new Vector2(SIZE / 2, SIZE / 2), SIZE / 2);

            engineSoundInstance = engineSound.CreateInstance();
            engineSoundInstance.IsLooped = true;
            engineSoundInstance.Volume = 0.1f;
            engineSoundInstance.Play();
        }

        /// <summary>
        /// Update the ship's position
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            KeyboardState kbState = Keyboard.GetState();
            //Move in the correct direction
            UpdateMovement(gameTime);

            // Update variables based on upgrades
            MaxHealth = 100 + HEALTH_INC * game.Player.ShipUpgradeLevels[UpgradeType.Health];
            MaxSpeed = 150 + SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Speed];
            LinAcceleration = 70 + SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Speed];
            MaxAngSpeed = 50 + ANG_SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Agility];
            AngAcceleration = 2.5f + ANG_SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.Agility];
            Damage = 20 + DAMAGE_INC * game.Player.ShipUpgradeLevels[UpgradeType.Damage];
            ProjectileSpeed = 400 + PROJ_SPEED_INC * game.Player.ShipUpgradeLevels[UpgradeType.ProjSpeed];



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
            float fuelConsumed = 0;

            Vector2 acc = new Vector2(0, 0);
            float angAcc = 0;


            float pitch = 0.0f;

            if (kbState.IsKeyDown(Keys.Left)) {
                angAcc -= AngAcceleration;
                fuelConsumed += ANG_FCONSUMPTION * time;
            } else if (kbState.IsKeyDown(Keys.Right)) {
                angAcc += AngAcceleration;
                fuelConsumed += ANG_FCONSUMPTION * time;
            } else if (InertiaDampers) {
                // Slow down angular movement if dampers are on
                if (angVelocity > 0) {
                    angAcc -= AngAcceleration;
                } else if (angVelocity < 0) {
                    angAcc += AngAcceleration;
                }
                if (angVelocity > 0.1f) {
                    fuelConsumed += ANG_FCONSUMPTION * time;
                }

            }

            if (kbState.IsKeyDown(Keys.Up)) {
                acc += direction * LinAcceleration;
                fuelConsumed += LIN_FCONSUMPTION * time;
                pitch = .5f;
            }
            else if (kbState.IsKeyDown(Keys.Down)) {
                acc -= direction * LinAcceleration;
                fuelConsumed += LIN_FCONSUMPTION * time;
                pitch = -.75f;
            } 

            if (InertiaDampers && ShipVelocity != Vector2.Zero) {
                // Slow down ship if dampers are on
                if (Vector2.Distance(ShipVelocity, Vector2.Zero) > Math.Sqrt(2)) {
                    // do not consume fuel if ship almost not moving
                    fuelConsumed += LIN_FCONSUMPTION * time;
                    
                }
                
                
                if (kbState.IsKeyDown(Keys.Down) || kbState.IsKeyDown(Keys.Up)) {
                    // if user input move the ship sideways in opposite direction that it is going
                    Vector2 norm = new Vector2(direction.Y, -direction.X);  //normal vector to direction
                    if (Vector2.Distance(norm, ShipVelocity) < Vector2.Distance(-norm, ShipVelocity))
                        norm = -norm;
                    acc += norm * LinAcceleration;
                } else {
                    // no user input means slow ship in opposite direction
                    acc += (-Vector2.Normalize(ShipVelocity)) * LinAcceleration;
                }
            }

            if (game.Player.Fuel > 0f) {
                game.Player.Fuel -= fuelConsumed;

                angVelocity += angAcc * time;
                ShipVelocity += acc * time;
            } else {
                game.Player.Fuel = 0f;
            }

            // Clamp values
            ShipVelocity.X = Math.Clamp(ShipVelocity.X, -MaxSpeed, +MaxSpeed);
            ShipVelocity.Y = Math.Clamp(ShipVelocity.Y, -MaxSpeed, +MaxSpeed);
            angVelocity = Math.Clamp(angVelocity, -MaxAngSpeed, +MaxAngSpeed);


            Angle += angVelocity * time;
            direction.X = (float)Math.Sin(Angle);
            direction.Y = (float)-Math.Cos(Angle);
            Position += ShipVelocity * time;


            engineSoundInstance.Pitch = pitch;

        }

    }
}

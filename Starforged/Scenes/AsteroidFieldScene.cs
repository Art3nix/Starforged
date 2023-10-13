using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;
using System.Collections.Generic;
using Starforged.Particles;

namespace Starforged {
    public class AsteroidFieldScene : Scene {

        // Background
        private Background background;
        private Texture2D splash;

        // Fonts
        private SpriteFont textFont;

        // Ships
        private PlayerShip ship;

        // Asteroids
        private Asteroid[] asteroids;

        // Ship projectiles
        private List<Projectile> projectiles = new List<Projectile>();

        // Collision sound
        private SoundEffect collisionSound;

        private KeyboardState priorKeyboardState;

        private Hud hud;

        ExplosionParticleSystem explosionParticles;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public AsteroidFieldScene(Starforged g) : base(g) {
            game.gGraphicsMgr.PreferredBackBufferWidth = 1800;
            game.gGraphicsMgr.PreferredBackBufferHeight = 1000;

            Width = 2600;
            Height = 2600;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        public override void Initialize() {

            // Initialize background
            background = new TiledBackground(Width, Height);

            // Initialize asteroids
            Random r = new Random();
            asteroids = new Asteroid[40];
            for (var i = 0; i < asteroids.Length; i++) {
                asteroids[i] = new Asteroid(game, r.Next(4), r.Next(3));
            }

            // Initialize particles
            explosionParticles = new ExplosionParticleSystem(game, 1500);
            game.Components.Add(explosionParticles);

            // Initialize hud
            hud = new Hud(game);

            // Transition times
            timeTransitionOn = 2;
            timeTransitionOff = 4;

            base.Initialize();


            // Initialize ships
            ship = new PlayerShip(Content, "ships/ship1");

        }

        /// <summary>
        /// Loads game content
        /// </summary>
        public override void LoadContent() {

            base.LoadContent();

            // Load background
            background.LoadContent(Content, "background/space_tile");
            splash = Content.Load<Texture2D>("background/black_splash");

            // Load Hud
            hud.LoadContent(Content);

            // Load font
            textFont = Content.Load<SpriteFont>("millennia");

            // Load asteroids
            foreach (var asteroid in asteroids) asteroid.LoadContent(Content);

            // Load sfx
            collisionSound = Content.Load<SoundEffect>("music/sfx/collision");

        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        public override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();


            // Turn on/off inertia dampers
            if (Keyboard.GetState().IsKeyDown(Keys.Z) && priorKeyboardState.IsKeyUp(Keys.Z)) {
                ship.InertiaDampers = !ship.InertiaDampers;
            }

            // Update ship
            ship.Update(gameTime);

            // Update projectile positions
            foreach (var p in projectiles) p.Update(gameTime);


            // Update asteroids
            for (int i = 0; i < asteroids.Length; i++) {
                asteroids[i].Update(gameTime);

                // Collision between two asteroids
                for (int j = i + 1; j < asteroids.Length; j++) {
                    if (CollisionHelper.handleElasticCollision(asteroids[i], asteroids[j])) {
                        collisionSound.Play();
                    }
                }

                // Collision between an asteroid and a ship
                if(CollisionHelper.handleElasticCollision(asteroids[i], ship)) {
                    collisionSound.Play();
                }

                // Collision between an asteroid and a projectile
                for (int j = 0; j < projectiles.Count; j++) {
                    if (CollisionHelper.Collides(asteroids[i].Bounds, projectiles[j].Bounds)) {
                        // Particle
                        explosionParticles.AddExplosion(asteroids[i].Bounds.Center);

                        asteroids[i].Respawn();
                        projectiles.RemoveAt(j);
                    }
                }
            }

            // Keep ship on the map
            var r = ship.SIZE / 2;
            if (ship.Position.X - r <= 0) {
                ship.ShipVelocity.X = 0;
                ship.Position.X = r;
            }
            if (ship.Position.X + r >= Width) {
                ship.ShipVelocity.X = 0;
                ship.Position.X = Width - r;
            }
            if (ship.Position.Y - r <= 0) {
                ship.ShipVelocity.Y = 0;
                ship.Position.Y = r;
            }
            if (ship.Position.Y + r >= Height) {
                ship.ShipVelocity.Y = 0;
                ship.Position.Y = Height - r;
            }

            // Shoot
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && priorKeyboardState.IsKeyUp(Keys.Space) && game.Player.Ammo > 0) {
                float projOffset = 24;
                Vector2 projPos = new Vector2((float)(ship.Position.X + projOffset * Math.Sin(ship.Angle)),
                                              (float)(ship.Position.Y + projOffset * -Math.Cos(ship.Angle)));
                Vector2 projDir = new Vector2((float)Math.Sin(ship.Angle),
                                              (float)-Math.Cos(ship.Angle));
                Projectile proj = new Projectile(Content, projPos, projDir);
                projectiles.Add(proj);
                game.Player.Ammo--;
            }

            priorKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            spriteBatch.End();
            // Variables for scrolling background
            Vector2 screenCenter = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
            Vector2 playerPos = new Vector2(MathHelper.Clamp(ship.Position.X, screenCenter.X, Width - screenCenter.X), MathHelper.Clamp(ship.Position.Y, screenCenter.Y, Height - screenCenter.Y));
            Vector2 offset = new Vector2(screenCenter.X - playerPos.X, screenCenter.Y - playerPos.Y);
            Matrix transform = Matrix.CreateTranslation(offset.X, offset.Y, 0);

            // Background batch
            spriteBatch.Begin(transformMatrix: transform);

            background.Draw(spriteBatch);
            spriteBatch.End();


            // Objects batch
            transform = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin(transformMatrix: transform);

            // Add transformation to particles
            explosionParticles.TransformMatrix = transform;

            // Draw asteroids
            foreach (var asteroid in asteroids) {
                asteroid.Draw(gameTime, spriteBatch);
            }

            // Draw ship
            ship.Draw(gameTime, spriteBatch);

            // Update projectile texture
            foreach (var p in projectiles) p.Draw(gameTime, spriteBatch);

            spriteBatch.End();


            // Static objects batch
            spriteBatch.Begin();

            // Draw text
            var dampersScale = 0.8f;
            var dampersText = "Dampers: ";
            var dampersStatus = ship.InertiaDampers ? "On" : "Off";
            var dampersColor = ship.InertiaDampers ? Color.LimeGreen : Color.Red;
            var dampersPos = new Vector2(10, game.GraphicsDevice.Viewport.Height - textFont.LineSpacing);
            spriteBatch.DrawString(textFont, dampersText, dampersPos, Color.White, 0f, new Vector2(0,0), dampersScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, dampersStatus, dampersPos + new Vector2(textFont.MeasureString(dampersText).X * dampersScale, 0), dampersColor, 0f, new Vector2(0, 0), dampersScale, SpriteEffects.None, 0);


            // Draw HUD
            hud.Draw(gameTime, spriteBatch);

            // Fade out transition
            if (State == SceneState.TransitionOff) {
                var rect = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Bounds.Width, game.GraphicsDevice.Viewport.Bounds.Height);
                float alpha = (float)Math.Pow(transitionTimeElapsed / timeTransitionOff, 2);
                spriteBatch.Draw(splash, rect, Color.White * alpha);
            }

            // Fade in transition
            if (State == SceneState.TransitionOn) {
                var rect = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Bounds.Width, game.GraphicsDevice.Viewport.Bounds.Height);
                float alpha = 1 - (float)Math.Pow(transitionTimeElapsed / timeTransitionOn, 2);
                spriteBatch.Draw(splash, rect, Color.White * alpha);
            }
        }

        public override void updateTransitionOn(GameTime gameTime) {
            if (transitionTimeElapsed > timeTransitionOn) State = SceneState.Active;
            else transitionTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void updateTransitionOff(GameTime gameTime) {
            if (transitionTimeElapsed > timeTransitionOff) State = SceneState.Inactive;
            else transitionTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
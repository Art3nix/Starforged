using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;
using System.Collections.Generic;
using Starforged.Particles;
using SharpDX.Direct3D9;

namespace Starforged {
    public class EnemyShipScene : Scene {

        // Background
        private Background background;
        private Texture2D splash;

        // Fonts
        private SpriteFont textFont;

        // Ships
        private PlayerShip ship;

        // Asteroids
        private EnemyShip[] enemies;

        // Ship projectiles
        private List<Projectile> projectiles = new List<Projectile>();
        private List<Projectile> enemyProjectiles = new List<Projectile>();

        // Collision sound
        private SoundEffect collisionSound;

        private KeyboardState priorKeyboardState;

        private Hud hud;
        private Texture2D viewportRectangle;
        private Texture2D playerIcon;

        private ExplosionParticleSystem explosionParticles;

        /// <summary>
        /// True if map is shown
        /// </summary>
        bool showMap = false;
        private Map map;
        private Background mapBackground;
        private int mapPadding = 60;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public EnemyShipScene(Starforged g) : base(g) {
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

            // Initialize particles
            explosionParticles = new ExplosionParticleSystem(game, 1500);
            game.Components.Add(explosionParticles);

            // Initialize hud
            hud = new Hud(game);

            // Initialize map
            mapBackground = new TiledBackground(game.gGraphicsMgr.PreferredBackBufferWidth - 2 * mapPadding,
                                                game.gGraphicsMgr.PreferredBackBufferHeight - 2 * mapPadding,
                                                mapPadding,
                                                mapPadding);

            // Transition times
            timeTransitionOn = 2;
            timeTransitionOff = 4;

            base.Initialize();


            // Initialize ships
            ship = new PlayerShip(Content, "ships/ship1");

            // Initialize enemies
            enemies = new EnemyShip[40];
            for (var i = 0; i < enemies.Length; i++) {
                enemies[i] = new EnemyShip(Content, "ships/enemyship", ship, game);
            }


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
            viewportRectangle = Content.Load<Texture2D>("rectangle");
            playerIcon = Content.Load<Texture2D>("player");

            // Load map
            map = Content.Load<Map>("map");
            mapBackground.LoadContent(Content, "background/space_tile");

            // Load font
            textFont = Content.Load<SpriteFont>("millennia");

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

            // Show/hide the map
            if (Keyboard.GetState().IsKeyDown(Keys.M) && priorKeyboardState.IsKeyUp(Keys.M)) {
                showMap = !showMap;
            }

            if (showMap) {
                // don't update the scene if the map is displayed
                priorKeyboardState = Keyboard.GetState();
                return;
            }

            // Turn on/off inertia dampers
            if (Keyboard.GetState().IsKeyDown(Keys.Z) && priorKeyboardState.IsKeyUp(Keys.Z)) {
                ship.InertiaDampers = !ship.InertiaDampers;
            }

            // Update ship
            ship.Update(gameTime);

            // Update projectile positions
            foreach (var p in projectiles) p.Update(gameTime);
            foreach (var p in enemyProjectiles) p.Update(gameTime);


            // Update enemies
            for (int i = 0; i < enemies.Length; i++) {
                enemies[i].Update(gameTime);

                if (enemies[i].TargetInRange() && enemies[i].ShootDelay >= 5f) {
                    float projOffset = 24;
                    Vector2 projPos = new Vector2((float)(enemies[i].Position.X + projOffset * Math.Sin(enemies[i].Angle)),
                                                  (float)(enemies[i].Position.Y + projOffset * -Math.Cos(enemies[i].Angle)));
                    Vector2 projDir = new Vector2((float)Math.Sin(enemies[i].Angle),
                                                  (float)-Math.Cos(enemies[i].Angle));
                    Projectile proj = new Projectile(Content, projPos, projDir, enemies[i].Damage);
                    enemyProjectiles.Add(proj);
                    enemies[i].ShootDelay = 0;
                }

                // Collision between two enemies
                for (int j = i + 1; j < enemies.Length; j++) {
                    CollisionHelper.handleElasticCollision(enemies[i], enemies[j]);
                }

                // Collision between an enemy and a ship
                if (CollisionHelper.handleElasticCollision(enemies[i], ship)) {
                    collisionSound.Play();
                }

                // Collision between an enemy and a projectile
                for (int j = 0; j < projectiles.Count; j++) {
                    if (CollisionHelper.Collides(enemies[i].Bounds, projectiles[j].Bounds)) {
                        // Particle
                        enemies[i].Health -= projectiles[j].Damage;
                        if (enemies[i].Health <= 0) {
                            explosionParticles.AddExplosion(enemies[i].Bounds.Center);
                            enemies[i].Despawn();
                        }

                        projectiles.RemoveAt(j);
                    }
                }


                // Keep enemies on the map
                if (enemies[i].EnteredBounds)
                    KeepShipInBounds(enemies[i]);

            }

            // Collision between the player and enemy projectile
            for (int j = 0; j < enemyProjectiles.Count; j++) {
                if (CollisionHelper.Collides(ship.Bounds, enemyProjectiles[j].Bounds)) {
                    // Particle
                    ship.Health -= enemyProjectiles[j].Damage;
                    if (ship.Health <= 0) {
                        explosionParticles.AddExplosion(ship.Bounds.Center);
                        game.ChangeScene(new TitleScene(game));
                    }

                    enemyProjectiles.RemoveAt(j);
                }
            }

            // Keep player ship on the map
            KeepShipInBounds(ship);

            // Player Shoot
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && priorKeyboardState.IsKeyUp(Keys.Space) && game.Player.Ammo > 0) {
                float projOffset = 24;
                Vector2 projPos = new Vector2((float)(ship.Position.X + projOffset * Math.Sin(ship.Angle)),
                                              (float)(ship.Position.Y + projOffset * -Math.Cos(ship.Angle)));
                Vector2 projDir = new Vector2((float)Math.Sin(ship.Angle),
                                              (float)-Math.Cos(ship.Angle));
                Projectile proj = new Projectile(Content, projPos, projDir, ship.Damage);
                projectiles.Add(proj);
                game.Player.Ammo--;
            }

            priorKeyboardState = Keyboard.GetState();
        }

        private void KeepShipInBounds(Ship ship) {
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

            // Draw enemies
            foreach (var enemy in enemies) {
                enemy.Draw(gameTime, spriteBatch);
            }

            // Draw ship
            ship.Draw(gameTime, spriteBatch);

            // Update projectile texture
            foreach (var p in projectiles) p.Draw(gameTime, spriteBatch);
            foreach (var p in enemyProjectiles) p.Draw(gameTime, spriteBatch);

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
            // Draw minimap
            drawMinimap(spriteBatch, playerPos);

            // Draw map
            if(showMap) {
                drawMap(gameTime, spriteBatch);
            }


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

        private void drawMinimap (SpriteBatch spriteBatch, Vector2 viewportCenter) {
            float mapScale;
            if (Width >= Height) {
                mapScale = 1/7f * Starforged.gDevice.Viewport.Width / Width;
            } else {
                mapScale = 1/4f * Starforged.gDevice.Viewport.Height / Height;
            }
            int mapWidth = (int)(mapScale * Width);
            int mapHeight = (int)(mapScale * Height);

            // Draw map text
            var mapTextScale = 0.6f;
            var mapText = "Press M to open map";
            var mapTextPos = new Vector2(Starforged.gDevice.Viewport.Width - mapWidth - textFont.MeasureString(mapText).X*mapTextScale,
                                         game.GraphicsDevice.Viewport.Height - textFont.LineSpacing*mapTextScale - 2);
            spriteBatch.DrawString(textFont, "Press M to open map", mapTextPos, Color.White, 0f, new Vector2(0, 0), mapTextScale, SpriteEffects.None, 0);

            // Draw map background
            background.DrawMapBackground(spriteBatch, mapScale);

            // Draw minimap border
            for (int i = 0; i < 3; i++) {
                spriteBatch.Draw(viewportRectangle,
                                new Rectangle(Starforged.gDevice.Viewport.Width - mapWidth + i,
                                              Starforged.gDevice.Viewport.Height - mapHeight + i,
                                              mapWidth - 2*i,
                                              mapHeight - 2*i),
                                new Rectangle(0, 0, viewportRectangle.Width, viewportRectangle.Height),
                                Color.DarkGray);
            }


            // Draw viewport rectangle
            spriteBatch.Draw(viewportRectangle,
                            new Rectangle((int)(Starforged.gDevice.Viewport.Width - mapWidth + viewportCenter.X * mapScale),
                                          (int)(Starforged.gDevice.Viewport.Height - mapHeight + viewportCenter.Y * mapScale),
                                          (int)(Starforged.gDevice.Viewport.Width * mapScale),
                                          (int)(Starforged.gDevice.Viewport.Height * mapScale)),
                            new Rectangle(0, 0, viewportRectangle.Width, viewportRectangle.Height),
                            Color.White,
                            0f,
                            new Vector2(viewportRectangle.Width / 2, viewportRectangle.Height / 2),
                            SpriteEffects.None,
                            1);

            // Draw player icon
            spriteBatch.Draw(playerIcon,
                             new Vector2((int)(Starforged.gDevice.Viewport.Width - mapWidth + ship.Position.X * mapScale),
                                         (int)(Starforged.gDevice.Viewport.Height - mapHeight + ship.Position.Y * mapScale)),
                             new Rectangle(0,0, playerIcon.Width, playerIcon.Height),
                             Color.White,
                             ship.Angle,
                             new Vector2(playerIcon.Width/2,playerIcon.Height/2),
                             1f,
                             SpriteEffects.None,
                             1);

        }


        private void drawMap (GameTime gameTime, SpriteBatch spriteBatch) {

            // Draw map background
            mapBackground.Draw(spriteBatch);

            // Draw map border
            for (int i = 0; i < 3; i++) {
                spriteBatch.Draw(viewportRectangle,
                                new Rectangle(mapPadding,
                                              mapPadding,
                                              Starforged.gDevice.Viewport.Width - 2*mapPadding,
                                              Starforged.gDevice.Viewport.Height - 2*mapPadding),
                                Color.DarkGray);
            }

            // Draw map objects
            map.Draw(gameTime, spriteBatch, mapPadding, mapPadding);

        }
    }
}
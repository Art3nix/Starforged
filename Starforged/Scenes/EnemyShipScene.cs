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
    public class EnemyShipScene : Scene {

        // Background
        private Background background;
        private Texture2D splash;

        // Fonts
        private SpriteFont textFont;

        // Ships
        private PlayerShip ship;

        // Enemies
        private EnemyShip[] enemies;
        private int enemiesLeft;

        // Items
        private List<Item> items = new List<Item>();

        // Ship projectiles
        private List<Projectile> projectiles = new List<Projectile>();
        private List<Projectile> enemyProjectiles = new List<Projectile>();

        // Collision sound
        private SoundEffect collisionSound;

        private KeyboardState priorKeyboardState;

        private Hud hud;
        private Texture2D viewportRectangle;
        private Texture2D playerIcon;
        private Texture2D red;
        private Texture2D green;

        private ExplosionParticleSystem explosionParticles;

        private int enemyCount;
        private bool clearedLevel = false;
        private String endText = "";

        /// <summary>
        /// True if map is shown
        /// </summary>
        bool showMap = false;
        private ImportedMap map;
        private Background mapBackground;
        private int mapPadding = 60;

        private SceneState prevState;
        private bool fullyTransitioned = false;
        private int introTransitionTimeOn = 1;
        private int introTransitionTimeDisplay = 2;
        private int introTransitionTimeOff = 2;
        private float introTransitionElapsed = 0;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public EnemyShipScene(Starforged g, int enemyCount) : base(g) {
            WindowWidth = 1800;
            WindowHeight = 1000;

            this.enemyCount = enemyCount;
            map = g.Map;

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

            // Initialize map background
            mapBackground = new TiledBackground(WindowWidth - 2 * mapPadding,
                                                WindowHeight - 2 * mapPadding,
                                                mapPadding,
                                                mapPadding);

            prevState = State;

            // Transition times
            timeTransitionOn = 2;
            timeTransitionOff = 4;

            base.Initialize();


            // Initialize map to respond to this screen size
            map.Initialize(game, mapPadding, mapPadding);


            // Initialize ships
            ship = new PlayerShip(game, Content, "ships/ship1");

            // Initialize enemies
            enemiesLeft = enemyCount;
            enemies = new EnemyShip[enemiesLeft];
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
            red = Content.Load<Texture2D>("utils/red");
            green = Content.Load<Texture2D>("utils/green");

            // Load map content
            map.LoadContent(Content);
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

            // Scene has fully transitioned - Start game
            if (prevState == SceneState.TransitionOn && State == SceneState.Active) {
                fullyTransitioned = true;
            }

            // Show/hide the map
            if (Keyboard.GetState().IsKeyDown(Keys.M) && priorKeyboardState.IsKeyUp(Keys.M)) {
                showMap = !showMap;
            }

            if (showMap) {
                // don't update the scene if the map is displayed
                map.Update(game);
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

                // Skip despawned enemies
                if (enemies[i].Health <= 0) continue;


                enemies[i].Update(gameTime);


                if (enemies[i].TargetInRange() && enemies[i].ShootDelay >= 5f) {
                    float projOffset = 24;
                    Vector2 projPos = new Vector2((float)(enemies[i].Position.X + projOffset * Math.Sin(enemies[i].Angle)),
                                                  (float)(enemies[i].Position.Y + projOffset * -Math.Cos(enemies[i].Angle)));
                    Vector2 projDir = new Vector2((float)Math.Sin(enemies[i].Angle),
                                                  (float)-Math.Cos(enemies[i].Angle));
                    Projectile proj = new Projectile(Content, projPos, projDir, enemies[i].Damage, enemies[i].ProjectileSpeed);
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
                            items.Add(Item.Create(Content, enemies[i].Bounds.Center,
                                            new float[] { 0.15f, 0.1f, 0.15f, 0.2f, 0.25f },
                                            new int[] { 20, 10, 20, 20, 25 }));

                            enemies[i].Despawn();
                            enemiesLeft--;
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
                    if (!clearedLevel)
                        ship.Health -= enemyProjectiles[j].Damage;  // When the level is done do not damage the ship

                    if (ship.Health <= 0) {
                        explosionParticles.AddExplosion(ship.Bounds.Center);
                        game.ChangeScene(new TitleScene(game));
                    }

                    enemyProjectiles.RemoveAt(j);
                }
            }

            // Collision between the player and an item
            for (int i = 0; i < items.Count; i++) {
                if (CollisionHelper.Collides(items[i].Bounds, ship.Bounds)) {
                    items[i].Add(game.Player);
                    items.RemoveAt(i);
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
                Projectile proj = new Projectile(Content, projPos, projDir, ship.Damage, ship.ProjectileSpeed);
                projectiles.Add(proj);
                game.Player.Ammo--;
            }

            if (fullyTransitioned)
                introTransitionElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            priorKeyboardState = Keyboard.GetState();
            prevState = State;
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

            // Draw items
            foreach (var i in items) i.Draw(gameTime, spriteBatch);

            // Draw enemies
            foreach (var enemy in enemies) {
                enemy.Draw(gameTime, spriteBatch);
                if (enemy.Health < enemy.MaxHealth) {
                    drawHealth(enemy, spriteBatch);
                }
            }

            // Draw ship
            ship.Draw(gameTime, spriteBatch);
            if (ship.Health < ship.MaxHealth) {
                drawHealth(ship, spriteBatch);
            }

            // Update projectile texture
            foreach (var p in projectiles) p.Draw(gameTime, spriteBatch);
            foreach (var p in enemyProjectiles) p.Draw(gameTime, spriteBatch);

            spriteBatch.End();


            // Static objects batch
            spriteBatch.Begin();

            // Enemies left text
            var enemiesScale = 0.8f;
            var enemiesText = "Enemies left: ";
            var enemiesPos = new Vector2(10, 10);
            spriteBatch.DrawString(textFont, enemiesText, enemiesPos, Color.White, 0f, new Vector2(0, 0), enemiesScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, enemiesLeft.ToString(), enemiesPos + new Vector2(textFont.MeasureString(enemiesText).X * enemiesScale, 0), Color.White, 0f, new Vector2(0, 0), enemiesScale, SpriteEffects.None, 0);



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
            if (showMap) {
                drawMap(gameTime, spriteBatch);
            }

            // Scene fully transitioned - Start game
            if (fullyTransitioned) {
                // Draw fade in/out introduction text
                float alpha = (float)Math.Pow(introTransitionElapsed / introTransitionTimeOn, 2);
                if (introTransitionElapsed > (introTransitionTimeOn + introTransitionTimeDisplay)) {
                    var elapsedOff = introTransitionElapsed - (introTransitionTimeDisplay + introTransitionTimeOn);
                    alpha = 1 - (float)Math.Pow(elapsedOff / introTransitionTimeOff, 2);
                }
                var introScale = 1.5f;
                var introText = "Destroy all enemies";
                spriteBatch.DrawString(textFont, introText, screenCenter, Color.White * alpha, 0f, textFont.MeasureString(introText) / 2, introScale, SpriteEffects.None, 0);

            }

            // End game
            if (!clearedLevel && (enemiesLeft == 0 || ship.Health <= 0)) {
                clearedLevel = true;
                if (ship.Health > 0) {
                    if (game.Map.CurrentLocation == game.Map.PlanetCount - 1) {
                        // Final level
                        endText = "Victory";
                    } else {
                        endText = "Level cleared";
                        if (game.Map.CurrentLocation == game.Player.Level) {
                            // Unlock next level
                            game.Player.Level++;
                        }
                    }
                } else {
                    endText = "Game Over";
                    game.ChangeScene(new TitleScene(game));
                }
            }
            
            if (clearedLevel) {
                var endScale = 2f;
                spriteBatch.DrawString(textFont, endText, screenCenter, Color.White, 0f, textFont.MeasureString(endText) / 2, endScale, SpriteEffects.None, 0);

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

        private void drawHealth(Ship ship, SpriteBatch spriteBatch) {
            float healthPercentage = ship.Health / ship.MaxHealth;
            var overlap = 5;
            var barLength = ship.SIZE + overlap*2;
            var barHeight = 8;

            Rectangle posRed = new Rectangle((int)ship.Position.X - ship.SIZE/2, (int)ship.Position.Y + ship.SIZE, barLength, barHeight);
            Rectangle posGreen = new Rectangle((int)ship.Position.X - ship.SIZE / 2, (int)ship.Position.Y + ship.SIZE, (int)(barLength*healthPercentage), barHeight);
            spriteBatch.Draw(red, posRed, Color.Red);
            spriteBatch.Draw(green, posGreen, Color.Green);

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
            map.Draw(gameTime, spriteBatch, game);

        }
    }
}
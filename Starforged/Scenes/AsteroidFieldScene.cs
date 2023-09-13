using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;

namespace Starforged {
    public class AsteroidFieldScene : Scene {

        // Background
        private Background background;

        // Ships
        private PlayerShip ship;

        // Asteroids
        private Asteroid[] asteroids;

        private Texture2D ball;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public AsteroidFieldScene(Starforged g) : base(g) {
            game.gGraphicsMgr.PreferredBackBufferWidth = 500;
            game.gGraphicsMgr.PreferredBackBufferHeight = 500;
            game.gGraphicsMgr.ApplyChanges();
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        public override void Initialize() {

            // Initialize background
            background = new TiledBackground(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            // Initialize ships
            ship = new PlayerShip();

            // Initialize asteroids
            Random r = new Random();
            asteroids = new Asteroid[12];
            for (var i = 0; i < asteroids.Length; i++) {
                asteroids[i] = new Asteroid(r.Next(4), r.Next(2));
            }

            base.Initialize();

        }

        /// <summary>
        /// Loads game content
        /// </summary>
        public override void LoadContent() {

            base.LoadContent();

            // Load background
            background.LoadContent(Content, "background/space_tile");

            // Load ship
            ship.LoadContent(Content, "ships/ship1");

            // Load asteroids
            foreach (var asteroid in asteroids) asteroid.LoadContent(Content);


            ball = Content.Load<Texture2D>("asteroids/ball");


        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        public override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();

            // Update ship
            ship.Update(gameTime);

            // Update asteroids
            for (int i = 0; i < asteroids.Length; i++) {
                Asteroid asteroid = asteroids[i];
                asteroid.Update(gameTime);

                // Collision between two asteroids
                for (int j = i + 1; j < asteroids.Length; j++) {
                    Asteroid otherAsteroid = asteroids[j];
                    // Check for overlapping
                    if (asteroid.Bounds.CollidesWith(otherAsteroid.Bounds)) {
                        Vector2 collisionVelocity = asteroid.Direction - otherAsteroid.Direction;
                        var collisionAxis = otherAsteroid.Bounds.Center - asteroid.Bounds.Center;

                        // Check for collision
                        if (Vector2.Dot(collisionAxis, collisionVelocity) >= 0) {
                            var m0 = asteroid.Mass;
                            var m1 = otherAsteroid.Mass;

                            float angle = (float)-Math.Atan2(otherAsteroid.Bounds.Center.Y - asteroid.Bounds.Center.Y,
                                                             otherAsteroid.Bounds.Center.X - asteroid.Bounds.Center.X);
                            
                            Vector2 u0 = Vector2.Transform(asteroid.Direction, Matrix.CreateRotationZ(angle));
                            Vector2 u1 = Vector2.Transform(otherAsteroid.Direction, Matrix.CreateRotationZ(angle));

                            Vector2 v0, v1;
                            v0 = new Vector2(u0.X * (m0 - m1) / (m0 + m1) + u1.X * 2 * m1 / (m0 + m1), u0.Y);
                            v1 = new Vector2(u1.X * (m1 - m0) / (m0 + m1) + u0.X * 2 * m0 / (m0 + m1), u1.Y);

                            asteroid.Direction = Vector2.Transform(v0, Matrix.CreateRotationZ(-angle));
                            otherAsteroid.Direction = Vector2.Transform(v1, Matrix.CreateRotationZ(-angle));
                        }


                    }
                }

                // Collision between an asteroid and a ship
                if (asteroid.Bounds.CollidesWith(ship.Bounds) ) {
                    
                }
            }
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            // Draw background
            background.Draw(spriteBatch);

            // Draw asteroids
            foreach (var asteroid in asteroids) {
                asteroid.Draw(gameTime, spriteBatch);


                var rect = new Rectangle((int)(asteroid.Bounds.Center.X - asteroid.Bounds.Radius),
                                         (int)(asteroid.Bounds.Center.Y - asteroid.Bounds.Radius),
                                         (int)(2* asteroid.Bounds.Radius), (int)(2* asteroid.Bounds.Radius));
                spriteBatch.Draw(ball, rect, Color.White);
            }

            var rectG = new Rectangle((int)(ship.Bounds.Center.X - ship.Bounds.Radius),
                                     (int)(ship.Bounds.Center.Y - ship.Bounds.Radius),
                                     (int)(2 * ship.Bounds.Radius), (int)(2 * ship.Bounds.Radius));
            spriteBatch.Draw(ball, rectG, Color.White);


            // Draw ship
            ship.Draw(gameTime, spriteBatch);

        }
    }
}
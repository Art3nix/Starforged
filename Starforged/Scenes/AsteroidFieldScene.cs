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


        /// <summary>
        /// Constructs the game
        /// </summary>
        public AsteroidFieldScene(Starforged g) : base(g) {
            game.gGraphicsMgr.PreferredBackBufferWidth = 1800;
            game.gGraphicsMgr.PreferredBackBufferHeight = 1000;
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
            asteroids = new Asteroid[40];
            for (var i = 0; i < asteroids.Length; i++) {
                asteroids[i] = new Asteroid(r.Next(4), r.Next(3));
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
                asteroids[i].Update(gameTime);

                // Collision between two asteroids
                for (int j = i + 1; j < asteroids.Length; j++) {
                    CollisionHelper.handleElasticCollision(asteroids[i], asteroids[j]);                   
                }

                // Collision between an asteroid and a ship
                CollisionHelper.handleElasticCollision(asteroids[i], ship);
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
            }

            // Draw ship
            ship.Draw(gameTime, spriteBatch);

        }
    }
}
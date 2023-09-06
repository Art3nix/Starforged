using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Starforged
{
    public class Starforged : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Background
        private Background background;

        // Fonts
        private SpriteFont titleFont;
        private SpriteFont textFont;

        // Ships
        private Ship[] ships;

        // Asteroids
        private Asteroid[] asteroids;

        /// <summary>
        /// Get size of the window
        /// </summary>
        /// <returns>size of the window</returns>
        public static GraphicsDevice gDevice;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public Starforged() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();
            gDevice = GraphicsDevice;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize() {

            // Initialize background
            background = new TiledBackground(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Initialize ships
            ships = new Ship[] {
                new Ship(),
                new Ship(),
                new Ship(),
                new Ship()
            };

            // Initialize asteroids
            Random r = new Random();
            asteroids = new Asteroid[12];
            for(var i = 0; i < asteroids.Length; i++) {
                asteroids[i] = new Asteroid(r.Next(4));
            }

            base.Initialize();

        }

        /// <summary>
        /// Loads game content
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load background
            background.LoadContent(Content, "background/space_tile");

            // Load font
            titleFont = Content.Load<SpriteFont>("title");
            textFont = Content.Load<SpriteFont>("millennia");

            // Load ships
            foreach (var ship in ships) ship.LoadContent(Content);

            // Load asteroids
            foreach (var asteroid in asteroids) asteroid.LoadContent(Content);

        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update ships
            foreach (var ship in ships) ship.Update(gameTime);

            // Update asteroids
            foreach (var asteroid in asteroids) asteroid.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Draw background
            background.Draw(spriteBatch);

            // Draw asteroids
            foreach (var asteroid in asteroids) asteroid.Draw(gameTime, spriteBatch);

            // Draw ships
            foreach (var ship in ships) ship.Draw(gameTime, spriteBatch);


            //Draw text
            var title = "Starforged";
            var screenCenter = new Vector2(GraphicsDevice.Viewport.Bounds.Width / 2,
                                           GraphicsDevice.Viewport.Bounds.Height / 2);
            spriteBatch.DrawString(titleFont, title, new Vector2(screenCenter.X, 100), Color.White, 0f, titleFont.MeasureString(title) / 2, 1f, SpriteEffects.None, 0);

            var exitText = "Press Esc to exit the game";
            spriteBatch.DrawString(textFont, exitText, screenCenter, Color.White, 0f, textFont.MeasureString(exitText) / 2, 1f, SpriteEffects.None, 0);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Starforged {
    public class TitleScene : Scene {

        // Background
        private Background background;

        // Fonts
        private SpriteFont titleFont;
        private SpriteFont textFont;

        // Ships
        private TitleScreenShip[] ships;

        // Asteroids
        private Asteroid[] asteroids;

        /// <summary>
        /// Constructs the game
        /// </summary>
        public TitleScene(Starforged g) : base(g) {
            game.gGraphicsMgr.PreferredBackBufferWidth = 700;
            game.gGraphicsMgr.PreferredBackBufferHeight = 700;
            game.gGraphicsMgr.ApplyChanges();
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        public override void Initialize() {

            // Initialize background
            background = new TiledBackground(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            // Initialize ships
            ships = new TitleScreenShip[] {
                    new TitleScreenShip(),
                    new TitleScreenShip(),
                    new TitleScreenShip(),
                    new TitleScreenShip(),
            };

            // Initialize asteroids
            Random r = new Random();
            asteroids = new Asteroid[12];
            for (var i = 0; i < asteroids.Length; i++) {
                asteroids[i] = new Asteroid(r.Next(4));
            }

            base.Initialize();

        }

        /// <summary>
        /// Loads game content
        /// </summary>
        public override void LoadContent() {

            base.LoadContent();

            // Load background
            background.LoadContent(Content, "background/space");

            // Load font
            titleFont = Content.Load<SpriteFont>("title");
            textFont = Content.Load<SpriteFont>("millennia");

            // Load ships
            foreach (var ship in ships) ship.LoadContent(Content, "ships/ship1");

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


            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                game.ChangeScene(new AsteroidFieldScene(game));
            }

            // Update ships
            foreach (var ship in ships) ship.Update(gameTime);

            // Update asteroids
            foreach (var asteroid in asteroids) asteroid.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            // Draw background
            background.Draw(spriteBatch);

            // Draw asteroids
            foreach (var asteroid in asteroids) asteroid.Draw(gameTime, spriteBatch);
            
            // Draw ships
            foreach (var ship in ships) ship.Draw(gameTime, spriteBatch);

            //Draw text
            var title = "Starforged";
            var screenCenter = new Vector2(game.GraphicsDevice.Viewport.Bounds.Width / 2,
                                           game.GraphicsDevice.Viewport.Bounds.Height / 2);
            spriteBatch.DrawString(titleFont, title, new Vector2(screenCenter.X, 100), Color.White, 0f, titleFont.MeasureString(title) / 2, 1f, SpriteEffects.None, 0);

            var exitText = "Press Enter to join the universe";
            spriteBatch.DrawString(textFont, exitText, screenCenter, Color.White, 0f, textFont.MeasureString(exitText) / 2, 1f, SpriteEffects.None, 0);
        }
    }
}
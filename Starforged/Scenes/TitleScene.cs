using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1.Effects;

namespace Starforged {
    public class TitleScene : Scene {

        // Background
        private Background background;
        private Texture2D splash;

        // Fonts
        private SpriteFont titleFont;
        private SpriteFont textFont;

        // Ships
        private TitleScreenShip[] ships;

        // Asteroids
        private Asteroid[] asteroids;

        // Music
        private Song backgroundMusic;

        private Portal crate;


        private float textScale = 0.9f;
        private float growTextIncrement = 0.25f;
        private float minTextScale = 0.9f;
        private float maxTextScale = 1.15f;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public TitleScene(Starforged g) : base(g) {
            game.gGraphicsMgr.PreferredBackBufferWidth = 700;
            game.gGraphicsMgr.PreferredBackBufferHeight = 700;

            Width = 700;
            Height = 700;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        public override void Initialize() {

            // Initialize background
            background = new TiledBackground(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            // Initialize ships
            ships = new TitleScreenShip[] {
                    new TitleScreenShip(game),
                    new TitleScreenShip(game),
                    new TitleScreenShip(game),
                    new TitleScreenShip(game),
            };

            // Initialize asteroids
            Random r = new Random();
            asteroids = new Asteroid[8];
            for (var i = 0; i < asteroids.Length; i++) {
                asteroids[i] = new Asteroid(game, r.Next(4), r.Next(3));
            }

            crate = new Portal(game, Matrix.Identity);

            // Transition times
            timeTransitionOn = 2;
            timeTransitionOff = 4;

            base.Initialize();

        }

        /// <summary>
        /// Loads game content
        /// </summary>
        public override void LoadContent() {

            base.LoadContent();

            // Load background
            background.LoadContent(Content, "background/space");
            splash = Content.Load<Texture2D>("background/black_splash");

            // Load font
            titleFont = Content.Load<SpriteFont>("title");
            textFont = Content.Load<SpriteFont>("millennia");

            // Load ships
            foreach (var ship in ships) ship.LoadContent(Content, "ships/ship1");

            // Load asteroids
            foreach (var asteroid in asteroids) asteroid.LoadContent(Content);

            // Load Music
            backgroundMusic = Content.Load<Song>("music/title_music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);


        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.Exit();


            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                game.ChangeScene(new EnemyShipScene(game));
            }

            // Update ships
            foreach (var ship in ships) ship.Update(gameTime);

            // Update asteroids
            foreach (var asteroid in asteroids) asteroid.Update(gameTime);

            //Grow and shrink text
            if ((growTextIncrement > 0 && textScale >= maxTextScale) ||
                growTextIncrement < 0 && textScale <= minTextScale) {
                growTextIncrement = -growTextIncrement;
            }
            textScale += (float)(growTextIncrement * gameTime.ElapsedGameTime.TotalSeconds);

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
            var title = "Starf  rged";
            var screenCenter = new Vector2(game.GraphicsDevice.Viewport.Bounds.Width / 2,
                                           game.GraphicsDevice.Viewport.Bounds.Height / 2);
            spriteBatch.DrawString(titleFont, title, new Vector2(screenCenter.X, 100), Color.White, 0f, titleFont.MeasureString(title) / 2, 1f, SpriteEffects.None, 0);

            var exitText = "Press Enter to join the universe";
            spriteBatch.DrawString(textFont, exitText, screenCenter, Color.White, 0f, textFont.MeasureString(exitText) / 2, textScale, SpriteEffects.None, 0);

            spriteBatch.End();

            crate.Draw();

            spriteBatch.Begin();

            // Fade out transition
            if(State == SceneState.TransitionOff) {
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
            if (transitionTimeElapsed > timeTransitionOff) {
                State = SceneState.Inactive;
                MediaPlayer.Stop();
            } else {
                transitionTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                MediaPlayer.Volume = 1 - (float)Math.Pow(transitionTimeElapsed / timeTransitionOff, 2);
            }
        }
    }
}
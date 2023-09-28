﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;

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

        // Collision sound
        private SoundEffect collisionSound;

        private KeyboardState priorKeyboardState;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public AsteroidFieldScene(Starforged g) : base(g) {
            game.gGraphicsMgr.PreferredBackBufferWidth = 1800;
            game.gGraphicsMgr.PreferredBackBufferHeight = 1000;
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        public override void Initialize() {

            // Initialize background
            background = new TiledBackground(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);

            // Initialize asteroids
            Random r = new Random();
            asteroids = new Asteroid[40];
            for (var i = 0; i < asteroids.Length; i++) {
                asteroids[i] = new Asteroid(r.Next(4), r.Next(3));
            }

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
            }

            priorKeyboardState = Keyboard.GetState();
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

            // Draw text
            var dampersScale = 0.8f;
            var dampersText = "Dampers: ";
            var dampersStatus = ship.InertiaDampers ? "On" : "Off";
            var dampersColor = ship.InertiaDampers ? Color.LimeGreen : Color.Red;
            var dampersPos = new Vector2(10, game.GraphicsDevice.Viewport.Height - textFont.LineSpacing);
            spriteBatch.DrawString(textFont, dampersText, dampersPos, Color.White, 0f, new Vector2(0,0), dampersScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, dampersStatus, dampersPos + new Vector2(textFont.MeasureString(dampersText).X * dampersScale, 0), dampersColor, 0f, new Vector2(0, 0), dampersScale, SpriteEffects.None, 0);


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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SharpDX.Win32;

namespace Starforged {
    public class Hud {

        private Starforged game;

        private Texture2D resourcesBackground;

        private Texture2D fuelIcon;

        private Texture2D jumpFuelIcon;

        private Texture2D componentsIcon;

        private Texture2D creditsIcon;

        private Texture2D ammoIcon;

        // Fonts
        private SpriteFont textFont;

        public Hud(Starforged g) {
            game = g;
        }

        /// <summary>
        /// Method to load the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="textureName">The name of the texture to load</param>
        public void LoadContent(ContentManager content) {
            resourcesBackground = content.Load<Texture2D>("icons/resourcesBackground");
            creditsIcon = content.Load<Texture2D>("icons/credits");
            fuelIcon = jumpFuelIcon = componentsIcon = ammoIcon = creditsIcon;

            // Load font
            textFont = content.Load<SpriteFont>("millennia");
        }

        /// <summary>
        /// Method to update the ship
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {

        }


        /// <summary>
        /// Method to draw the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            var iconSize = 32;
            var textOffset = 12;
            var iconOffset = 24;
            var padding = 16;
            var maxTextLen = 48;
            var bgWidth = 5 * (iconSize + textOffset + maxTextLen) + 4 * iconOffset + 2 * padding;
            
            var bgRectangle = new Rectangle(Starforged.gDevice.Viewport.Width / 2, 0, bgWidth, resourcesBackground.Height);
            spriteBatch.Draw(resourcesBackground, bgRectangle, new Rectangle(0,0,resourcesBackground.Width, resourcesBackground.Height), Color.White, 0f, new Vector2(resourcesBackground.Width / 2, 0), SpriteEffects.None, 0);

            //TODO create method for this
            Vector2 pos = new Vector2(bgRectangle.X - bgWidth / 2, padding);
            spriteBatch.Draw(fuelIcon, new Rectangle((int)pos.X, (int)pos.Y, iconSize, iconSize), Color.White);
            pos.X += textOffset + iconSize;
            spriteBatch.DrawString(textFont, game.Player.Fuel.ToString(), pos, Color.White);
            pos.X += iconOffset + maxTextLen;

            spriteBatch.Draw(jumpFuelIcon, new Rectangle((int)pos.X, (int)pos.Y, iconSize, iconSize), Color.White);
            pos.X += textOffset + iconSize;
            spriteBatch.DrawString(textFont, game.Player.JumpFuel.ToString(), pos, Color.White);
            pos.X += iconOffset + maxTextLen;

            spriteBatch.Draw(componentsIcon, new Rectangle((int)pos.X, (int)pos.Y, iconSize, iconSize), Color.White);
            pos.X += textOffset + iconSize;
            spriteBatch.DrawString(textFont, game.Player.Components.ToString(), pos, Color.White);
            pos.X += iconOffset + maxTextLen;

            spriteBatch.Draw(creditsIcon, new Rectangle((int)pos.X, (int)pos.Y, iconSize, iconSize), Color.White);
            pos.X += textOffset + iconSize;
            spriteBatch.DrawString(textFont, game.Player.Credits.ToString(), pos, Color.White);
            pos.X += iconOffset + maxTextLen;

            spriteBatch.Draw(ammoIcon, new Rectangle((int)pos.X, (int)pos.Y, iconSize, iconSize), Color.White);
            pos.X += textOffset + iconSize;
            spriteBatch.DrawString(textFont, game.Player.Ammo.ToString(), pos, Color.White);
            pos.X += iconOffset + maxTextLen;



        }
    }
}

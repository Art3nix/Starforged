using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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

        // Constants
        private const int iconSize = 32;
        private const int textOffset = 12;
        private const int iconOffset = 24;
        private const int padding = 16;
        private const int maxTextLen = 48;
        private const int bgWidth = 5 * (iconSize + textOffset + maxTextLen) + 4 * iconOffset + 2 * padding;

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
            fuelIcon = content.Load<Texture2D>("icons/fuel"); 
            jumpFuelIcon = content.Load<Texture2D>("icons/jumpfuel"); 
            componentsIcon = content.Load<Texture2D>("icons/component"); 
            ammoIcon = content.Load<Texture2D>("icons/ammo"); 

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
            
            var bgRectangle = new Rectangle(Starforged.gDevice.Viewport.Width / 2, 0, bgWidth, resourcesBackground.Height);
            spriteBatch.Draw(resourcesBackground, bgRectangle, new Rectangle(0,0,resourcesBackground.Width, resourcesBackground.Height), Color.White, 0f, new Vector2(resourcesBackground.Width / 2, 0), SpriteEffects.None, 0);

            Vector2 pos = new Vector2(bgRectangle.X - bgWidth / 2 + padding, padding);
            var fuel = game.Player.Fuel > 20f ? game.Player.Fuel.ToString("0") : game.Player.Fuel.ToString("0.0");
            printResource(spriteBatch, fuelIcon, ref pos, fuel);
            printResource(spriteBatch, jumpFuelIcon, ref pos, game.Player.JumpFuel.ToString());
            printResource(spriteBatch, componentsIcon, ref pos, game.Player.Components.ToString());
            printResource(spriteBatch, creditsIcon, ref pos, game.Player.Credits.ToString());
            printResource(spriteBatch, ammoIcon, ref pos, game.Player.Ammo.ToString());



        }

        private void printResource(SpriteBatch spriteBatch, Texture2D icon, ref Vector2 pos, string amount) {
            spriteBatch.Draw(icon, new Rectangle((int)pos.X, (int)pos.Y, iconSize, iconSize), Color.White);
            pos.X += textOffset + iconSize;
            spriteBatch.DrawString(textFont, amount, pos, Color.White);
            pos.X += iconOffset + maxTextLen;

        }
    }
}

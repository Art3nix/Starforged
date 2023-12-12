using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Starforged {
    public class Shop {

        private Starforged game;

        private Texture2D shopBackground;
        private Texture2D border;
        private Texture2D white;

        private Texture2D fuelIcon;
        private Texture2D jumpFuelIcon;
        private Texture2D componentsIcon;
        private Texture2D creditsIcon;
        private Texture2D ammoIcon;

        // Fonts
        private SpriteFont titleFont;
        private SpriteFont textFont;

        // Constants
        private int maxWidth = 1300;
        private int maxHeight = 700;
        private int offsetW = 100;
        private int offsetH = 100;
        private int spacing = 20;
        private int padding = 30;
        private int shopItemIconSize;
        private int buttonW;

        private Rectangle background;

        private ContentManager content;

        private Button[] buttons;
        private int purchaseAmount;


        public Shop(Starforged g) {
            game = g;

            var bgWidth = Starforged.gDevice.Viewport.Width - 2 * offsetW;
            var bgHeight = Starforged.gDevice.Viewport.Height - 2 * offsetH;
            bgWidth = bgWidth < maxWidth ? bgWidth : maxWidth;
            bgHeight = bgHeight < maxHeight ? bgHeight : maxHeight;

            background = new Rectangle((Starforged.gDevice.Viewport.Width - bgWidth) / 2,
                                       (Starforged.gDevice.Viewport.Height - bgHeight) / 2,
                                       bgWidth,
                                       bgHeight);
            shopItemIconSize = background.Width / 16;
            buttonW = background.Width / 12;

            buttons = new Button[10];
            purchaseAmount = 1;
        }

        /// <summary>
        /// Method to load the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="textureName">The name of the texture to load</param>
        public void LoadContent(ContentManager content) {
            this.content = content;


            // Background
            shopBackground = content.Load<Texture2D>("background/shopBackground");
            border = content.Load<Texture2D>("rectangle");
            white = content.Load<Texture2D>("utils/white");

            creditsIcon = content.Load<Texture2D>("icons/credits");
            fuelIcon = content.Load<Texture2D>("icons/fuel"); 
            jumpFuelIcon = content.Load<Texture2D>("icons/jumpfuel"); 
            componentsIcon = content.Load<Texture2D>("icons/component"); 
            ammoIcon = content.Load<Texture2D>("icons/ammo");

            // Buttons
            buttons[4] = new AmountButton(content);
            buttons[0] = new ShopButton(creditsIcon, 1, ItemType.Fuel, ItemType.Credits, content, game, (AmountButton)buttons[4]);
            buttons[1] = new ShopButton(creditsIcon, 5, ItemType.JumpFuel, ItemType.Credits, content, game, (AmountButton)buttons[4]);
            buttons[2] = new ShopButton(creditsIcon, 10, ItemType.Components, ItemType.Credits, content, game, (AmountButton)buttons[4]);
            buttons[3] = new ShopButton(creditsIcon, 2, ItemType.Ammo, ItemType.Credits, content, game, (AmountButton)buttons[4]);
            buttons[5] = new UpgradeButton(componentsIcon, 10 * (game.Player.ShipUpgradeLevels[UpgradeType.Health] + 1), UpgradeType.Health, ItemType.Components, content, game);
            buttons[6] = new UpgradeButton(componentsIcon, 10 * (game.Player.ShipUpgradeLevels[UpgradeType.Speed] + 1), UpgradeType.Speed, ItemType.Components, content, game);
            buttons[7] = new UpgradeButton(componentsIcon, 10 * (game.Player.ShipUpgradeLevels[UpgradeType.Agility] + 1), UpgradeType.Agility, ItemType.Components, content, game);
            buttons[8] = new UpgradeButton(componentsIcon, 10 * (game.Player.ShipUpgradeLevels[UpgradeType.Damage] + 1), UpgradeType.Damage, ItemType.Components, content, game);
            buttons[9] = new UpgradeButton(componentsIcon, 10 * (game.Player.ShipUpgradeLevels[UpgradeType.ProjSpeed] + 1), UpgradeType.ProjSpeed, ItemType.Components, content, game);

            // Load font
            titleFont = content.Load<SpriteFont>("title");
            textFont = content.Load<SpriteFont>("millennia");
        }

        /// <summary>
        /// Method to update the shop
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) {
            for(int i = 0; i < buttons.Length; i++) buttons[i].Update(gameTime);

        }


        /// <summary>
        /// Method to draw the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            
            // Background
            var bgRectangle = new Rectangle(Starforged.gDevice.Viewport.Width / 2, 
                                            Starforged.gDevice.Viewport.Height / 2,
                                            background.Width,
                                            background.Height);
            spriteBatch.Draw(shopBackground, bgRectangle, new Rectangle(0,0, shopBackground.Width, shopBackground.Height),
                             Color.White, 0f, new Vector2(shopBackground.Width / 2, shopBackground.Height / 2), SpriteEffects.None, 0);

            // Border
            for (int i = 0; i < 3; i++) {
                var borderWidth = 3;
                spriteBatch.Draw(border,
                                new Rectangle(background.X - borderWidth,
                                              background.Y - borderWidth,
                                              Starforged.gDevice.Viewport.Width - 2 * (background.X - borderWidth),
                                              Starforged.gDevice.Viewport.Height - 2 * (background.Y - borderWidth)),
                                Color.DimGray);
            }

            // Middle line
            spriteBatch.Draw(white,
                                new Rectangle(Starforged.gDevice.Viewport.Width / 2,
                                              background.Y + padding - 1,
                                              2,
                                              background.Height - 2 * padding),
                                Color.White);

            // Titles
            var titleScale = 0.7f;
            var shopText = "Shop";
            var shopOrigin = titleFont.MeasureString(shopText) * titleScale;
            var shopPos = new Vector2(background.X + background.Width / 4, background.Y + padding);
            spriteBatch.DrawString(titleFont, shopText, shopPos, Color.White, 0f, shopOrigin / 2, titleScale, SpriteEffects.None, 0);

            // Shop items
            Vector2 shopItemPos = new Vector2(background.X + background.Width / 8, shopPos.Y + shopOrigin.Y + 2 * spacing);
            drawShopItem(spriteBatch, shopItemPos, fuelIcon, "Fuel", 0);
            drawShopItem(spriteBatch, shopItemPos + new Vector2(2 * background.Width / 8,0), jumpFuelIcon, "Jump Fuel", 1);
            shopItemPos += new Vector2(0, shopItemIconSize + 4 * spacing + 2 * textFont.MeasureString("1").Y);
            drawShopItem(spriteBatch, shopItemPos, componentsIcon, "Components", 2);
            drawShopItem(spriteBatch, shopItemPos + new Vector2(2 * background.Width / 8,0), ammoIcon, "Ammo", 3);

            var amountText = "Amount: ";
            var amountOrigin = textFont.MeasureString(amountText);
            var amountPos = new Vector2(background.X + background.Width / 8, background.Bottom - padding);
            spriteBatch.DrawString(textFont, amountText, amountPos, Color.White, 0f, new Vector2(amountOrigin.X / 2, amountOrigin.Y), 1f, SpriteEffects.None, 0);
            Point amountButtonSize = new Point((int)textFont.MeasureString(" 1000x ").X, (int)textFont.MeasureString("1").Y);
            buttons[4].Draw(spriteBatch, new Rectangle((int)amountPos.X + (int)amountOrigin.X,
                                                       background.Bottom - padding - amountButtonSize.Y,
                                                       amountButtonSize.X,
                                                       amountButtonSize.Y));


            var upgradeText = "Upgrade";
            var upgradeOrigin = titleFont.MeasureString(upgradeText) * titleScale;
            var upgradePos = new Vector2(background.Right - background.Width / 4, background.Y + padding);
            spriteBatch.DrawString(titleFont, upgradeText, upgradePos, Color.White, 0f, upgradeOrigin / 2, titleScale, SpriteEffects.None, 0);


            // Upgrades
            Vector2 upgradeItemPos = new Vector2(background.X + 9 * background.Width / 16, upgradePos.Y + upgradeOrigin.Y + 2 * spacing);
            int upgradeItemH = (int)textFont.MeasureString("1").Y + spacing;
            drawUpgradeItem(spriteBatch, upgradeItemPos, "Health", game.Player.ShipUpgradeLevels[UpgradeType.Health], 5);
            upgradeItemPos += new Vector2(0, upgradeItemH);
            drawUpgradeItem(spriteBatch, upgradeItemPos, "Thrusters", game.Player.ShipUpgradeLevels[UpgradeType.Speed], 6);
            upgradeItemPos += new Vector2(0, upgradeItemH);
            drawUpgradeItem(spriteBatch, upgradeItemPos, "Maneuverability", game.Player.ShipUpgradeLevels[UpgradeType.Agility], 7);
            upgradeItemPos += new Vector2(0, upgradeItemH);
            drawUpgradeItem(spriteBatch, upgradeItemPos, "Damage", game.Player.ShipUpgradeLevels[UpgradeType.Damage], 8);
            upgradeItemPos += new Vector2(0, upgradeItemH);
            drawUpgradeItem(spriteBatch, upgradeItemPos, "Projectile speed", game.Player.ShipUpgradeLevels[UpgradeType.ProjSpeed], 9);



        }

        private void drawShopItem(SpriteBatch spriteBatch, Vector2 pos, Texture2D icon, string name, int index) {
            spriteBatch.Draw(icon, new Rectangle((int)pos.X, (int)pos.Y, shopItemIconSize, shopItemIconSize), 
                                   new Rectangle(0, 0, icon.Width, icon.Height), Color.White, 0f, new Vector2(icon.Width / 2, 0), SpriteEffects.None, 0);
            var textOrigin = textFont.MeasureString(name);
            pos = new Vector2(pos.X, pos.Y + shopItemIconSize + spacing);
            spriteBatch.DrawString(textFont, name, pos, Color.White, 0f, new Vector2(textOrigin.X / 2, 0), 1f, SpriteEffects.None, 0);
            buttons[index].Draw(spriteBatch, new Rectangle((int)pos.X - buttonW / 2,
                                                           (int)pos.Y + (int)textOrigin.Y + spacing,
                                                           buttonW,
                                                           (int)textFont.MeasureString("1").Y));
        }

        private void drawUpgradeItem(SpriteBatch spriteBatch, Vector2 pos, string name, int level, int index) {
            var textOrigin = textFont.MeasureString(name);
            spriteBatch.DrawString(textFont, name, pos, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            pos = new Vector2(pos.X + 5 * background.Width / 16, pos.Y);
            if (level < 5) {
                buttons[index].Draw(spriteBatch, new Rectangle((int)pos.X,
                                                               (int)pos.Y,
                                                               buttonW,
                                                               (int)textFont.MeasureString("1").Y));
            }
            string levelText = "lvl. " + (level == 5 ? "Max" : level.ToString());
            var levelTextOrigin = textFont.MeasureString(levelText);
            spriteBatch.DrawString(textFont, levelText, new Vector2(pos.X - levelTextOrigin.X / 2 - spacing, pos.Y), Color.White, 0f, new Vector2(levelTextOrigin.X / 2, 0), 1f, SpriteEffects.None, 0);

        }
    }
}

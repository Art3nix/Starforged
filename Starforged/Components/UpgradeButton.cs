using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Starforged {

    public enum UpgradeType {
        Health = 1,
        Speed = 2,
        Agility = 3,
        Damage = 4,
        ProjSpeed = 5
    }
    public class UpgradeButton : Button {

        private Texture2D icon;
        private SpriteFont font;

        private int iconSize;
        private int price;
        private int padding;

        private UpgradeType upgradeType;
        private ItemType soldType;

        private MouseState priorMouse;

        private Starforged game;

        public UpgradeButton(Texture2D icon, int price, UpgradeType upgradeType, ItemType soldType, ContentManager content, Starforged game) { 
            this.icon = icon;
            this.price = price;
            this.upgradeType = upgradeType;
            this.soldType = soldType;
            this.game = game;

            LoadContent(content);

            priorMouse = Mouse.GetState();
        }

        public override void LoadContent(ContentManager content) {

            base.LoadContent(content);

            // Load font
            font = content.Load<SpriteFont>("millennia");
        }

        public override void Update(GameTime gameTime) {
            if (bounds.Contains(Mouse.GetState().Position) && 
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                priorMouse.LeftButton != ButtonState.Pressed) {
                
                // TODO create class for every resource type
                switch (soldType) {
                    case ItemType.Fuel:
                        if (game.Player.Fuel >= price) {
                            game.Player.Fuel -= price;
                            price += 10;
                            upgrade();
                        }
                        break;
                    case ItemType.JumpFuel:
                        if (game.Player.JumpFuel >= price) {
                            game.Player.JumpFuel -= price;
                            price += 10;
                            upgrade();
                        }
                        break;
                    case ItemType.Components:
                        if (game.Player.Components >= price) {
                            game.Player.Components -= price;
                            price += 10;
                            upgrade();
                        }
                        break;
                    case ItemType.Credits:
                        if (game.Player.Credits >= price) {
                            game.Player.Credits -= price;
                            price += 10;
                            upgrade();
                        }
                        break;
                    case ItemType.Ammo:
                        if (game.Player.Ammo >= price) {
                            game.Player.Ammo -= price;
                            price += 10;
                            upgrade();
                        }
                        break;
                }
            }

            

            priorMouse = Mouse.GetState();

        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds) {
            this.bounds = bounds;
            padding = bounds.Height / 10;
            iconSize = bounds.Height - 2 * padding;

            DrawBackground(spriteBatch);

            var textOrigin = font.MeasureString(price.ToString());
            spriteBatch.DrawString(font, price.ToString(), new Vector2(bounds.Center.X - iconSize / 2, bounds.Center.Y),
                                   Color.White, 0f, textOrigin / 2, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(icon, new Rectangle(bounds.Center.X + (int)textOrigin.X / 2, bounds.Center.Y, iconSize, iconSize), new Rectangle(0, 0, icon.Width, icon.Height),
                                                 Color.White, 0f, new Vector2(icon.Width / 2, icon.Height / 2), SpriteEffects.None, 0f);

        }

        private void upgrade() {
            switch (upgradeType) {
                case UpgradeType.Health:
                    game.Player.ShipUpgradeLevels[UpgradeType.Health] += 1;
                    break;
                case UpgradeType.Speed:
                    game.Player.ShipUpgradeLevels[UpgradeType.Speed] += 1;
                    break;
                case UpgradeType.Agility:
                    game.Player.ShipUpgradeLevels[UpgradeType.Agility] += 1;
                    break;
                case UpgradeType.Damage:
                    game.Player.ShipUpgradeLevels[UpgradeType.Damage] += 1;
                    break;
                case UpgradeType.ProjSpeed:
                    game.Player.ShipUpgradeLevels[UpgradeType.ProjSpeed] += 1;
                    break;
            }
        }

        
    }
}

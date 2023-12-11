using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Starforged {
    public class ShopButton : Button {

        private Texture2D icon;
        private SpriteFont font;

        private int iconSize;
        private int price;
        private AmountButton amountButton;
        private int padding;

        private ItemType purchasedType;
        private ItemType soldType;

        private MouseState priorMouse;

        private Starforged game;

        public ShopButton(Texture2D icon, int price, ItemType purchasedType, ItemType soldType, ContentManager content, Starforged game, AmountButton amountButton) { 
            this.icon = icon;
            this.price = price;
            this.purchasedType = purchasedType;
            this.soldType = soldType;
            this.game = game;
            this.amountButton = amountButton;

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
                        if (game.Player.Fuel >= (price * amountButton.PurchaseAmount)) {
                            game.Player.Fuel -= (price * amountButton.PurchaseAmount);
                            purchaseItem();
                        }
                        break;
                    case ItemType.JumpFuel:
                        if (game.Player.JumpFuel >= (price * amountButton.PurchaseAmount)) {
                            game.Player.JumpFuel -= (price * amountButton.PurchaseAmount);
                            purchaseItem();
                        }
                        break;
                    case ItemType.Components:
                        if (game.Player.Components >= (price * amountButton.PurchaseAmount)) {
                            game.Player.Components -= (price * amountButton.PurchaseAmount);
                            purchaseItem();
                        }
                        break;
                    case ItemType.Credits:
                        if (game.Player.Credits >= (price * amountButton.PurchaseAmount)) {
                            game.Player.Credits -= (price * amountButton.PurchaseAmount);
                            purchaseItem();
                        }
                        break;
                    case ItemType.Ammo:
                        if (game.Player.Ammo >= (price * amountButton.PurchaseAmount)) {
                            game.Player.Ammo -= (price * amountButton.PurchaseAmount);
                            purchaseItem();
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

            var textOrigin = font.MeasureString((price * amountButton.PurchaseAmount).ToString());
            spriteBatch.DrawString(font, (price * amountButton.PurchaseAmount).ToString(), new Vector2(bounds.Center.X - iconSize / 2, bounds.Center.Y),
                                   Color.White, 0f, textOrigin / 2, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(icon, new Rectangle(bounds.Center.X + (int)textOrigin.X / 2, bounds.Center.Y, iconSize, iconSize), new Rectangle(0, 0, icon.Width, icon.Height),
                                                 Color.White, 0f, new Vector2(icon.Width / 2, icon.Height / 2), SpriteEffects.None, 0f);

        }

        private void purchaseItem() {
            switch (purchasedType) {
                case ItemType.Fuel:
                    game.Player.Fuel += amountButton.PurchaseAmount;
                    break;
                case ItemType.JumpFuel:
                    game.Player.JumpFuel += amountButton.PurchaseAmount;
                    break;
                case ItemType.Components:
                    game.Player.Components += amountButton.PurchaseAmount;
                    break;
                case ItemType.Credits:
                    game.Player.Credits += amountButton.PurchaseAmount;
                    break;
                case ItemType.Ammo:
                    game.Player.Ammo += amountButton.PurchaseAmount;
                    break;
            }
        }

        
    }
}

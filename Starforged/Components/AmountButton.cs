using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Starforged {
    public class AmountButton : Button {

        private SpriteFont font;
        private MouseState priorMouse;

        public int PurchaseAmount;

        public AmountButton(ContentManager content) {

            PurchaseAmount = 1;

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
                PurchaseAmount *= 10;
                if (PurchaseAmount == 1000) PurchaseAmount = 1;

            }

            priorMouse = Mouse.GetState();
        }
        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds) {
            this.bounds = bounds;

            DrawBackground(spriteBatch);

            var textOrigin = font.MeasureString("x" + PurchaseAmount.ToString());
            spriteBatch.DrawString(font, "x" + PurchaseAmount.ToString(), new Vector2(bounds.Center.X, bounds.Center.Y),
                                   Color.White, 0f, textOrigin / 2, 1f, SpriteEffects.None, 0f);
        }

    }
}

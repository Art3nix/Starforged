using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged {
    public abstract class Button {

        protected Texture2D background;
        protected Texture2D border;

        protected Rectangle bounds;

        public Button() { }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, Rectangle bounds);

        public void Draw(SpriteBatch spriteBatch) {
            Draw(spriteBatch, bounds);
        }

        public virtual void LoadContent(ContentManager content) {
            background = content.Load<Texture2D>("background/buttonBackground");
            border = content.Load<Texture2D>("rectangle");
        }

        public void DrawBackground(SpriteBatch spriteBatch) {
            // Background
            spriteBatch.Draw(background, bounds, Color.White);

            // Border
            var borderWidth = 3;
            for (int i = 0; i < borderWidth; i++) {
                spriteBatch.Draw(border,
                                new Rectangle(bounds.X - borderWidth + i,
                                              bounds.Y - borderWidth + i,
                                              bounds.Width + 2 * (borderWidth - i),
                                              bounds.Height + 2 * (borderWidth - i)),
                                Color.White);
            }
        }

        
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged {

    /// <summary>
    /// Representation of fixed background
    /// </summary>
    public class FixedBackground {
        //To be used later

        private Texture2D texture;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>("background/space");
        }

        /// <summary>
        /// Draws the the background tiles
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);
        }
    }
}

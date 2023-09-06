using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged {

    /// <summary>
    /// Representation of fixed background
    /// </summary>
    public class FixedBackground : Background {

        /// <summary>
        /// Draws the the background tiles
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);
        }
    }
}

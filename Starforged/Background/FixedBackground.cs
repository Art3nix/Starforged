using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged {

    /// <summary>
    /// Representation of fixed background
    /// </summary>
    public class FixedBackground : Background {

        /// <summary>
        /// Draws the background tiles
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);
        }

        /// <summary>
        /// Draw the map of the background in bottom right corner
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public override void DrawMapBackground(SpriteBatch spriteBatch, float mapScale) {
            var ratio = 5;
            spriteBatch.Draw(texture,
                             new Rectangle(Starforged.gDevice.Viewport.Width - texture.Width / ratio - texture.Width, 
                                           Starforged.gDevice.Viewport.Height - texture.Height / ratio - texture.Height,
                                           texture.Width / ratio,
                                           texture.Height / ratio),
                             Color.White);

        }
    }
}

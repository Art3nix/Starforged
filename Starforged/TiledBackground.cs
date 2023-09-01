using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starforged {

    /// <summary>
    /// Representation of tiled background
    /// </summary>
    public class TiledBackground {
        //To be used later

        private Texture2D texture;
        private int horizontalCount;
        private int verticalCount;
        /// <summary>
        /// Constructs a new tiled background
        /// </summary>
        /// <param name="texture">The texture to show on background</param>
        /// <param name="screenWidth">The width of the screen</param>
        /// <param name="screenHeight">The height of the screen</param>
        public TiledBackground(Texture2D texture, int screenWidth, int screenHeight) {
            this.texture = texture;
            this.horizontalCount = (int) (screenWidth/texture.Width + 1);
            this.verticalCount = (int) (screenHeight/texture.Height + 1);
        }

        /// <summary>
        /// Draws the the background tiles
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(SpriteBatch spriteBatch) {

            for(int i = 0; i < horizontalCount; i++) {
                for(int j = 0; j < verticalCount; j++) {
                    spriteBatch.Draw(texture,
                                     new Rectangle((int)(i * texture.Width),
                                                   (int)(j * texture.Height),
                                                   texture.Width,
                                                   texture.Height),
                                     Color.White);
                }
            }
        }
    }
}

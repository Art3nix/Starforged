using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged {

    /// <summary>
    /// Representation of tiled background
    /// </summary>
    public class TiledBackground : Background {

        private int horizontalCount;
        private int verticalCount;
        private int screenWidth;
        private int screenHeight;

        /// <summary>
        /// Constructs a new tiled background
        /// </summary>
        /// <param name="screenWidth">The width of the screen</param>
        /// <param name="screenHeight">The height of the screen</param>
        public TiledBackground(int screenW = 1920, int screenH = 1080) {
            screenWidth = screenW;
            screenHeight = screenH;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        /// <param name="textureName">The name of the texture to load</param>
        public override void LoadContent(ContentManager content, string textureName) {
            texture = content.Load<Texture2D>(textureName);
            horizontalCount = screenWidth / texture.Width + 1;
            verticalCount = screenHeight / texture.Height + 1;
        }

        /// <summary>
        /// Draws the the background tiles
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public override void Draw(SpriteBatch spriteBatch) {

            for (int i = 0; i < horizontalCount; i++) {
                for (int j = 0; j < verticalCount; j++) {
                    spriteBatch.Draw(texture,
                                     new Rectangle(i * texture.Width,
                                                   j * texture.Height,
                                                   texture.Width,
                                                   texture.Height),
                                     Color.White);
                }
            }
        }
    }
}

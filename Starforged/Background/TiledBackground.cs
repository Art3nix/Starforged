using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;

namespace Starforged {

    /// <summary>
    /// Representation of tiled background
    /// </summary>
    public class TiledBackground : Background {

        private int horizontalCount;
        private int verticalCount;
        private int sceneWidth;
        private int sceneHeight;
        private int offsetX;
        private int offsetY;

        /// <summary>
        /// Constructs a new tiled background
        /// </summary>
        /// <param name="screenWidth">The width of the screen</param>
        /// <param name="screenHeight">The height of the screen</param>
        public TiledBackground(int sceneW = 1920, int sceneH = 1080, int offsetX = 0, int offsetY = 0) {
            sceneWidth = sceneW;
            sceneHeight = sceneH;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        /// <param name="textureName">The name of the texture to load</param>
        public override void LoadContent(ContentManager content, string textureName) {
            texture = content.Load<Texture2D>(textureName);
            horizontalCount = sceneWidth / texture.Width + 1;
            verticalCount = sceneHeight / texture.Height + 1;
        }

        /// <summary>
        /// Draws the the background tiles
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public override void Draw(SpriteBatch spriteBatch) {

            for (int i = 0; i < horizontalCount; i++) {
                for (int j = 0; j < verticalCount; j++) {
                    int width = texture.Width, height = texture.Height;
                    if (i == horizontalCount - 1)
                        width = sceneWidth % texture.Width;
                    if (j == verticalCount - 1)
                        height = sceneHeight % texture.Height;
                    spriteBatch.Draw(texture,
                                     new Vector2(offsetX + i * texture.Width,
                                                 offsetY + j * texture.Height),
                                     new Rectangle(0, 0, width, height),
                                     Color.White);
                }
            }
        }

        /// <summary>
        /// Draw the map in the bottom right corner
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public override void DrawMapBackground(SpriteBatch spriteBatch, float mapScale) {
            int mapWidth = (int)(mapScale * sceneWidth);
            int mapHeight = (int)(mapScale * sceneHeight);
            int mapTileWidth = (int)(texture.Width * mapScale);
            int mapTileHeight = (int)(texture.Height * mapScale);

            for (int i = 0; i < horizontalCount; i++) {
                for (int j = 0; j < verticalCount; j++) {
                    int width = texture.Width, height = texture.Height;
                    if (i == horizontalCount - 1)
                        width = sceneWidth % texture.Width;
                    if (j == verticalCount - 1)
                        height = sceneHeight % texture.Height;
                    spriteBatch.Draw(texture,
                                     new Vector2(Starforged.gDevice.Viewport.Width - mapWidth + i * mapTileWidth,
                                                 Starforged.gDevice.Viewport.Height - mapHeight + j * mapTileHeight),
                                     new Rectangle(0,0,width, height),
                                     Color.White, 0f, new Vector2(0,0), mapScale, SpriteEffects.None, 1);
                }
            }
        }
    }
}

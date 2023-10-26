using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged {
    public class Map {

        /// <summary>
        /// The tile width
        /// </summary>
        public int TileWidth { get; init; }

        /// <summary>
        /// The tile height
        /// </summary>
        public int TileHeight { get; init; }

        /// <summary>
        /// The map width
        /// </summary>
        public int MapWidth { get; init; }

        /// <summary>
        /// The map height
        /// </summary>
        public int MapHeight { get; init; }

        /// <summary>
        /// The texture of the tileset
        /// </summary>
        public Texture2D TilesetTexture { get; init; }

        /// <summary>
        /// The tile info on the map
        /// </summary>
        public Rectangle[] Tiles { get; init; }

        /// <summary>
        /// The data which tile to draw on the map
        /// </summary>
        public int[] MapData { get; init; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, int offsetX = 0, int offsetY = 0) {
            for (int y = 0; y < MapHeight; y++) {
                for (int x = 0; x < MapWidth; x++) {
                    int index = MapData[y * MapWidth + x];
                    if (index == -1) continue;
                    spriteBatch.Draw(TilesetTexture, new Vector2(offsetX + x * TileWidth, offsetY + y * TileHeight), Tiles[index], Color.White);
                }
            }
        }
    }
}

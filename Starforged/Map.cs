using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;

namespace Starforged {
    public class Map {

        /// <summary>
        /// THe dimensions of tiles and map
        /// </summary>
        int tileWidth, tileHeight, mapWidth, mapHeight;

        /// <summary>
        /// The texture of the tileset
        /// </summary>
        Texture2D tilesetTexture;

        /// <summary>
        /// The tile info on the map
        /// </summary>
        Rectangle[] tiles;

        /// <summary>
        /// The data which tile to draw on the map
        /// </summary>
        int[] map;

        /// <summary>
        /// The name of the file containing map data
        /// </summary>
        string filename;

        public Map(string filename) {
            this.filename = filename;
        }

        public void LoadContent(ContentManager content) {
            string data = File.ReadAllText(Path.Join(content.RootDirectory, filename));
            var lines = data.Split('\n');

            // First line is the tileset filename
            var tilesetFilename = lines[0].Trim();
            tilesetTexture = content.Load<Texture2D>(tilesetFilename);

            // Second line is dimensions of the tile
            var secondLine = lines[1].Split(',');
            tileWidth = int.Parse(secondLine[0]);
            tileHeight = int.Parse(secondLine[1]);

            // Initialize tiles
            int tilesetCols = tilesetTexture.Width / tileWidth;
            int tilesetRows = tilesetTexture.Height / tileHeight;
            tiles = new Rectangle[tilesetCols * tilesetRows];
            for (int x = 0; x < tilesetRows; x++) {
                for (int y = 0; y < tilesetCols; y++) {
                    int index = x * tilesetCols + y;
                    tiles[index] = new Rectangle(y * tileWidth, x * tileHeight, tileWidth, tileHeight);
                }
            }

            // Third line is the map size
            var thirdLine = lines[2].Split(',');
            mapWidth = int.Parse(thirdLine[0]);
            mapHeight = int.Parse(thirdLine[1]);

            // Fourth line are map data
            var fourthLine = lines[3].Split(',');
            map = new int[mapWidth * mapHeight];
            for (int i = 0; i < mapWidth * mapHeight; i++) {
                map[i] = int.Parse(fourthLine[i]);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, int offsetX = 0, int offsetY = 0) {
            for (int y = 0; y < mapHeight; y++) {
                for (int x = 0; x < mapWidth; x++) {
                    int index = map[y * mapWidth + x];
                    if (index == -1) continue;
                    spriteBatch.Draw(tilesetTexture, new Vector2(offsetX + x * tileWidth, offsetY + y * tileHeight), tiles[index], Color.White);
                }
            }
        }
    }
}

using MapPipeline;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace MapPipeline {

    [ContentProcessor(DisplayName = "MapProcessor")]
    public class MapProcessor : ContentProcessor<MapContent, MapContent> {

        public float Scale { get; set; } = 1f;

        public override MapContent Process(MapContent map, ContentProcessorContext context) {

            map.TilesetTexture = context.BuildAndLoadAsset<TextureContent, Texture2DContent>(
                    new ExternalReference<TextureContent>(map.Filename), "TextureProcessor");

            int tilesetCols = map.TilesetTexture.Mipmaps[0].Width / map.TileWidth;
            int tilesetRows = map.TilesetTexture.Mipmaps[0].Height / map.TileHeight;
            map.Tiles = new Rectangle[tilesetCols * tilesetRows];
            for (int x = 0; x < tilesetRows; x++) {
                for (int y = 0; y < tilesetCols; y++) {
                    int index = x * tilesetCols + y;
                    map.Tiles[index] = new Rectangle(y * map.TileWidth, x * map.TileHeight, map.TileWidth, map.TileHeight);
                }
            }

            map.TileWidth = (int)(map.TileWidth * Scale);
            map.TileHeight = (int)(map.TileHeight * Scale);

            return map;
        }
    }
}
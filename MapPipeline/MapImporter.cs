using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace MapPipeline {

    [ContentImporter(".tmap", DisplayName = "MapImporter", DefaultProcessor = "MapProcessor")]
    public class MapImporter : ContentImporter<MapContent> {
        public override MapContent Import(string filename, ContentImporterContext context) {

            MapContent map = new();

            string data = File.ReadAllText(filename);
            var lines = data.Split('\n');

            // First line is the tileset filename
            map.Filename = lines[0].Trim();

            // Second line is dimensions of the tile
            var secondLine = lines[1].Split(',');
            map.TileWidth = int.Parse(secondLine[0]);
            map.TileHeight = int.Parse(secondLine[1]);

            // Third line is the map size
            var thirdLine = lines[2].Split(',');
            map.MapWidth = int.Parse(thirdLine[0]);
            map.MapHeight = int.Parse(thirdLine[1]);

            // Fourth line are map data
            var fourthLine = lines[3].Split(',');
            map.MapData = new int[map.MapWidth * map.MapHeight];
            for (int i = 0; i < map.MapWidth * map.MapHeight; i++) {
                map.MapData[i] = int.Parse(fourthLine[i]);
            }

            return map;
        }
    }
}
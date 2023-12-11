using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace MapPipeline {

    [ContentSerializerRuntimeType("Starforged.ImportedMap, Starforged")]
    public class MapContent {


        /// <summary>
        /// THe dimensions of tiles and map
        /// </summary>
        public int TileWidth, TileHeight, MapWidth, MapHeight;

        /// <summary>
        /// The texture of the tileset
        /// </summary>
        public Texture2DContent TilesetTexture;

        /// <summary>
        /// The tile info on the map
        /// </summary>
        public Rectangle[] Tiles;

        /// <summary>
        /// The data which tile to draw on the map
        /// </summary>
        public int[] MapData;

        /// <summary>
        /// Number of locations
        /// </summary>
        public int PlanetCount;

        /// <summary>
        /// Index of the current planet
        /// </summary>
        public int CurrentLocation;

        /// <summary>
        /// The name of the file containing map data
        /// </summary>
        [ContentSerializerIgnore]
        public string Filename;
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Starforged;

namespace MapPipeline {

    [ContentSerializerRuntimeType("Starforged.Map, Starforged")]
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
        /// The objects on the map
        /// </summary>
        public Planet[] Planets;

        /// <summary>
        /// Number of locations
        /// </summary>
        public int PlanetCount;

        /// <summary>
        /// Index of the current planet
        /// </summary>
        public int CurrentLocation;

        /// <summary>
        /// The game
        /// </summary>
        public Starforged.Starforged game;

        /// <summary>
        /// The name of the file containing map data
        /// </summary>
        [ContentSerializerIgnore]
        public string Filename;
    }
}

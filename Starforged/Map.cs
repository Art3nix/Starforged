using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        /// <summary>
        /// The objects on the map
        /// </summary>
        public Planet[] planets;

        /// <summary>
        /// The game
        /// </summary>
        public Starforged game;

        private SpriteFont planetFont;
        private int offsetX;
        private int offsetY;


        public void Initialize(Starforged g, int offsetX = 0, int offsetY = 0) {
            game = g;
            this.offsetX = offsetX;
            this.offsetY = offsetY;


            (string, Scene)[] planetInfo = {
                            ("Base", new AsteroidFieldScene(game)),
                            ("Level 1", new EnemyShipScene(game, 5)),
                            ("Level 2", new EnemyShipScene(game, 10)),
                            ("Level 3", new EnemyShipScene(game, 15)),
                            ("Level 4", new EnemyShipScene(game, 20)),
                            ("Level 5", new EnemyShipScene(game, 25)),
                            };

            int planetCount = Tiles.Length;
            planets = new Planet[planetCount];

            for (int y = 0; y < MapHeight; y++) {
                for (int x = 0; x < MapWidth; x++) {
                    int index = MapData[y * MapWidth + x];
                    if (index == -1) continue;
                    Rectangle area = new Rectangle(offsetX + x * TileWidth,
                                                   offsetY + y * TileHeight,
                                                   TileWidth,
                                                   TileHeight);
                    planets[index] = new Planet(planetInfo[index].Item1, planetInfo[index].Item2, area);
                }
            }

        }

        public void LoadContent(ContentManager content) {

            // Load font
            planetFont = content.Load<SpriteFont>("millennia");
        }

        public void Update() {
            MouseState mouse = Mouse.GetState();
            for (int i = 0; i < planets.Length; i++) {
                if (planets[i].Area.Contains(mouse.Position)) {
                    if (mouse.LeftButton == ButtonState.Pressed) {
                        // enter the map
                        game.ChangeScene(planets[i].LevelScene);
                        break;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            for (int y = 0; y < MapHeight; y++) {
                for (int x = 0; x < MapWidth; x++) {
                    int index = MapData[y * MapWidth + x];
                    if (index == -1) continue;
                    spriteBatch.Draw(TilesetTexture, new Vector2(offsetX + x * TileWidth, offsetY + y * TileHeight), Tiles[index], Color.White);
                }
            }

            MouseState mouse = Mouse.GetState();
            for (int i = 0; i < planets.Length; i++) {
                if (planets[i].Area.Contains(mouse.Position)) {
                    //on hover show name
                    var planetScale = 0.8f;
                    var planetText = planets[i].Name;
                    var planetPos = new Vector2(planets[i].Area.Center.X, planets[i].Area.Bottom + 10);
                    spriteBatch.DrawString(planetFont, planetText, planetPos, Color.White, 0f, planetFont.MeasureString(planetText) / 2, planetScale, SpriteEffects.None, 0);

                }
            }
        }
    }
}

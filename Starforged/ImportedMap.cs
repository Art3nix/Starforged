﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Starforged {
    public class ImportedMap : Map {

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
        /// Number of locations
        /// </summary>
        public int PlanetCount;

        /// <summary>
        /// Index of the current planet
        /// </summary>
        public int CurrentLocation = 0;

        private SpriteFont planetFont;


        public void Initialize(Starforged game, int offsetX = 0, int offsetY = 0) {
            

            (string, Scene)[] planetInfo = {
                            ("Base", new BaseScene(game)),
                            ("Level 1", new EnemyShipScene(game, 5)),
                            ("Level 2", new EnemyShipScene(game, 10)),
                            ("Level 3", new EnemyShipScene(game, 15)),
                            ("Level 4", new EnemyShipScene(game, 20)),
                            ("Level 5", new EnemyShipScene(game, 25)),
                            };

            PlanetCount = Tiles.Length;
            Planets = new Planet[PlanetCount];

            for (int y = 0; y < MapHeight; y++) {
                for (int x = 0; x < MapWidth; x++) {
                    int index = MapData[y * MapWidth + x];
                    if (index == -1) continue;
                    Rectangle area = new Rectangle(offsetX + x * TileWidth,
                                                   offsetY + y * TileHeight,
                                                   TileWidth,
                                                   TileHeight);
                    Planets[index] = new Planet(planetInfo[index].Item1, planetInfo[index].Item2, area);
                }
            }

        }

        public void LoadContent(ContentManager content) {

            // Load font
            planetFont = content.Load<SpriteFont>("millennia");
        }

        public void Update(Starforged game) {
            MouseState mouse = Mouse.GetState();
            for (int i = 0; i < PlanetCount && i <= game.Player.Level; i++) {
                if (Planets[i].Area.Contains(mouse.Position)) {
                    if (mouse.LeftButton == ButtonState.Pressed) {
                        // enter the map
                        game.ChangeScene(Planets[i].LevelScene);
                        CurrentLocation = i;
                        break;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Starforged game) {

            MouseState mouse = Mouse.GetState();
            for (int i = 0; i < PlanetCount && i <= game.Player.Level; i++) {
                // Draw the planet
                spriteBatch.Draw(TilesetTexture, new Vector2(Planets[i].Area.X, Planets[i].Area.Y), Tiles[i], Color.White);

                if (Planets[i].Area.Contains(mouse.Position)) {
                    //on hover show name
                    var planetScale = 0.8f;
                    var planetText = Planets[i].Name;
                    var planetPos = new Vector2(Planets[i].Area.Center.X, Planets[i].Area.Bottom + 10);
                    spriteBatch.DrawString(planetFont, planetText, planetPos, Color.White, 0f, planetFont.MeasureString(planetText) / 2, planetScale, SpriteEffects.None, 0);

                }
            }
        }
    }
}

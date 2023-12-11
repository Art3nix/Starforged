using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Starforged {

    public enum ItemType {
        Fuel = 1,
        JumpFuel = 2,
        Components = 3,
        Credits = 4,
        Ammo = 5
    }
    public class Item : CollisionObject {


        /// <summary>
        /// Position of the item
        /// </summary>
        public Vector2 Position;

        // Texture
        private Texture2D texture;
        private String textureName;

        private ItemType type;
        public int amount;
        private int size;

        public Item (ContentManager content, ItemType type, int amount, Vector2 pos, String textureName) {


            this.type = type;
            this.amount = amount;
            this.textureName = "icons/" + textureName;

            Position = pos;

            LoadContent(content);

        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>(textureName);
            size = texture.Width;
            Bounds = new BoundingCircle(Position + new Vector2(size / 2, size / 2), size / 2);
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float scale = 1f) {

            spriteBatch.Draw(texture, Position,
                new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(size / 2, size / 2), scale, SpriteEffects.None, 0);
        }

        public void Add(Player player) {
            switch (type) {
                case ItemType.Fuel:
                    player.Fuel += amount;
                    break;
                case ItemType.JumpFuel:
                    player.JumpFuel += amount;
                    break;
                case ItemType.Components:
                    player.Components += amount;
                    break;
                case ItemType.Credits:
                    player.Credits += amount;
                    break;
                case ItemType.Ammo:
                    player.Ammo += amount;
                    break;
            }
            amount = 0;
        }

        public static Item Create (ContentManager content, Vector2 pos, float[] probabilities, int[] maxAmounts) {
            if (probabilities.Length < 5) {
                probabilities = new float[] { 0.1f, 0.05f, 0.05f, 0.2f, 0.25f };
            }
            if (maxAmounts.Length < 5) {
                maxAmounts = new int[] { 20, 10, 10, 20, 5 };
            }
            for (int i = 0; i < maxAmounts.Length; i++) {
                if (maxAmounts[i] < 1) {
                    maxAmounts[i] = 1;
                }
            }

            // process probabilities array
            for (int i = 1; i < probabilities.Length; i++) {
                probabilities[i] += probabilities[i - 1];
            }

            Random r = new Random();
            float f = (float)r.NextDouble();
            int am = 1;
            ItemType type = ItemType.Credits;
            String tName = "credits";

            if (f < probabilities[(int)ItemType.Fuel - 1]) {
                // fuel
                type = ItemType.Fuel;
                am = r.Next(1, maxAmounts[(int)ItemType.Fuel - 1]);
                tName = "fuel";

            } else if (f < probabilities[(int)ItemType.JumpFuel - 1]) {
                // jump fuel
                type = ItemType.JumpFuel;
                am = r.Next(1, maxAmounts[(int)ItemType.JumpFuel - 1]);
                tName = "jumpfuel";

            } else if (f < probabilities[(int)ItemType.Components - 1]) {
                // components
                type = ItemType.Components;
                am = r.Next(1, maxAmounts[(int)ItemType.Components - 1]);
                tName = "component";

            } else if (f < probabilities[(int)ItemType.Credits - 1]) {
                // credits
                type = ItemType.Credits;
                am = r.Next(1, maxAmounts[(int)ItemType.Credits - 1]);
                tName = "credits";

            } else if (f < probabilities[(int)ItemType.Ammo - 1]) {
                // ammo
                type = ItemType.Ammo;
                am = r.Next(1, maxAmounts[(int)ItemType.Ammo - 1]);
                tName = "ammo";

            }

            return new Item(content, type, am, pos, tName);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged {
    public class SpaceStation : CollisionObject {


        /// <summary>
        /// Position of the station
        /// </summary>
        public Vector2 Position;

        // Texture
        private Texture2D texture;
        private int size;

        /// <summary>
        /// Area close to the space station
        /// </summary>
        public BoundingCircle Vicinity;

        /// <summary>
        /// Bounds of the space station
        /// </summary>
        public BoundingRectangle Bounds;

        public SpaceStation(ContentManager content, Vector2 pos) {

            Position = pos;

            LoadContent(content);

        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content) {
            texture = content.Load<Texture2D>("station");
            size = texture.Width;
            Bounds = new BoundingRectangle(Position - new Vector2(size / 2, size / 2), size, size);
            Vicinity = new BoundingCircle(Position + new Vector2(size / 2, size / 2), size * 2);
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
    }
}

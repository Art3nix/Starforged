using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged {
    public abstract class Background {

        protected Starforged game;

        protected Texture2D texture;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        /// <param name="textureName">The name of the texture to load</param>
        public virtual void LoadContent(ContentManager content, string textureName) {
            texture = content.Load<Texture2D>(textureName);
        }

        /// <summary>
        /// Abstract method to draw the background
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Abstract method to draw map of the background
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public abstract void DrawMapBackground(SpriteBatch spriteBatch, float mapScale);
    }
}

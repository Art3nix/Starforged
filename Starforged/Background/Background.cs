using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace Starforged {
    public abstract class Background {


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
    }
}

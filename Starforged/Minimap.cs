using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starforged {
    public class Minimap {

        private Background background;

        private Scene scene;

        public Minimap(Background background, Scene scene) {
            this.background = background;
            this.scene = scene;
        }


        public void Draw (SpriteBatch spriteBatch) {
            float scale, mapScale;
            if (scene.Width >= scene.Height) {
                scale = 1 / 7f;
                mapScale = scale * Starforged.gDevice.Viewport.Width / scene.Width;
            } else {
                scale = 1 / 4f;
                mapScale = scale * Starforged.gDevice.Viewport.Height / scene.Height;
            }
            background.DrawMapBackground(spriteBatch, mapScale);



        }
    }
}

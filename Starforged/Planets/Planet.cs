using Microsoft.Xna.Framework;

namespace Starforged {
    public class Planet {

        public Rectangle Area;

        public Scene LevelScene;

        public string Name;

        public Planet(string name, Scene scene, Rectangle area) {
            Name = name;
            LevelScene = scene;
            Area = area;
        }

    }
}

using Microsoft.Xna.Framework;

namespace Starforged {
    public class CollisionObject {

        // Collision box
        protected BoundingCircle bounds;

        /// <summary>
        /// Get bounds of the asteroid
        /// </summary>
        public BoundingCircle Bounds => bounds;

        /// <summary>
        /// Get mass of the asteroid
        /// </summary>
        public int Mass;

        /// <summary>
        /// Flying direction of the object
        /// </summary>
        public Vector2 Velocity;
    }
}

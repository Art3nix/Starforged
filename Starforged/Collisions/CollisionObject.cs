using Microsoft.Xna.Framework;

namespace Starforged {
    public class CollisionObject {

        /// <summary>
        /// Collision area of the object
        /// </summary>
        public BoundingCircle Bounds;

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

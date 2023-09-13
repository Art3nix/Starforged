using Microsoft.Xna.Framework;

namespace Starforged {
    /// <summary>
    /// Representation of bounding circle
    /// </summary>
    public struct BoundingCircle {

        /// <summary>
        /// The center of the bounding circle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The radius of the bounding circle
        /// </summary>
        public float Radius;

        /// <summary>
        /// Constructs a new Bounding Circle
        /// </summary>
        /// <param name="center">The center</param>
        /// <param name="radius">The radius</param>
        public BoundingCircle(Vector2 center, float radius) {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Checks for a collision between this and another bounding circle
        /// </summary>
        /// <param name="other">The other bounding circle</param>
        /// <returns>true when there is a collision</returns>
        public bool CollidesWith(BoundingCircle other) {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Checks for a collision between this and another bounding rectangle
        /// </summary>
        /// <param name="other">The other bounding rectangle</param>
        /// <returns>true when there is a collision</returns>
        public bool CollidesWith(BoundingRectangle other) {
            return CollisionHelper.Collides(this, other);
        }
    }
}

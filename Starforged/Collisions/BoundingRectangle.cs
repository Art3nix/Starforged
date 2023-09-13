using Microsoft.Xna.Framework;

namespace Starforged {
    /// <summary>
    /// Representation of bounding circle
    /// </summary>
    public struct BoundingRectangle { 

        /// <summary>
        /// The position of the bounding rectangle
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The width of the bounding rectangle
        /// </summary>
        public float Width;
        /// <summary>
        /// The height of the bounding rectangle
        /// </summary>
        public float Height;

        /// <summary>
        /// The X coordinate of the top left corner
        /// </summary>
        public float Left => Position.X;
        /// <summary>
        /// The X coordinate of the top right corner
        /// </summary>
        public float Right => Position.X + Width;
        /// <summary>
        /// The Y coordinate of the top left corner
        /// </summary>
        public float Top => Position.Y;
        /// <summary>
        /// The Y coordinate of the bottom left corner
        /// </summary>
        public float Bottom => Position.Y + Height;


        /// <summary>
        /// Constructs a new bounding rectangle
        /// </summary>
        /// <param name="position">The position of the rectangle</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        public BoundingRectangle(Vector2 position, float width, float height) {
            Position = position;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Checks for a collision between this and another bounding rectangle
        /// </summary>
        /// <param name="other">The other bounding rectangle</param>
        /// <returns>true when there is a collision</returns>
        public bool CollidesWith(BoundingRectangle other) {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Checks for a collision between this and another bounding circle
        /// </summary>
        /// <param name="other">The other bounding circle</param>
        /// <returns>true when there is a collision</returns>
        public bool CollidesWith(BoundingCircle other) {
            return CollisionHelper.Collides(this, other);
        }

    }
}

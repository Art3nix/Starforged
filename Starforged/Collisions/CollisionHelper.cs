using System;
using Microsoft.Xna.Framework;

namespace Starforged{
    public static class CollisionHelper {

        /// <summary>
        /// Checks for a collision between two bounding circles
        /// </summary>
        /// <param name="a">The first bounding circle</param>
        /// <param name="b">The second bounding circle</param>
        /// <returns>true when circles collide</returns>
        public static bool Collides(BoundingCircle a,  BoundingCircle b) {
            return Math.Pow(a.Radius + b.Radius, 2) >=
                Math.Pow(a.Center.X - b.Center.X, 2) +
                Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Checks for a collision between two bounding rectangles
        /// </summary>
        /// <param name="a">The first bounding rectangle</param>
        /// <param name="b">The second bounding rectangle</param>
        /// <returns>true when rectangles collide</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b) {
            return !(a.Right < b.Left || a.Left > b.Right ||
                     a.Top > b.Bottom || a.Bottom < b.Top);
        }

        /// <summary>
        /// Checks for a collision between a bounding circle and a bounding rectangle
        /// </summary>
        /// <param name="c">The bounding circle</param>
        /// <param name="r">The bounding rectangle</param>
        /// <returns>true when the objects collide</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r) {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);

            return Math.Pow(c.Radius, 2) >=
                Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2);

        }


        /// <summary>
        /// Checks for a collision between a bounding rectangle and a bounding circle
        /// </summary>
        /// <param name="r">The bounding rectangle</param>
        /// <param name="c">The bounding circle</param>
        /// <returns>true when the objects collide</returns>
        public static bool Collides(BoundingRectangle r, BoundingCircle c) {
            return Collides(c, r);
        }
    }
}

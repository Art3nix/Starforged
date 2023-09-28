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

        /// <summary>
        /// Check for and handle elastic collision between two collision objects with bounding circles
        /// </summary>
        /// <param name="a">The first collision object</param>
        /// <param name="b">The second collision object</param>
        public static bool handleElasticCollision(CollisionObject a, CollisionObject b) {
            //TODO fix collisions using aether2d

            // Check for overlapping
            if (a.Bounds.CollidesWith(b.Bounds)) {
                Vector2 collisionVelocity = (a.Velocity - b.Velocity);
                var collisionAxis = b.Bounds.Center - a.Bounds.Center;

                // Check for collision
                if (Vector2.Dot(collisionAxis, collisionVelocity) >= 0) {
                    var m0 = a.Mass;
                    var m1 = b.Mass;

                    float angle = (float)-Math.Atan2(b.Bounds.Center.Y - a.Bounds.Center.Y,
                                                     b.Bounds.Center.X - a.Bounds.Center.X);

                    Vector2 u0 = Vector2.Transform(a.Velocity, Matrix.CreateRotationZ(angle));
                    Vector2 u1 = Vector2.Transform(b.Velocity, Matrix.CreateRotationZ(angle));

                    Vector2 v0, v1;
                    v0 = new Vector2(u0.X * (m0 - m1) / (m0 + m1) + u1.X * 2 * m1 / (m0 + m1), u0.Y);
                    v1 = new Vector2(u1.X * (m1 - m0) / (m0 + m1) + u0.X * 2 * m0 / (m0 + m1), u1.Y);

                    a.Velocity = Vector2.Transform(v0, Matrix.CreateRotationZ(-angle));
                    b.Velocity = Vector2.Transform(v1, Matrix.CreateRotationZ(-angle));

                    return true;
                }


            }

            return false;

        }
    }
}

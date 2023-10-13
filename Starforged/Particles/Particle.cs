using Microsoft.Xna.Framework;

namespace Starforged.Particles {
    public class Particle {

        /// <summary>
        /// Position of the particle
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Velocity of the particle
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// Acceleration of the particle
        /// </summary>
        public Vector2 Acceleration;

        /// <summary>
        /// Angular acceleration of the particle
        /// </summary>
        public float AngularAcceleration;

        /// <summary>
        /// Angular velocity of the particle
        /// </summary>
        public float AngularVelocity;

        /// <summary>
        /// Scale of the particle
        /// </summary>
        public float Scale;

        /// <summary>
        /// Rotation angle of the particle
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Time this particle can be alive
        /// </summary>
        public float Lifetime;

        /// <summary>
        /// Time this particle has been alive for
        /// </summary>
        public float TimeAlive;

        /// <summary>
        /// Color of the particle
        /// </summary>
        public Color Color;

        /// <summary>
        /// Whether this particle is still alive
        /// </summary>
        public bool Active => TimeAlive < Lifetime;



        public void Initialize(Vector2 pos, Vector2 vel, Vector2 acc, Color color, float angAcc = 0, float angVel = 0,
                               float rotation = 0, float lifetime = 1) {
            Position = pos;
            Velocity = vel;
            Acceleration = acc;
            AngularAcceleration = angAcc;
            AngularVelocity = angVel;
            Scale = 1f;
            Rotation = rotation;
            Lifetime = lifetime;
            TimeAlive = 0f;
            Color = color;
        }
    }
}

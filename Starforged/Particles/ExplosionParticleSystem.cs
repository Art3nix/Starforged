using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Starforged.Particles {
    public class ExplosionParticleSystem : ParticleSystem {
        public ExplosionParticleSystem(Starforged game, int maxParticles) : base(game, maxParticles) {

        }

        protected override void InitializeConstants() {

            textureFilename = "Particles/asteroidExplosion";

            minNumParticles = 100;
            maxNumParticles = 175;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 pos) {
            Random r = new Random();
            float angle = (float)r.NextDouble() * MathHelper.TwoPi;
            Vector2 velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * (40 + r.Next(80));
            float lifetime = 0.5f + (float)r.NextDouble();
            Vector2 acc = -velocity / lifetime;
            float rotation = (float)r.NextDouble() * MathHelper.Pi;
            float angVel = (float)r.NextDouble() * MathHelper.Pi - MathHelper.PiOver2; //-pi/2 - pi/2

            p.Initialize(pos, velocity, acc, Color.White, 0f, angVel, rotation, lifetime);
            
        }

        protected override void UpdateParticle(ref Particle particle, GameTime gameTime) {
            base.UpdateParticle(ref particle, gameTime);

            float percentageLifetime = particle.TimeAlive / particle.Lifetime;

            float alpha = 4 * percentageLifetime * (1 - percentageLifetime);
            particle.Color = Color.White * alpha;
            particle.Scale = .05f + .15f * percentageLifetime;
        }

        public void AddExplosion(Vector2 pos) => AddParticles(pos);
    }
}

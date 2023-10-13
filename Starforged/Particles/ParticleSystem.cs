using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Starforged.Particles {
    public abstract class ParticleSystem : DrawableGameComponent {

        protected static SpriteBatch spriteBatch;

        protected static ContentManager content;

        /// <summary>
        /// All particles
        /// </summary>
        Particle[] particles;

        /// <summary>
        /// A Queue of available particles
        /// </summary>
        Queue<int> freeParticles;

        /// <summary>
        /// Particle texture
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// Origin point of particle
        /// </summary>
        Vector2 origin;

        /// <summary>
        /// The BlendState of particle system
        /// </summary>
        protected BlendState blendState = BlendState.AlphaBlend;

        /// <summary>
        /// Draw order for additive blending
        /// </summary>
        public const int AdditiveBlendDrawOrder = 200;

        /// <summary>
        /// Particle texture filename
        /// </summary>
        protected string textureFilename;

        /// <summary>
        /// Minimum number of particles
        /// </summary>
        protected int minNumParticles;

        /// <summary>
        /// Maximum number of particles
        /// </summary>
        protected int maxNumParticles;



        /// <summary>
        /// Construct a new instance of a particle system
        /// </summary>
        /// <param name="game"></param>
        /// <param name="maxParticles"></param>
        public ParticleSystem(Starforged game, int maxParticles) : base(game) {
            particles = new Particle[maxParticles];
            freeParticles = new Queue<int>(maxParticles);
            for (int i = 0; i < particles.Length; i++) {
                particles[i] = new Particle();
                particles[i].Initialize(Vector2.Zero, Vector2.Zero, Vector2.Zero, Color.White);
                freeParticles.Enqueue(i);
            }

            InitializeConstants();

        }

        /// <summary>
        /// Initialize constants of a particle system
        /// </summary>
        protected abstract void InitializeConstants();

        /// <summary>
        /// Initialize particle
        /// </summary>
        /// <param name="p">The particle</param>
        /// <param name="pos">Position on the screen</param>
        protected virtual void InitializeParticle(ref Particle p, Vector2 pos) {
            p.Initialize(pos, Vector2.Zero, Vector2.Zero, Color.White);
        }

        /// <summary>
        /// Update the particle
        /// </summary>
        /// <param name="particle">The particle</param>
        /// <param name="gameTime">The game time</param>
        protected virtual void UpdateParticle(ref Particle particle, GameTime gameTime) {

            particle.AngularVelocity += particle.AngularAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            particle.Rotation += particle.AngularVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            particle.Velocity += particle.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            particle.Position += particle.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the time the particle has been alive 
            particle.TimeAlive += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Load content
        /// </summary>
        protected override void LoadContent() {
            if (content == null) content = new ContentManager(Game.Services, "Content");
            if (spriteBatch == null) spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            texture = content.Load<Texture2D>(textureFilename);
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;

            base.LoadContent();
        }

        /// <summary>
        /// Update the particle system
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            for (int i = 0; i < particles.Length; i++) {
                if (particles[i].Active) {
                    UpdateParticle(ref particles[i], gameTime);
                    if (!particles[i].Active) {
                        freeParticles.Enqueue(i);
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw particles
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime) {

            spriteBatch.Begin(blendState: blendState);
            foreach (Particle p in particles) {
                if (!p.Active)
                    continue;

                spriteBatch.Draw(texture, p.Position, null, p.Color, p.Rotation, origin, p.Scale, SpriteEffects.None, 0.0f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Add new particle
        /// </summary>
        /// <param name="pos">Position to add particle at</param>
        protected void AddParticles(Vector2 pos) {
            Random r = new Random();
            int numParticles = r.Next(minNumParticles, maxNumParticles);

            for (int i = 0; i < numParticles && freeParticles.Count > 0; i++) {
                int index = freeParticles.Dequeue();
                InitializeParticle(ref particles[index], pos);
            }
        }
    }
}

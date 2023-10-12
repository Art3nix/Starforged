﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Starforged {

    /// <summary>
    /// A class representing a ship
    /// </summary>
    public class TitleScreenShip : Ship {

        // Animation values
        private double animationTimer;
        private short animationFrame = 1;

        public TitleScreenShip () {
            // Choose random angle
            Random r = new Random();
            Angle = r.Next(360);
            ShipVelocity = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));

            // Init values
            MAXSPEED = 150;
            SIZE = 48;

            // Choose random position based on the direction
            Position = getRandomPosition(ShipVelocity);

        }

        /// <summary>
        /// Update the ship's position
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            var windowWidth = Starforged.gDevice.Viewport.Width;
            var windowHeight = Starforged.gDevice.Viewport.Height;

            //Move in the correct direction
            Position += ShipVelocity * MAXSPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Return ship back to the screen
            if ((Position.X < -SIZE && ShipVelocity.X < 0) ||
                (Position.Y < -SIZE && ShipVelocity.Y < 0) ||
                (Position.X > windowWidth + SIZE && ShipVelocity.X > 0) ||
                (Position.Y > windowHeight + SIZE && ShipVelocity.Y > 0)) {

                Random r = new Random();
                Angle = r.Next(360);
                ShipVelocity = new Vector2((float)Math.Sin(Angle), (float)-Math.Cos(Angle));
                Position = getRandomPosition(ShipVelocity);

            }

        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (animationTimer > 0.1) {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 1;
                animationTimer -= 0.1;

            }

            //Draw the sprite
            var source = new Rectangle(animationFrame * SIZE, 0, SIZE, SIZE);
            var textureCenter = new Vector2(SIZE / 2, SIZE / 2);
            spriteBatch.Draw(texture, Position, source, Color.White, Angle, textureCenter, 1f, SpriteEffects.None, 0);
        }

        private Vector2 getRandomPosition(Vector2 dir) {
            Random r = new Random();

            var maxX = Starforged.gDevice.Viewport.Width - SIZE;
            var maxY = Starforged.gDevice.Viewport.Height - SIZE;

            var pos = new Vector2(r.Next(maxX), r.Next(maxY));

            if(dir.X < 0) {
                pos.X += maxX;
            } else {
                pos.X -= maxX;
            }
           
            if(dir.Y < 0) {
                pos.Y += maxY;
            } else {
                pos.Y -= maxY;
            }

            return pos;
        }

    }
}

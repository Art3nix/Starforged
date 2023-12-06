using System;

namespace Starforged {

    [Serializable]
    public class Player {

        /// <summary>
        /// The number of regular fuel the player has
        /// </summary>
        public float Fuel;

        /// <summary>
        /// The number of hyper drive fuel the player has
        /// </summary>
        public int JumpFuel;

        /// <summary>
        /// The number of components the player has
        /// </summary>
        public int Components;

        /// <summary>
        /// The number of credits the player has
        /// </summary>
        public int Credits;

        /// <summary>
        /// The number of ammunition the player has
        /// </summary>
        public int Ammo;

        /// <summary>
        /// Which level the player is on
        /// </summary>
        public int Level;

        /// <summary>
        /// The currently used ship
        /// </summary>
        public PlayerShip ship;

        public Player () {
            Fuel = 100f;
            JumpFuel = 150;
            Components = 200;
            Credits = 250;
            Ammo = 300;
            Level = 1;

        }

        public Player(float fuel, int jumpFuel, int components, int credits, int ammo) {
            Fuel = fuel;
            JumpFuel = jumpFuel;
            Components = components;
            Credits = credits;
            Ammo = ammo;
            Level = 1;
        }
    }
}

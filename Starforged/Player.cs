using System;

namespace Starforged {

    [Serializable]
    public class Player {

        /// <summary>
        /// The number of regular fuel the player has
        /// </summary>
        public int Fuel;

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
        /// The currently used ship
        /// </summary>
        public PlayerShip ship;

        public Player() { }

        public Player(int fuel = 100, int jumpFuel = 150, int components = 200, int credits = 250, int ammo = 300) {
            Fuel = fuel;
            JumpFuel = jumpFuel;
            Components = components;
            Credits = credits;
            Ammo = ammo;
        }
    }
}

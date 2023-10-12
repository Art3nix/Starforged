using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starforged {
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

        public Player() {
            Fuel = 100;
            JumpFuel = 150;
            Components = 200;
            Credits = 250;
            Ammo = 300;
        }
    }
}

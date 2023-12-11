using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Starforged {

    [DataContract]
    public class Player {

        /// <summary>
        /// The number of regular fuel the player has
        /// </summary>
        [DataMember]
        public float Fuel;

        /// <summary>
        /// The number of hyper drive fuel the player has
        /// </summary>
        [DataMember]
        public int JumpFuel;

        /// <summary>
        /// The number of components the player has
        /// </summary>
        [DataMember]
        public int Components;

        /// <summary>
        /// The number of credits the player has
        /// </summary>
        [DataMember]
        public int Credits;

        /// <summary>
        /// The number of ammunition the player has
        /// </summary>
        [DataMember]
        public int Ammo;

        /// <summary>
        /// Which level the player is on
        /// </summary>
        [DataMember]
        public int Level;

        /// <summary>
        /// The currently used ship
        /// </summary>
        [DataMember]
        public Dictionary<UpgradeType, int> ShipUpgradeLevels;

        public Player () {
            Fuel = 100f;
            JumpFuel = 20;
            Components = 20;
            Credits = 20;
            Ammo = 50;
            Level = 1;

            ShipUpgradeLevels = new Dictionary<UpgradeType, int>{{ UpgradeType.Health, 0 },
                                                             { UpgradeType.Speed, 0 },
                                                             { UpgradeType.Agility, 0 },
                                                             { UpgradeType.Damage, 0 },
                                                             { UpgradeType.ProjSpeed, 0 }};
        }

        public Player(float fuel, int jumpFuel, int components, int credits, int ammo) {
            Fuel = fuel;
            JumpFuel = jumpFuel;
            Components = components;
            Credits = credits;
            Ammo = ammo;
            Level = 1;

            ShipUpgradeLevels = new Dictionary<UpgradeType, int>{{ UpgradeType.Health, 0 },
                                                             { UpgradeType.Speed, 0 },
                                                             { UpgradeType.Agility, 0 },
                                                             { UpgradeType.Damage, 0 },
                                                             { UpgradeType.ProjSpeed, 0 }};
        }
    }
}

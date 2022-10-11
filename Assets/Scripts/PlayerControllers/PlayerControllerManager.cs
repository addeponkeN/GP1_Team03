using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllers
{
    public class PlayerControllerManager : ControllerSet
    {
        /// <summary>
        /// The player root GameObject
        /// </summary>
        public GameObject PlayerGo => Player.gameObject;
        
        public Player Player; 
        public PlayerInput Input;

        public override bool ControllersEnabled
        {
            get => base.ControllersEnabled;
            set
            {
                base.ControllersEnabled = value;
                Input.enabled = ControllersEnabled;
            }
        }

        public PlayerControllerManager(Player pl)
        {
            Player = pl;
            Input = pl.GetComponent<PlayerInput>();
            Manager = this;
        }

    }
}
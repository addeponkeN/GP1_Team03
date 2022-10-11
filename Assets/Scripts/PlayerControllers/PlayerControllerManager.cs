using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControllers
{
    public class  PlayerControllerManager : ControllerSet
    {
        [NonSerialized] public GameObject PlayerGo;

        public PlayerInput Input;

        public override bool ControllersEnabled
        {
            get => base.ControllersEnabled;
            set
            {
                base.ControllersEnabled = value;
                Input.enabled = ControllersEnabled;
            }
            //  NICE
        }

        public PlayerControllerManager(GameObject pl)
        {
            PlayerGo = pl;
            Manager = this;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Util;

namespace PlayerControllers.Controllers
{
    [Serializable]
    public class MovementController : BasePlayerController
    {
        public float Speed => _accelerator.Speed;
        public float MaxSpeed => _accelerator.MaxSpeed;

        public Accelerator Accelerator => _accelerator;

        public float SpeedMultiplier { get; set; } = 1f;
        public float MaxSpeedMultiplier { get; set; } = 1f;

        private Accelerator _accelerator;
        private PlayerStatContainer _stats;
        private InputActionReference _inputMove;

        private bool _isMoving;
        private bool _isOldMoving;

        [SerializeField] private UnityEvent onStartedMoving;
        [SerializeField] private UnityEvent onStoppedMoving;

        public override void Init()
        {
            base.Init();
            _stats = Manager.Player.Stats;

            const float acceleratorBreakSensitivity = 0.25f;

            _accelerator = new Accelerator(
                _stats.MovementAcceleration,
                _stats.MaxMoveSpeed,
                acceleratorBreakSensitivity);

            _inputMove = Manager.Player.Input.Movement;
            _inputMove.action.Enable();
        }

        public void SetMultipliers(float speed = 1f, float maxSpeed = 1f)
        {
            SpeedMultiplier = speed;
            MaxSpeedMultiplier = maxSpeed;
        }

        public override void FixedUpdate(float dt)
        {
            base.FixedUpdate(dt);
            _isOldMoving = _isMoving;

            var forwardVelocity = _inputMove.action.ReadValue<Vector2>().y;
            //  temporary permanent forward movement
            forwardVelocity = 1f;

            _isMoving = forwardVelocity != 0f;

            if(_isMoving != _isOldMoving)
            {
                if(_isMoving)
                {
                    Debug.Log("moving started");
                    onStartedMoving?.Invoke();
                }
                else
                {
                    Debug.Log("moving stopped");
                    onStoppedMoving?.Invoke();
                }
            }

            //  fetch the player stats and apply to the controller
            _accelerator.Acceleration = _stats.MovementAcceleration * SpeedMultiplier;
            _accelerator.MaxSpeed = _stats.MaxMoveSpeed * MaxSpeedMultiplier;

            _accelerator.Update(dt, forwardVelocity);

            if(_accelerator.IsAccelerating())
            {
                var body = Manager.Player.Body;
                var tf = body.transform;
                var fw = tf.forward;
                fw.y = 0;

                var move = fw * (_accelerator.Speed * dt);
                body.MovePosition(tf.position + move);
            }
        }

        public void Stop()
        {
            _accelerator.Stop();
        }
    }
}
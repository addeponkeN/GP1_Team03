using System;
using UnityEngine;
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
        private Animator _anim;

        private float _baseMaxSpeed;
        private float _animSpeed = 1f;

        public override void Init()
        {
            base.Init();

            _stats = Manager.Player.Stats;
            _baseMaxSpeed = _stats.MaxMoveSpeed;

            const float acceleratorBreakSensitivity = 0.25f;

            _accelerator = new Accelerator(
                _stats.MovementAcceleration,
                _stats.MaxMoveSpeed,
                acceleratorBreakSensitivity);

            _inputMove = Manager.Player.Input.Movement;
            _inputMove.action.Enable();
        }

        public void AssignAnimator(Animator anim, float animationSpeed = 1f)
        {
            _animSpeed = animationSpeed;
            _anim = anim;
        }

        public void SetMultipliers(float speed = 1f, float maxSpeed = 1f)
        {
            SpeedMultiplier = speed;
            MaxSpeedMultiplier = maxSpeed;
        }

        private void UpdateAnimator()
        {
            float moveSpeedPercentage = Speed / _baseMaxSpeed;
            _anim.speed = moveSpeedPercentage * _animSpeed;
        }

        public override void FixedUpdate(float dt)
        {
            base.FixedUpdate(dt);

            //  fetch the player stats and apply to the controller
            _accelerator.Acceleration = _stats.MovementAcceleration * SpeedMultiplier;
            _accelerator.MaxSpeed = _stats.MaxMoveSpeed * MaxSpeedMultiplier;

            const float forwardVelocity = 1f;
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

            if(_anim != null)
                UpdateAnimator();
        }

        public void Stop()
        {
            _accelerator.Stop();
        }
    }
}
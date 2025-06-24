using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerView : EntityView, ILook
    {
        private int _oldSector = 1;
    
        private static readonly int Direction = Animator.StringToHash("MoveDirection");
        private static readonly int Spin = Animator.StringToHash("Spin");
        private static readonly int Vel = Animator.StringToHash("Vel");

        protected override void Start()
        {
            base.Start();
            manager.HealthComponent.OnDamage += (f => ServiceLocator.Instance.GetService<CameraShake>().Shake());
        }

        public override void LookDir(Vector2 dir)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 lookDir = (mouseWorldPos - transform.position);
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            RotateSprite(lookDir);
        }

        private void RotateSprite(Vector2 lookDir)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            if (angle < 0f)
            {
                angle += 360;
            }
            int sector = Mathf.FloorToInt((angle) / 60f) % 6;

            if (sector == _oldSector)
            {
                return;
            }
            _oldSector = sector;
            animator.SetFloat(Direction,sector);
        }

        public override void PlayStateAnimation(StateEnum state)
        {
            throw new System.NotImplementedException();
        }

        public void OnAttackAnim()
        {
            animator.SetTrigger(Spin);
        }
        void OnMoveAnim()
        {
            animator.SetFloat(Vel, manager.Rb.linearVelocity.magnitude);
        }
    }
}

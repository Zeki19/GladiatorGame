using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerView : EntityView, ILook
    {
        private int _oldSector = 1;

        private static readonly int Direction = Animator.StringToHash("MoveDirection");

        protected override void Start()
        {
            base.Start();
            manager.HealthComponent.OnDamage += (f => ServiceLocator.Instance.GetService<CameraShake>().Shake());
            manager.HealthComponent.OnDamage += ChackCurrentPhace;
        }

        void ChackCurrentPhace(float not)
        {
            var playerManager = manager as PlayerManager;
            var controler = playerManager.controller as PlayerController;
            animator.SetFloat("State", controler._phaseSystem.currentPhase());
        }

        public override void LookDir(Vector2 dir)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 lookDir = (mouseWorldPos - transform.position);
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            RotateSprite(lookDir);
        }

        public override void LookDirInsta(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(Vector2.up, dir);
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = targetRotation;
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
            animator.SetFloat(Direction, sector);
        }

        public void SetAnimationBool(StateEnum state, bool boolState)
        {
            switch (state)
            {
                case StateEnum.Idle:
                    animator.SetTrigger("Idle");
                    Debug.Log("Idle");
                    break;
                case StateEnum.Dash:
                    animator.SetBool("Dash", boolState);
                    Debug.Log("Dash");
                    break;
                case StateEnum.Walk:
                    animator.SetBool("Walk", boolState);
                    Debug.Log("Walk");
                    break;
                case StateEnum.Default:
                case StateEnum.Attack:
                case StateEnum.ShortAttack:
                case StateEnum.MidAttack:
                case StateEnum.LongAttack:
                case StateEnum.ChargeAttack:
                case StateEnum.Chase:
                case StateEnum.Search:
                case StateEnum.Patrol:
                case StateEnum.Runaway:
                case StateEnum.Phase1:
                case StateEnum.Phase2:
                case StateEnum.Phase3:
                case StateEnum.BackStep:
                default:
                    break;
            }
        }

        public override void PlayStateAnimation(StateEnum state)
        {
            switch (state)
            {
                case StateEnum.Idle:
                    animator.SetTrigger("Idle");
                    break;
                case StateEnum.Dash:
                    animator.SetBool("Dash", true);
                    break;
                case StateEnum.Walk:
                    animator.SetBool("Walk", true);
                    break;
                case StateEnum.Default:
                case StateEnum.Attack:
                case StateEnum.ShortAttack:
                case StateEnum.MidAttack:
                case StateEnum.LongAttack:
                case StateEnum.ChargeAttack:
                case StateEnum.Chase:
                case StateEnum.Search:
                case StateEnum.Patrol:
                case StateEnum.Runaway:
                case StateEnum.Phase1:
                case StateEnum.Phase2:
                case StateEnum.Phase3:
                case StateEnum.BackStep:
                default:
                    break;
            }
        }

        public override void StopStateAnimation(StateEnum state)
        {
            switch (state)
            {
                case StateEnum.Idle: break;
                case StateEnum.Dash:
                    animator.SetBool("Dash", false);
                    break;
                case StateEnum.Walk:
                    animator.SetBool("Walk", false);
                    break;
                case StateEnum.Default:
                case StateEnum.Attack:
                case StateEnum.ShortAttack:
                case StateEnum.MidAttack:
                case StateEnum.LongAttack:
                case StateEnum.ChargeAttack:
                case StateEnum.Chase:
                case StateEnum.Search:
                case StateEnum.Patrol:
                case StateEnum.Runaway:
                case StateEnum.Phase1:
                case StateEnum.Phase2:
                case StateEnum.Phase3:
                case StateEnum.BackStep:
                default:
                    break;
            }
        }
    }
}
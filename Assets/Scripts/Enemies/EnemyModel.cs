using System.Collections;
using Entities;
using UnityEngine;

namespace Enemies
{
    public class EnemyModel : EntityModel
    {
        protected float SpeedModifier = 1;
        [SerializeField] float _moveSpeed;
        [SerializeField] private LayerMask obstaclesMask;

        public override void ModifySpeed(float speed)
        {
            SpeedModifier += speed;
        }

        public override void Move(Vector2 dir)
        {
            dir.Normalize();
            manager.Rb.linearVelocity = dir * (_moveSpeed * SpeedModifier);
        }

        public void Reposition(Transform target, float dashForce, LayerMask hitMask) 
        {
            RaycastHit2D hit = RaycastBetweenCharacters(transform, target, hitMask);

            if (hit.collider != null)
            {
                Vector2 direction = (target.position - transform.position).normalized;

                Vector2 perpDir = new Vector2(-direction.y, direction.x);

                var controller = manager.controller as EnemyController;
                var dashData = new DashStateData(
                    perpDir,
                    10,
                    dashForce
                );
                controller.SetStateData(EnemyStates.Dash, dashData);
                controller.ChangeToState(EnemyStates.Dash);
                //Dash(perpDir, dashForce);
            }
        }

        public RaycastHit2D RaycastBetweenCharacters(Transform start, Transform target, LayerMask hitMask = default)
        {
            Vector2 direction = (target.position - start.position);
            float distance = direction.magnitude;

            return Physics2D.Raycast(start.position, direction.normalized, distance, obstaclesMask);
        }
        


        public void AttackTarget(Transform target, float damage)
        {
            if (target == null) return;

            var manager = target.GetComponent<EntityManager>();
            if (manager != null)
            {
                manager.HealthComponent.TakeDamage(damage);
            }
        }
        #region Dash

        public override void Dash(float dashForce)
        {
            Dash(gameObject.transform.up, dashForce);
        }

        public override void Dash(Vector2 dir, float dashForce)
        {
            manager.Rb.AddForce(dir.normalized * dashForce, ForceMode2D.Impulse);
        }

        public override void Dash(Vector2 dir, float dashForce, float dashDistance)
        {
            Dash(dir, dashForce);
            StartDashMonitoring(dir.normalized, dashDistance, transform.position);
        }

        private void StartDashMonitoring(Vector2 dir, float distance, Vector2 startingPosition)
        {
            StartCoroutine(MonitorDashDistance(dir, distance, startingPosition));
        }

        private IEnumerator MonitorDashDistance(Vector2 dir, float distance, Vector2 startingPosition)
        {
            Vector2 targetPosition = startingPosition + dir * distance;
            float targetDistance = Vector2.Distance(startingPosition, targetPosition);
            while (true)
            {
                float currentDistance = Vector2.Distance(startingPosition, (Vector2)transform.position);
                if (currentDistance >= targetDistance)
                {
                    break;
                }
                yield return null;
            }

            manager.Rb.linearVelocity = Vector2.zero;

            var controler = manager.controller as EnemyController;
            controler?.SetStatus(StatusEnum.Dashing, false);
        }

        #endregion
    }
}

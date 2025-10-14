using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Attack
{
    [CreateAssetMenu(fileName = "RangedAttack", menuName = "Attacks/MeteorLikeAttack" )]
    public class MeteorLikeAttack : RangedAttack
    {
        [SerializeField] private int Radiuns;
        [SerializeField] private Vector2 FallingTimeRange;
        [SerializeField] private Vector2Int AmountRange;
        [SerializeField] private float MinimunSeparation;
        public override void ExecuteAttack()
        {
            CoroutineRunner.StartCoroutine(Attack());
        }

        protected override IEnumerator Attack()
        {
            //notNeed
            yield return 0;
            var ProjectileAmont = Random.Range(AmountRange.x, AmountRange.y);
            var Points =GetRandomPointsInCircle(Weapon.transform.position,Radiuns,ProjectileAmont,MinimunSeparation);
            foreach (var point in Points)
            {
                Shoot(point);
            }
            yield return 0;
            FinishAttack();
        }

        protected void Shoot(Vector2 point)
        {
            var projectile = ServiceLocator.Instance.GetService<ProjectileManager>()
                .GetProjectile(projectilePrefab.name);
            projectile.OnHit += Hit;
            projectile.OnReturnedToPool -= OnProjectileReturnsToPool;
            ActiveProjectiles.Add(projectile);
            if (projectile != null)
            {
                projectile.transform.position = point;
                projectile.SetUp(damage, collisionLayer, projectileSpeed,maxRange);
            }
        }
        public static List<Vector2> GetRandomPointsInCircle(Vector2 center, float radius, int count, float minSeparation)
        {
            List<Vector2> points = new List<Vector2>();
            int maxAttempts = count * 50; // safety limit to avoid infinite loops
            int attempts = 0;

            while (points.Count < count && attempts < maxAttempts)
            {
                attempts++;
                // Random point in circle (uniform distribution)
                Vector2 randomPoint = center + Random.insideUnitCircle * radius;

                // Check minimum distance to other points
                bool valid = true;
                foreach (var p in points)
                {
                    if (Vector2.Distance(p, randomPoint) < minSeparation)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                    points.Add(randomPoint);
            }

            return points;
        }
    }
}
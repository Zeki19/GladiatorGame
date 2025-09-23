using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Attack
{
    [CreateAssetMenu(fileName = "RangedAttack", menuName = "Attacks/ConeShot")]
    public class ConeShot : RangedAttack
    {
        [SerializeField,Range(0f, 360f)] private float coneAngle;
        [SerializeField] private int numberOfProjectiles;
        public override void ExecuteAttack()
        {
            CoroutineRunner.StartCoroutine(Attack());
        }
        protected override IEnumerator Attack()
        {
            //notNeed
            yield return 0;
            Shoot();
            FinishAttack();
        }

        protected void Shoot()
        {
            var projectiles = new List<BaseProjectile>();
            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var projectile = ServiceLocator.Instance.GetService<ProjectileManager>()
                    .GetProjectile(projectilePrefab.name);
                projectiles.Add(projectile);
                projectile.OnReturnedToPool -= OnProjectileReturnsToPool;
            }
            if (projectiles.Count == 0) return;
            
            var SeparationAngle=coneAngle/(numberOfProjectiles-1);
            for (int j = 0; j < numberOfProjectiles; j++)
            {
                projectiles[j].OnHit += Hit;
                projectiles[j].transform.position = Weapon.transform.position + Weapon.transform.up * .5f;
                projectiles[j].transform.rotation = 
                    (numberOfProjectiles == 1) 
                        ? Weapon.transform.rotation 
                        : Weapon.transform.rotation * Quaternion.Euler(0, 0, coneAngle / 2 - SeparationAngle * j);
                projectiles[j].SetUp(damage, collisionLayer, projectileSpeed,maxRange);
            }
        }

        
    }
}
using System.Collections;
using UnityEngine;
using Weapons;

namespace Attack
{
    [CreateAssetMenu(fileName = "RangedAttack", menuName = "Attacks/StraightShot")]
    public class StraightShot : RangedAttack
    {
        public override void ExecuteAttack()
        {
            CoroutineRunner.StartCoroutine(Attack());
        }

        protected override IEnumerator Attack()
        {
            //notNeed
            yield return 0;
            Shoot();
            yield return 0;
            FinishAttack();
        }

        protected void Shoot()
        {
            var projectile = ServiceLocator.Instance.GetService<ProjectileManager>()
                .GetProjectile(projectilePrefab.name);
            projectile.OnHit += Hit;
            projectile.OnReturnedToPool -= OnProjectileReturnsToPool;
            ActiveProjectiles.Add(projectile);
            if (projectile != null)
            {
                projectile.transform.position = Weapon.transform.position + Weapon.transform.up * .5f;
                projectile.transform.rotation = Weapon.transform.rotation;
                projectile.Configure(damage, collisionLayer, projectileSpeed,maxRange);
            }
        }
        
    }
}
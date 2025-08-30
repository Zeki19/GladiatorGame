using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public abstract class RangedAttack : BaseAttack
    {
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected float maxRange;
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected LayerMask collisionLayer;
        protected List<BaseProjectile> ActiveProjectiles = new List<BaseProjectile>() ;
        public override void OnUnequip()
        {
            base.OnUnequip();
            ClearAllProjectiles();
        }

        private void ClearAllProjectiles()
        {
            foreach (var projectile in ActiveProjectiles)
            {
                projectile.OnHit -= Hit;
                projectile.OnReturnedToPool -= OnProjectileReturnsToPool;
            }
            ActiveProjectiles.Clear();
        }

        protected void OnProjectileReturnsToPool(BaseProjectile projectile)
        {
            projectile.OnHit -= Hit;
            projectile.OnReturnedToPool -= OnProjectileReturnsToPool;
            ActiveProjectiles.Remove(projectile);
        }
    }
}
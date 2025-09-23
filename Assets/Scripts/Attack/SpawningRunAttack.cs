using System.Collections;
using UnityEngine;
using Weapons;

namespace Attack
{
    [CreateAssetMenu(fileName = "SpawningRunAttack", menuName = "Attacks/SpawningRunAttack")]
    public class SpawningRunAttack : SpawningAttack
    {
        public float distance;
        public float speed;
        [SerializeField] private float delayBetweenSwans;
        

        public override void ExecuteAttack()
        {
            Move.StopMovement();
            CoroutineRunner.StartCoroutine(Attack());
        }

        protected override IEnumerator Attack()
        {
            Weapon.SetActive(true);
            collider.enabled = true;
            var user = Move as MonoBehaviour;
            Move.Dash(user.transform.up, speed,distance);
            Status.SetStatus(StatusEnum.Dashing, true);
            var timer = delayBetweenSwans;
            while (Status.GetStatus(StatusEnum.Dashing)) 
            {
                timer-=Time.deltaTime;
                if (timer <= 0)
                {
                    Shoot();
                    timer = delayBetweenSwans;
                }
                yield return 0;
            }
            collider.enabled = false;
            FinishAttack();
        }
        protected void Shoot()
        {
            var projectile = ServiceLocator.Instance.GetService<ProjectileManager>()
                .GetProjectile(projectilePrefab.name);
            projectile.OnReturnedToPool -= OnProjectileReturnsToPool;
            ActiveProjectiles.Add(projectile);
            if (projectile != null)
            {
                projectile.transform.position = Weapon.transform.position + Weapon.transform.up * .5f;
                projectile.transform.rotation = Weapon.transform.rotation;
                projectile.SetUp(damage, collisionLayer, projectileSpeed,maxRange);
            }
        }
    }
}
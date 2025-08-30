using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Manages pools of projectile instances for efficiency.
    /// </summary>
    public class ProjectileManager : MonoBehaviour
    {

        [System.Serializable]
        public struct ProjectilePoolConfig
        {
            public string projectileType;
            public GameObject projectilePrefab;
            public int initialSize;
        }

        [Header("Projectile Pools")]
        public ProjectilePoolConfig[] projectilePools;

        private readonly Dictionary<string, Queue<BaseProjectile>> _poolDict = new();

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);

            // Initialize all pools
            foreach (var pool in projectilePools)
            {
                var newQueue = new Queue<BaseProjectile>();
                for (int i = 0; i < pool.initialSize; i++)
                {
                    var proj = CreateProjectile(pool.projectilePrefab);
                    proj.gameObject.SetActive(false);
                    newQueue.Enqueue(proj);
                }
                _poolDict[pool.projectileType] = newQueue;
            }
        }

        private BaseProjectile CreateProjectile(GameObject prefab)
        {
            var obj = Instantiate(prefab, transform);
            obj.name =prefab.name;
            var projectile = obj.GetComponent<BaseProjectile>();
            if (projectile == null)
                Debug.LogError($"Prefab {prefab.name} does not contain a BaseProjectile!");
            return projectile;
        }

        /// <summary>
        /// Gets a projectile from the specified pool.
        /// </summary>
        public BaseProjectile GetProjectile(string projectileType)
        {
            if (!_poolDict.TryGetValue(projectileType, out var pool) || pool.Count == 0)
            {
                // If pool is empty or missing, try to create one more from the correct prefab config.
                var config = System.Array.Find(projectilePools, p => p.projectileType == projectileType);
                if (config.projectilePrefab != null)
                    return CreateProjectile(config.projectilePrefab);
                Debug.LogWarning($"Unknown projectile type: {projectileType}");
                return null;
            }
            var projectile = pool.Dequeue();
            projectile.gameObject.SetActive(true);
            return projectile;
        }

        /// <summary>
        /// Returns a projectile to its pool.
        /// </summary>
        public void ReturnProjectile(string projectileType, BaseProjectile projectile)
        {
            projectile.ResetState();
            projectile.gameObject.SetActive(false);
            if (!_poolDict.TryGetValue(projectileType, out var pool))
            {
                pool = new Queue<BaseProjectile>();
                _poolDict[projectileType] = pool;
            }
            pool.Enqueue(projectile);
        }
    }
}

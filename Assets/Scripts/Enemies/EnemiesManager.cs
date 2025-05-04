using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemiesManager : MonoBehaviour
    {
        private Dictionary<GameObject, EnemyManager> _enemies = new Dictionary<GameObject, EnemyManager>();

        private void Awake()
        {
            ServiceLocator.Instance.RegisterService(this);
        }

        public void RegisterEnemy(GameObject enemy, EnemyManager enemyManager) =>
            _enemies.Add(enemy, enemyManager);

        public EnemyManager GetManager(GameObject enemy)
        {
            return _enemies.GetValueOrDefault(enemy);
        }
    }
}

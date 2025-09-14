using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    
    [Space]
    
    [Header("Food config")]
    [SerializeField] private List<SOFood> foodTypes;
    [Tooltip("Maximum number of foods allowed at the same time")]
    [SerializeField] private int foodAmount;
    
    [Space]
    
    [Header("Spawn config")] 
    [SerializeField] private List<Vector2> spawnPoints;
    [Tooltip("Maximum wait time for food spawn")]
    [SerializeField] private float waitTime;
    
    private readonly HashSet<Vector2> _usedPoints = new HashSet<Vector2>();
    private Food[] _instancedFoods;
    private int _nFoodsOnScene;
    private bool _spawning;
    
    private void Start()
    {
        if (foodAmount == 0 || foodAmount > spawnPoints.Count) enabled = false;
        
        _instancedFoods = new Food[foodAmount];
        
        InstanceFood();
    }
    private void InstanceFood()
    {
        for (int i = 0; i < foodAmount; i++)
        {
            var go = Instantiate(foodPrefab, transform.position, Quaternion.identity, this.transform);
            var script = go.GetComponent<Food>();
            
            script.OnPickUp += FoodDestroyed;
            
            _instancedFoods[i] = script;
        }
    }
    private void Update()
    {
        if (_nFoodsOnScene >= foodAmount || _spawning) return;
        
        _spawning = true;
        var rTime = Random.Range(0f, waitTime);
        StartCoroutine(Timer(rTime));
    }
    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnFood();
        _spawning = false;
    }
    private void SpawnFood()
    {
        var spawnPoint = GetRandomPoint();
        var foodType = GetFoodType();
        
        foreach (var f in _instancedFoods)
        {
            if (f.IsActive) continue;
            
            f.Initialize(foodType, spawnPoint);
            _nFoodsOnScene++;
            return;
        }
    }
    private void FoodDestroyed(Vector2 spawnPoint)
    {
        _usedPoints.Remove(spawnPoint);
        _nFoodsOnScene--;
    }

    #region Utility
        private Vector2 GetRandomPoint()
        {
            var freePoints = new List<Vector2>(spawnPoints);

            foreach (var p in  _usedPoints)
            {
                freePoints.Remove(p);
            }
            
            var point = freePoints[Random.Range(0, freePoints.Count)];
            _usedPoints.Add(point);
            return point;
        }

        private SOFood GetFoodType()
        {
            return foodTypes[Random.Range(0, foodTypes.Count)];
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (var p in spawnPoints)
            {
                Gizmos.DrawSphere((Vector2)p,.5f);
            }
        }

        #endregion

}

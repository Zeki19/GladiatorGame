using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EnemyTelegraph", menuName = "Scriptable Objects/EnemyTelegraph")]
public class EnemyTelegraph : ScriptableObject
{
    [SerializeField] private GameObject telegraphPrefab;
    [Serializable]
    public struct spriteValues
    {
        public Shapes shape;
        public Sprite sprite;
    }

    [Serializable]
    public struct telegraphValues
    {
        public string attackName;
        public Shapes shape;
        public Vector2 scale;
        public float aliveTime;
    }

    public List<spriteValues> correlations = new List<spriteValues>();

    public List<telegraphValues> attacks = new List<telegraphValues>();

    Dictionary<Shapes, Sprite> Sprites;

    private void OnEnable()
    {
        Sprites = new Dictionary<Shapes, Sprite>();

        foreach (spriteValues s in correlations)
        {
            if (s.sprite != null && !Sprites.ContainsKey(s.shape))
            {
                Sprites.Add(s.shape, s.sprite);
            }
        }
        if (telegraphPrefab == null)
        {
            Debug.LogWarning("TelegraphPrefab is NULL");
        }
    }

    public void InstantiateTelegraph(Vector2 telePosition, Quaternion teleQuaternion, Shapes teleShape, string name)
    {
        telegraphValues teleAttack = default;
        bool foundTele = false;

        foreach(telegraphValues tv in attacks)
        {
            if (tv.attackName == name)
            {
                teleAttack = tv;
                foundTele = true;
            }
        }

        if (!foundTele)
        {
            return;
        }

        GameObject go = new GameObject("Telegraph");

        go.transform.position = telePosition;
        go.transform.rotation = teleQuaternion;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = Sprites[teleShape];

        go.AddComponent<Telegraph>().StartTelegraph(sr, teleAttack.aliveTime);

    }
    
}


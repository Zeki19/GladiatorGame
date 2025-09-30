using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTelegraph", menuName = "Scriptable Objects/EnemyTelegraph")]
public class EnemyTelegraph : ScriptableObject
{
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
        public bool center;
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
    }

    public bool InstantiateTelegraph(Vector2 telePosition, Quaternion teleQuaternion, string name)
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
            Debug.LogWarning("Telegraph name " + name + " not found.");
            return false;
        }

        GameObject go = new GameObject("Telegraph");

        go.transform.position = telePosition;
        go.transform.rotation = teleQuaternion;
        go.transform.localScale = new Vector3(teleAttack.scale.x, teleAttack.scale.y, 1f);

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = Sprites[teleAttack.shape];
        sr.sortingLayerName = "Background";
        if (!teleAttack.center) 
        {
            AdjustSpriteLocation(go, sr, teleAttack);
        }
        go.AddComponent<Telegraph>().StartTelegraph(sr, teleAttack.aliveTime);
        return true;
    }

    private void AdjustSpriteLocation(GameObject go, SpriteRenderer sr, telegraphValues teleAttack)
    {

        float rawHeight = sr.sprite.bounds.size.y;
        float scaledHeight = rawHeight * go.transform.localScale.y;
        Vector3 localOffset = new Vector3(0, scaledHeight / 2f, 0);

        go.transform.position += go.transform.rotation * localOffset;
    }

    public telegraphValues GetTelegraph(string name)
    {
        foreach (telegraphValues tv in attacks)
        {
            if (tv.attackName == name)
            {
                return tv;
            }
        }
        return default;
    }

    public enum Shapes
    {
        circle,
        square,
        triangle,
        semiCircle
    }
}


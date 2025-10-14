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

        //This three work only for multiple telegraphs.
        public bool multiple;
        public int amount;
        [SerializeField, Range(0f, 360f)] public float coneAngle;
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

        foreach (telegraphValues tv in attacks)
        {
            if (tv.attackName == name)
            {
                teleAttack = tv;
                foundTele = true;
                break;
            }
        }

        if (!foundTele)
        {
            Debug.LogWarning("Telegraph name " + name + " not found.");
            return false;
        }

        if (teleAttack.multiple)
        {
            HandleMultiple(teleAttack, telePosition, teleQuaternion);
            return true;
        }

        CreateTelegraph(teleAttack, telePosition, teleQuaternion);
        return true;
    }

    private void AdjustSpriteLocation(GameObject go, SpriteRenderer sr, telegraphValues teleAttack)
    {

        float rawHeight = sr.sprite.bounds.size.y;
        float scaledHeight = rawHeight * go.transform.localScale.y;
        Vector3 localOffset = new Vector3(0, scaledHeight / 2f, 0);

        go.transform.position += go.transform.rotation * localOffset;
    }

    private void HandleMultiple(telegraphValues teleAttack, Vector2 telePosition, Quaternion teleQuaternion)
    {
        if (teleAttack.amount <= 1)
        {
            CreateTelegraph(teleAttack, telePosition, teleQuaternion);
            return;
        }

        float halfCone = teleAttack.coneAngle / 2f;
        float angleStep = teleAttack.coneAngle / (teleAttack.amount - 1);

        for (int i = 0; i < teleAttack.amount; i++)
        {
            float angleOffset = -halfCone + (angleStep * i);
            Quaternion rotation = teleQuaternion * Quaternion.Euler(0f, 0f, angleOffset);
            CreateTelegraph(teleAttack, telePosition, rotation);
        }
    }

    private void CreateTelegraph(telegraphValues teleAttack, Vector2 telePosition, Quaternion teleQuaternion)
    {
        GameObject go = new GameObject("Telegraph");
        go.transform.position = telePosition;
        go.transform.rotation = teleQuaternion;
        go.transform.localScale = new Vector3(teleAttack.scale.x, teleAttack.scale.y, 1f);

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = Sprites[teleAttack.shape];
        sr.sortingLayerName = "Background";

        if (!teleAttack.center)
            AdjustSpriteLocation(go, sr, teleAttack);

        go.AddComponent<Telegraph>().StartTelegraph(sr, teleAttack.aliveTime);
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


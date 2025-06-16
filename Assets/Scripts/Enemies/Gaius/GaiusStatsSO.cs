using UnityEngine;

[CreateAssetMenu(fileName = "GaiusStats", menuName = "Scriptable Objects/Enemies/Gaius")]
public class GaiusStatsSO : ScriptableObject
{
    [Header("Short Attack")]
    [SerializeField] public float shortDelay;
    [SerializeField] public float shortDamage;
    [SerializeField] public float shortPunish;

    [Header("Medium Attack")]
    [SerializeField] public float mediumDelay;
    [SerializeField] public float mediumDamage;
    [SerializeField] public float mediumPunish;

    [Header("Long Attack")]
    [SerializeField] public float longDelay;
    [SerializeField] public float longDamage;
    [SerializeField] public float longPunish;

    private void OnValidate()
    {
        shortDelay = Mathf.Max(shortDelay, 0);
        mediumDelay = Mathf.Max(mediumDelay, 0);
        longDelay = Mathf.Max(longDelay, 0);

        shortDamage = Mathf.Max(shortDamage, 0);
        mediumDamage = Mathf.Max(mediumDamage, 0);
        longDamage = Mathf.Max(longDamage, 0);
        
        shortPunish = Mathf.Max(shortPunish, 0);
        mediumPunish = Mathf.Max(mediumPunish, 0);
        longPunish = Mathf.Max(longPunish, 0);
    }
}

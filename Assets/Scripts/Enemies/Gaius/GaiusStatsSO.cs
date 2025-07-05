using UnityEngine;

[CreateAssetMenu(fileName = "GaiusStats", menuName = "Scriptable Objects/Enemies/Gaius")]
public class GaiusStatsSO : ScriptableObject
{
    [Header("Short Attack")]
    [SerializeField] public float shortAngle;
    [SerializeField] public float shortDelay;
    [SerializeField] public float shortDamage;
    [SerializeField] public float shortPunish;
    [SerializeField] public LayerMask shortTargetLayer;


    [Header("Medium Attack")]
    [SerializeField] public float mediumRange;
    [SerializeField] public float mediumWidth;
    [SerializeField] public float mediumDelay;
    [SerializeField] public float mediumDamage;
    [SerializeField] public float mediumPunish;
    [SerializeField] public LayerMask mediumTargetLayer;


    [Header("Long Attack")]
    [SerializeField] public float longRange;
    [SerializeField] public float longDelay;
    [SerializeField] public float longDamage;
    [SerializeField] public float longPunish;
    [SerializeField] public float longSpeed;
    [SerializeField] public float longDuration;
    [SerializeField] public float longIntervalCheck;
    [SerializeField] public LayerMask longTargetLayer;


    [Header("Obstacle Avoidance Settings")]
    [SerializeField] public int maxObs;
    [SerializeField] public float radius;
    [SerializeField] public float angleOfVision;
    [SerializeField] public float personalArea;
    [SerializeField] public LayerMask obsMask;


    private void OnValidate()
    {
        mediumRange = Mathf.Clamp(mediumRange, 0, longRange);
        longRange = Mathf.Max(longRange, mediumRange);

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

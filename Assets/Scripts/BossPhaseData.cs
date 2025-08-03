using UnityEngine;

public class BossPhaseData
{
    public string phaseName;
    public Sprite bossImage;
    public Color barColor = Color.red;
    [Range(0f, 100f)] public float triggerHealthPercent;
}
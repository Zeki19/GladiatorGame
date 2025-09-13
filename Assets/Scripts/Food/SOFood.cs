using UnityEngine;
[CreateAssetMenu(fileName = "FoodType",menuName = "New Food/Food")]
public class SOFood : ScriptableObject
{
    public string foodType;
    public Sprite foodSprite;
    public float healingAmount;
}

using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerWeaponSpriteChange : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Subscribe()
    {
        ServiceLocator.Instance.GetService<PlayerWeaponController>().OnWeaponLossDurability+=ChangeSprite;
    }
    public void UnSubscribe()
    {
        ServiceLocator.Instance.GetService<PlayerWeaponController>().OnWeaponLossDurability-=ChangeSprite;
    }

    private void ChangeSprite(float durability)
    {
        switch (durability)
        {
            case >= 0.66f: spriteRenderer.sprite = sprites[0]; break;
            case >= 0.33f: spriteRenderer.sprite = sprites[1]; break;
            case > 0f: spriteRenderer.sprite = sprites[2]; break;
            case <= 0f: spriteRenderer.sprite = sprites[0]; break;
        }
    }
    
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private RectTransform cursorRoot;
    [SerializeField] private Image shadowImage;
    [SerializeField] private Image cursorImage;
    
    [Header("Data input")]
    [SerializeField] private List<CursorProfileSO> profiles;
    private CursorProfileSO _currentProfile;
    
    private void Awake()
    {
        cursorImage.gameObject.SetActive(true);
        shadowImage.gameObject.SetActive(true);
        
        ApplyProfile(profiles[0]);

        Cursor.visible = false;
    }
    void Update()
    {
        Vector2 p;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)cursorRoot.parent, Input.mousePosition, null, out p);

        cursorRoot.anchoredPosition = p;

        if (Input.GetMouseButtonDown(0))
        {
            RotateCursor(true);
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            RotateCursor(false);
        }
    }
    private Sprite ToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
    }
    private void ApplyProfile(CursorProfileSO profile)
    {
        if (_currentProfile == profile) return;
        
        cursorRoot.localScale = new Vector3(1f,1f,1f) * profile.cursorScale;
        
        if (!profile.mainTexture)
        {
            Cursor.visible = true;
            Debug.Log("Missing mainTexture on the CursorProfile");
            return;
        }
        
        //Main cursor
        cursorImage.sprite = ToSprite(profile.mainTexture);
        
        //Shadow cursor
        shadowImage.sprite = ToSprite(profile.mainTexture);
        shadowImage.rectTransform.anchoredPosition = profile.shadowOffset;
        shadowImage.color = profile.shadowColor;
        
        _currentProfile = profile;
    }
    public void ChangeProfile(String cursorProfileName)
    {
        foreach (var cursor in profiles)
        {
            if (cursor.name == cursorProfileName)
            {
                ApplyProfile(cursor);
            }
        }
    }
    private void RotateCursor(bool value)
    {
        if (value)
        {
            cursorImage.transform.rotation = Quaternion.Euler(0, 0, 30);
            shadowImage.transform.rotation = Quaternion.Euler(0, 0, 10);
        } 
        else
        {
            cursorImage.transform.rotation = Quaternion.Euler(0, 0, 0);
            shadowImage.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void SecondaryTexture(bool value)
    {
        if (!_currentProfile.secondaryTexture) return;

        cursorImage.sprite = ToSprite(value ? _currentProfile.secondaryTexture : _currentProfile.mainTexture);
        shadowImage.sprite = ToSprite(value ? _currentProfile.secondaryTexture : _currentProfile.mainTexture);
    }
}

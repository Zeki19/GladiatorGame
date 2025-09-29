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
            cursorImage.sprite = ToSprite(_currentProfile.clickTexture);
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            cursorImage.sprite = ToSprite(_currentProfile.mainTexture);
        }
    }
    private Sprite ToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
    }
    private void ApplyProfile(CursorProfileSO profile)
    {
        cursorRoot.localScale *= profile.cursorScale;
        
        if (profile.mainTexture == null)
        {
            Cursor.visible = true;
            return;
        }
        
        //Main cursor
        if (cursorImage.enabled) cursorImage.sprite = ToSprite(profile.mainTexture);
        
        //Shadow cursor
        if(profile.shadowTexture == null) return;
        shadowImage.sprite = ToSprite(profile.shadowTexture);
        shadowImage.rectTransform.anchoredPosition = profile.shadowOffset;
        shadowImage.color = profile.shadowColor;
        
        //Set variable
        _currentProfile = profile; //This should be moved because if there is no shadow there wont be a currentprofile.
    }
    public void ChangeCursor(String cursorProfileName)
    {
        foreach (var cursor in profiles)
        {
            if (cursor.name == cursorProfileName)
            {
                ApplyProfile(cursor);
            }
        }
    }
}

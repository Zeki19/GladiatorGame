using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Cursor_Controller : MonoBehaviour
{
    [SerializeField] private RectTransform _cursorRoot;
    [SerializeField] private Image _cursorImage;
    [SerializeField] private Image _shadowImage;
    [SerializeField] private List<SO_CursorType> _cursorList;
    private SO_CursorType _currentCursor;
    
    private void Awake()
    {
        Cursor.visible = false;
        ApplyCursor(_cursorList[0]);

        _cursorImage.gameObject.SetActive(true);
        _shadowImage.gameObject.SetActive(true);
    }
    private void Update()
    {
        // p = posicion del cursor
        Vector2 p;
        //toma la posicion del mouse en base al canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_cursorRoot.parent, Input.mousePosition, null, out p);
        //actualizar la posicion del mouse
        _cursorRoot.anchoredPosition = p;

        if(Input.GetMouseButtonDown(0))
        {
            _cursorImage.sprite = TOsprite(_currentCursor.clickTexture);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _cursorImage.sprite = TOsprite(_currentCursor.mainTexture);
        }
    }
    private void ApplyCursor(SO_CursorType Type)
    {
        _currentCursor = Type;

        _cursorRoot.localScale *= Type.cursorScale;

        if (Type.mainTexture == null)
        {
            Cursor.visible = true;
            Debug.Log("no cursor applied");
            return;
        }
        _cursorImage.sprite = TOsprite(Type.mainTexture);

        if (Type.shadowTexture == null) return;

        _shadowImage.sprite = TOsprite(Type.shadowTexture);

        _shadowImage.rectTransform.anchoredPosition = Type.shadowOffset;
        _shadowImage.color = Type.shadowColour;
    }
    private Sprite TOsprite(Texture2D texture) 
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f); 
    }
    public void ChangeCursor(string cursorName)
    {
        foreach (var cursor in _cursorList)
        {
            if (cursor.name == cursorName)
            {
                ApplyCursor(cursor);
            }
        }
    }
}

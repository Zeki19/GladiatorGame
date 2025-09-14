using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MenuManager : MonoBehaviour
{
    [Tooltip("The first item on the list is the start screen")]
    [SerializeField] private List<GameObject> menuList = new List<GameObject>();

    private GameObject _currentScreen;

    private void Awake()
    {
        foreach (var go in menuList) go.SetActive(false);
    }
    private void Start()
    {
        menuList[0].SetActive(true);
        _currentScreen = menuList[0];
    }
    public void ChangeScreen(string screenName)
    {
        _currentScreen.SetActive(false);

        foreach (var go in menuList.Where(go => go.name == screenName))
        {
            go.SetActive(true);
            _currentScreen = go;
        }
    }
    public void Quit() 
    {
        Application.Quit();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Tooltip("The first item on the list is the start screen")]
    [SerializeField] private List<GameObject> menuList = new List<GameObject>();

    [Header("Button References (Optional)")]
    [SerializeField] private Button continueButton; 

    private GameObject _currentScreen;

    private void Awake()
    {
        foreach (var go in menuList) go.SetActive(false);
    }

    private void Start()
    {
        foreach (var item in menuList)
        {
            item.SetActive(false);
        }

        menuList[0].SetActive(true);
        _currentScreen = menuList[0];

        UpdateContinueButton();
    }

    private void UpdateContinueButton()
    {
        if (continueButton != null)
        {
            continueButton.interactable = SaveManager.Instance.HasSaveData();
        }
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

    public void StartNewGame()
    {
        SaveManager.Instance.StartNewGame();

        SaveData saveData = SaveManager.Instance.GetCurrentSaveData();

        ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(saveData.nextSceneToLoad);
    }

    public void ContinueGame()
    {
        SaveData saveData = SaveManager.Instance.GetCurrentSaveData();

        if (saveData != null)
        {
            Debug.Log($"Continue: Loading scene {saveData.nextSceneToLoad}, Training Mode: {saveData.isTrainingMode}");
            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(saveData.nextSceneToLoad);
        }
        else
        {
            Debug.LogWarning("No save data found! Starting new game instead.");
            StartNewGame();
        }
    }

    public void DeleteAllSaves()
    {
        SaveManager.Instance.DeleteAllSaves();
        UpdateContinueButton();
        Debug.Log("All saves deleted from menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
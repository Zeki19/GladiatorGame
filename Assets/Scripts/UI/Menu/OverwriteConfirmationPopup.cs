using UnityEngine;
using UnityEngine.UI;

public class OverwriteConfirmationPopup : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private GameObject popupPanel;

    private MenuManager _menuManager;

    private void Awake()
    {
        if (yesButton != null)
        {
            yesButton.onClick.AddListener(OnYesClicked);
        }

        if (noButton != null)
        {
            noButton.onClick.AddListener(OnNoClicked);
        }

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Initialize(MenuManager menuManager)
    {
        _menuManager = menuManager;
    }

    public void ShowPopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void HidePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnYesClicked()
    {
        Debug.Log("User confirmed overwrite");
        HidePopup();
        
        if (_menuManager != null)
        {
            _menuManager.ConfirmNewGame();
        }
    }

    private void OnNoClicked()
    {
        Debug.Log("User cancelled overwrite");
        HidePopup();
    }

    private void OnDestroy()
    {
        if (yesButton != null)
        {
            yesButton.onClick.RemoveListener(OnYesClicked);
        }

        if (noButton != null)
        {
            noButton.onClick.RemoveListener(OnNoClicked);
        }
    }
}
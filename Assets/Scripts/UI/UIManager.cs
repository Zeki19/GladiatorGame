using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject playerUI;
    public GameObject enemyUI;
    
    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    public void HideUI()
    {
        playerUI.SetActive(false);
        enemyUI.SetActive(false);
    }

    public void ShowUI()
    {
        playerUI.SetActive(true);
        enemyUI.SetActive(true);
    }
}

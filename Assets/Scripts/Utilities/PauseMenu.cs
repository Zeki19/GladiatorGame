using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities
{
    public class PauseMenu : MonoBehaviour
    {
        private bool _isGamePause;
        [SerializeField] private GameObject uiPauseMenu;
        
        [SerializeField]private InputActionAsset inputActions;
        [SerializeField]private string playerActionMapName = "Player";
        private InputActionMap playerActionMap;

        private void Start()
        {
            playerActionMap = inputActions.FindActionMap(playerActionMapName);

            if (playerActionMap == null)
            {
                Debug.LogError($"Player Action Map '{playerActionMapName}' not found!");
                return;
            }

        }

        public void TogglePause(InputAction.CallbackContext context)
        {
            if (context.started)
                TogglePause();
        }
        public void TogglePause()
        {
            PauseGame(!_isGamePause);
        }
        private void PauseGame(bool state)
        {
            var timeScale = state ?  0 : 1;
            Time.timeScale = timeScale;
            //EnablePlayerInput(!state);
            uiPauseMenu.SetActive(state);
            _isGamePause = state;
        }
        private void EnablePlayerInput(bool enable)
        {
            if (enable)
            {
                playerActionMap.Enable();
            }
            else
            {
                playerActionMap.Disable();
            }
        }
    }
}

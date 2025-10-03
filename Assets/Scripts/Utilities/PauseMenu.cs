using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities
{
    public class PauseMenu : MonoBehaviour
    {
        private bool _isGamePaused = false;
        private bool _isSettings;

        [Header("GameObjects")]
        [SerializeField] private GameObject Menu;
        [SerializeField] private GameObject Settings;

        [Header("Config")]
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
            if (context.started) TogglePause();
        }

        public void TogglePause()
        {
            PauseGame(!_isGamePaused);
        }

        private void PauseGame(bool state)
        {
            //Use PAUSE SYSTEM
            var timeScale = state ?  0 : 1;
            Time.timeScale = timeScale;

            //EnablePlayerInput(!state);

            Menu.SetActive(state);
            _isGamePaused = state;
            SettingsMenu(!state);
        }

        public void ToggleSettings()
        {
            SettingsMenu(!_isSettings);
        }

        private void SettingsMenu(bool state)
        {
            if (!_isGamePaused)
            {
                Settings.SetActive(false);
                _isSettings = false;
                return;
            }

            Settings.SetActive(state);
            _isSettings = state;
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

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }

        public void Quit()
        {
            Debug.Log("Missing POP-UP screen");
            //Show pop up

            ServiceLocator.Instance.GetService<SceneChanger>().ChangeScene(0);
        }

    }
}

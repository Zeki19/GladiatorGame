using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities
{
    public class PauseMenu : MonoBehaviour, IPausable
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
            PauseManager.OnPauseStateChanged += HandlePause;
        }
        void OnDisable()
        {
            PauseManager.OnPauseStateChanged -= HandlePause;
        }


        public void TogglePause(InputAction.CallbackContext context)
        {
            if (context.started) TogglePause();    
        }

        public void TogglePause()
        {
            PauseManager.TogglePause();
        }

        private void PauseGame()
        {
            EnablePlayerInput(!_isGamePaused);

            Menu.SetActive(_isGamePaused);
            SettingsMenu(!_isGamePaused);
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

        private void HandlePause(bool paused)
        {
            _isGamePaused = paused;
            if (paused)
                OnPause();
            else
                OnResume();
        }

        public void OnPause()
        {
            PauseGame();
        }

        public void OnResume()
        {
            PauseGame();
        }
    }
}

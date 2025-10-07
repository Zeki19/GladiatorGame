using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// A reusable component with a self-contained UI for rebinding a single action.
    /// Uses TextMeshPro (TMP_Text) for all text fields.
    /// </summary>
    public class RebindActionUI : MonoBehaviour
    {
        public InputActionReference actionReference
        {
            get => m_Action;
            set { m_Action = value; UpdateActionLabel(); UpdateBindingDisplay(); }
        }

        public string bindingId
        {
            get => m_BindingId;
            set { m_BindingId = value; UpdateBindingDisplay(); }
        }

        public InputBinding.DisplayStringOptions displayStringOptions
        {
            get => m_DisplayStringOptions;
            set { m_DisplayStringOptions = value; UpdateBindingDisplay(); }
        }

        /// <summary> Text that receives the action name (optional). </summary>
        public TMP_Text actionLabel
        {
            get => m_ActionLabel;
            set { m_ActionLabel = value; UpdateActionLabel(); }
        }

        /// <summary>
        /// Text that receives the binding display string (optional; if null, rely on updateBindingUIEvent).
        /// </summary>
        public TMP_Text bindingText
        {
            get => m_BindingText;
            set { m_BindingText = value; UpdateBindingDisplay(); }
        }

        /// <summary> Optional text shown while waiting for input. </summary>
        public TMP_Text rebindPrompt
        {
            get => m_RebindText;
            set => m_RebindText = value;
        }

        /// <summary> Optional overlay shown during interactive rebind. </summary>
        public GameObject rebindOverlay
        {
            get => m_RebindOverlay;
            set => m_RebindOverlay = value;
        }

        public UpdateBindingUIEvent updateBindingUIEvent
        {
            get { if (m_UpdateBindingUIEvent == null) m_UpdateBindingUIEvent = new UpdateBindingUIEvent(); return m_UpdateBindingUIEvent; }
        }

        public InteractiveRebindEvent startRebindEvent
        {
            get { if (m_RebindStartEvent == null) m_RebindStartEvent = new InteractiveRebindEvent(); return m_RebindStartEvent; }
        }

        public InteractiveRebindEvent stopRebindEvent
        {
            get { if (m_RebindStopEvent == null) m_RebindStopEvent = new InteractiveRebindEvent(); return m_RebindStopEvent; }
        }

        public InputActionRebindingExtensions.RebindingOperation ongoingRebind => m_RebindOperation;

        public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
        {
            bindingIndex = -1;

            action = m_Action?.action;
            if (action == null) return false;
            if (string.IsNullOrEmpty(m_BindingId)) return false;

            var bindingGuid = new Guid(m_BindingId);
            bindingIndex = action.bindings.IndexOf(x => x.id == bindingGuid);
            if (bindingIndex == -1)
            {
                Debug.LogError($"Cannot find binding with ID '{bindingGuid}' on '{action}'", this);
                return false;
            }

            return true;
        }

        public void UpdateBindingDisplay()
        {
            var displayString = string.Empty;
            string deviceLayoutName = default;
            string controlPath = default;

            var action = m_Action?.action;
            if (action != null)
            {
                var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == m_BindingId);
                if (bindingIndex != -1)
                    displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, displayStringOptions);
            }

            if (m_BindingText != null)
                m_BindingText.text = displayString;

            m_UpdateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath);
        }

        public void ResetToDefault()
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex)) return;

            if (action.bindings[bindingIndex].isComposite)
            {
                for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                    action.RemoveBindingOverride(i);
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex);
            }

            UpdateBindingDisplay();
        }

        public void StartInteractiveRebind()
        {
            m_Action.action.Disable();

            if (!ResolveActionAndBinding(out var action, out var bindingIndex)) return;

            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true);
            }
            else
            {
                PerformInteractiveRebind(action, bindingIndex);
            }
        }

        private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            m_RebindOperation?.Cancel();

            void CleanUp()
            {
                m_RebindOperation?.Dispose();
                m_RebindOperation = null;

                m_Action.action.Enable();
                SaveActionBinding();

                action.actionMap.Enable();
                m_UIInputActionMap?.Enable();
            }

            action.actionMap.Disable();
            m_UIInputActionMap?.Disable();

            m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .OnCancel(operation =>
                {
                    m_RebindStopEvent?.Invoke(this, operation);
                    if (m_RebindOverlay != null) m_RebindOverlay.SetActive(false);
                    UpdateBindingDisplay();
                    CleanUp();
                })
                .OnComplete(operation =>
                {
                    if (m_RebindOverlay != null) m_RebindOverlay.SetActive(false);
                    m_RebindStopEvent?.Invoke(this, operation);
                    UpdateBindingDisplay();
                    CleanUp();

                    if (allCompositeParts)
                    {
                        var next = bindingIndex + 1;
                        if (next < action.bindings.Count && action.bindings[next].isPartOfComposite)
                            PerformInteractiveRebind(action, next, true);
                    }
                });

            string partName = default;
            if (action.bindings[bindingIndex].isPartOfComposite)
                partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

            m_RebindOverlay?.SetActive(true);
            if (m_RebindText != null)
            {
                var text = !string.IsNullOrEmpty(m_RebindOperation.expectedControlType)
                    ? $"{partName}Waiting for {m_RebindOperation.expectedControlType} input..."
                    : $"{partName}Waiting for input...";
                m_RebindText.text = text;
            }

            if (m_RebindOverlay == null && m_RebindText == null && m_RebindStartEvent == null && m_BindingText != null)
                m_BindingText.text = "<Waiting...>";

            m_RebindStartEvent?.Invoke(this, m_RebindOperation);
            m_RebindOperation.Start();
        }

        protected void OnEnable()
        {
            if (s_RebindActionUIs == null)
                s_RebindActionUIs = new List<RebindActionUI>();
            s_RebindActionUIs.Add(this);
            if (s_RebindActionUIs.Count == 1)
                InputSystem.onActionChange += OnActionChange;
            if (m_DefaultInputActions != null && m_UIInputActionMap == null)
                m_UIInputActionMap = m_DefaultInputActions.FindActionMap("UI");
        }

        protected void OnDisable()
        {
            m_RebindOperation?.Dispose();
            m_RebindOperation = null;

            s_RebindActionUIs.Remove(this);
            if (s_RebindActionUIs.Count == 0)
            {
                s_RebindActionUIs = null;
                InputSystem.onActionChange -= OnActionChange;
            }
        }

        private static void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.BoundControlsChanged) return;

            var action = obj as InputAction;
            var actionMap = action?.actionMap ?? obj as InputActionMap;
            var actionAsset = actionMap?.asset ?? obj as InputActionAsset;

            for (var i = 0; i < s_RebindActionUIs.Count; ++i)
            {
                var component = s_RebindActionUIs[i];
                var referencedAction = component.actionReference?.action;
                if (referencedAction == null) continue;

                if (referencedAction == action ||
                    referencedAction.actionMap == actionMap ||
                    referencedAction.actionMap?.asset == actionAsset)
                    component.UpdateBindingDisplay();
            }
        }

        [Tooltip("Reference to action that is to be rebound from the UI.")]
        [SerializeField] private InputActionReference m_Action;

        [SerializeField] private string m_BindingId;
        [SerializeField] private InputBinding.DisplayStringOptions m_DisplayStringOptions;

        [Tooltip("TMP label that will receive the name of the action. Optional.")]
        [SerializeField] private TMP_Text m_ActionLabel;

        [Tooltip("TMP label that will receive the current, formatted binding string.")]
        [SerializeField] private TMP_Text m_BindingText;

        [Tooltip("Optional UI shown while a rebind is in progress.")]
        [SerializeField] private GameObject m_RebindOverlay;

        [Tooltip("Optional TMP label updated with prompt while waiting for input.")]
        [SerializeField] private TMP_Text m_RebindText;

        [Tooltip("Optional reference to default input actions containing the UI action map. The UI action map is disabled when rebinding is in progress.")]
        [SerializeField] private InputActionAsset m_DefaultInputActions;
        private InputActionMap m_UIInputActionMap;

        [Tooltip("Event triggered when the binding display should be updated (e.g., to drive custom visuals).")]
        [SerializeField] private UpdateBindingUIEvent m_UpdateBindingUIEvent;

        [Tooltip("Event triggered when an interactive rebind starts.")]
        [SerializeField] private InteractiveRebindEvent m_RebindStartEvent;

        [Tooltip("Event triggered when an interactive rebind completes or is aborted.")]
        [SerializeField] private InteractiveRebindEvent m_RebindStopEvent;

        private InputActionRebindingExtensions.RebindingOperation m_RebindOperation;
        private static List<RebindActionUI> s_RebindActionUIs;

#if UNITY_EDITOR
        protected void OnValidate()
        {
            UpdateActionLabel();
            UpdateBindingDisplay();
        }
#endif

        private void UpdateActionLabel()
        {
            if (m_ActionLabel != null)
            {
                var action = m_Action?.action;
                m_ActionLabel.text = action != null ? action.name : string.Empty;
            }
        }

        private void Start()
        {
            LoadActionBinding();
        }

        private void SaveActionBinding()
        {
            var currentBinding = actionReference.action.actionMap.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(m_Action.action.name + bindingId, currentBinding);
        }

        private void LoadActionBinding()
        {
            var savedBindings = PlayerPrefs.GetString(m_Action.action.name + bindingId);
            if (!string.IsNullOrEmpty(savedBindings))
                actionReference.action.actionMap.LoadBindingOverridesFromJson(savedBindings);
        }

        [Serializable]
        public class UpdateBindingUIEvent : UnityEvent<RebindActionUI, string, string, string> { }

        [Serializable]
        public class InteractiveRebindEvent : UnityEvent<RebindActionUI, InputActionRebindingExtensions.RebindingOperation> { }
    }
}

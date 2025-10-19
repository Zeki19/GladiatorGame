using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Editor utility to quickly create and set up the Tutorial Quest Panel UI.
/// </summary>
public class TutorialQuestPanelSetup : EditorWindow
{
    [MenuItem("Tools/Tutorial/Setup Quest Panel UI")]
    public static void ShowWindow()
    {
        GetWindow<TutorialQuestPanelSetup>("Quest Panel Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tutorial Quest Panel Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "This tool will create a complete Tutorial Quest Panel UI setup including:\n" +
            "- Quest Entry Prefab\n" +
            "- Quest Panel with proper layout\n" +
            "- All necessary components configured",
            MessageType.Info
        );

        GUILayout.Space(10);

        if (GUILayout.Button("Create Quest Entry Prefab", GUILayout.Height(30)))
        {
            CreateQuestEntryPrefab();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Create Quest Panel in Scene", GUILayout.Height(30)))
        {
            CreateQuestPanelInScene();
        }

        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "1. First create the Quest Entry Prefab\n" +
            "2. Then create the Quest Panel in Scene\n" +
            "3. Assign the prefab to the panel's Quest Entry Prefab field",
            MessageType.Warning
        );
    }

    private void CreateQuestEntryPrefab()
    {
        // Create Canvas if needed
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
        }

        // Create quest entry GameObject
        GameObject questEntry = new GameObject("TutorialQuestEntry");
        questEntry.transform.SetParent(canvas.transform, false);

        // Add Image component (background)
        Image bgImage = questEntry.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);

        // Add CanvasGroup
        questEntry.AddComponent<CanvasGroup>();

        // Add TutorialQuestEntryUI component
        TutorialQuestEntryUI entryUI = questEntry.AddComponent<TutorialQuestEntryUI>();

        // Add Vertical Layout Group
        VerticalLayoutGroup layoutGroup = questEntry.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.UpperLeft;
        layoutGroup.padding = new RectOffset(15, 15, 10, 10);
        layoutGroup.spacing = 5;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = false;

        // Add Content Size Fitter
        ContentSizeFitter sizeFitter = questEntry.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Configure RectTransform
        RectTransform rectTransform = questEntry.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(350, 100);

        // Create Label Text
        GameObject labelObj = new GameObject("LabelText");
        labelObj.transform.SetParent(questEntry.transform, false);
        TextMeshProUGUI labelText = labelObj.AddComponent<TextMeshProUGUI>();
        labelText.text = "Quest Title";
        labelText.fontSize = 20;
        labelText.fontStyle = FontStyles.Bold;
        labelText.color = Color.white;
        labelText.alignment = TextAlignmentOptions.Left;

        // Create Description Text
        GameObject descObj = new GameObject("DescriptionText");
        descObj.transform.SetParent(questEntry.transform, false);
        TextMeshProUGUI descText = descObj.AddComponent<TextMeshProUGUI>();
        descText.text = "Quest description goes here...";
        descText.fontSize = 16;
        descText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        descText.alignment = TextAlignmentOptions.Left;
        descText.enableWordWrapping = true;

        // Assign references via SerializedObject (to handle private fields)
        SerializedObject serializedEntry = new SerializedObject(entryUI);
        serializedEntry.FindProperty("labelText").objectReferenceValue = labelText;
        serializedEntry.FindProperty("descriptionText").objectReferenceValue = descText;
        serializedEntry.ApplyModifiedProperties();

        // Create prefab
        string prefabPath = "Assets/TutoAssets/Prefabs/UI/TutorialQuestEntry.prefab";
        
        // Ensure directory exists
        string directory = System.IO.Path.GetDirectoryName(prefabPath);
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        PrefabUtility.SaveAsPrefabAsset(questEntry, prefabPath);
        DestroyImmediate(questEntry);

        EditorUtility.DisplayDialog("Success", 
            $"Quest Entry Prefab created at:\n{prefabPath}", "OK");
        
        // Ping the prefab in project window
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        EditorGUIUtility.PingObject(Selection.activeObject);
    }

    private void CreateQuestPanelInScene()
    {
        // Find Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            EditorUtility.DisplayDialog("Error", 
                "No Canvas found in scene. Please create a Canvas first.", "OK");
            return;
        }

        // Create Panel Root
        GameObject panelRoot = new GameObject("TutorialQuestPanel");
        panelRoot.transform.SetParent(canvas.transform, false);

        // Add Image (background)
        Image bgImage = panelRoot.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.6f);

        // Configure RectTransform (Top-Left)
        RectTransform panelRect = panelRoot.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 1);
        panelRect.anchorMax = new Vector2(0, 1);
        panelRect.pivot = new Vector2(0, 1);
        panelRect.anchoredPosition = new Vector2(30, -30);
        panelRect.sizeDelta = new Vector2(380, 120);

        // Create Quest Container
        GameObject container = new GameObject("QuestContainer");
        container.transform.SetParent(panelRoot.transform, false);
        
        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = Vector2.zero;
        containerRect.anchorMax = Vector2.one;
        containerRect.offsetMin = new Vector2(10, 10);
        containerRect.offsetMax = new Vector2(-10, -10);

        // Add Vertical Layout Group to container
        VerticalLayoutGroup containerLayout = container.AddComponent<VerticalLayoutGroup>();
        containerLayout.childAlignment = TextAnchor.UpperLeft;
        containerLayout.spacing = 10;
        containerLayout.childForceExpandWidth = true;
        containerLayout.childForceExpandHeight = false;

        // Create Completion Message
        GameObject completionObj = new GameObject("CompletionMessage");
        completionObj.transform.SetParent(panelRoot.transform, false);
        
        TextMeshProUGUI completionText = completionObj.AddComponent<TextMeshProUGUI>();
        completionText.text = "Tutorial Complete!";
        completionText.fontSize = 22;
        completionText.fontStyle = FontStyles.Bold;
        completionText.color = new Color(1f, 0.84f, 0f); // Gold color
        completionText.alignment = TextAlignmentOptions.Center;
        
        RectTransform completionRect = completionObj.GetComponent<RectTransform>();
        completionRect.anchorMin = Vector2.zero;
        completionRect.anchorMax = Vector2.one;
        completionRect.offsetMin = new Vector2(15, 15);
        completionRect.offsetMax = new Vector2(-15, -15);
        
        completionObj.SetActive(false);

        // Add TutorialQuestPanelUI component
        TutorialQuestPanelUI panelUI = panelRoot.AddComponent<TutorialQuestPanelUI>();

        // Assign references via SerializedObject
        SerializedObject serializedPanel = new SerializedObject(panelUI);
        serializedPanel.FindProperty("questContainer").objectReferenceValue = container.transform;
        serializedPanel.FindProperty("panelRoot").objectReferenceValue = panelRoot;
        serializedPanel.FindProperty("completionMessageText").objectReferenceValue = completionText;
        serializedPanel.FindProperty("completionMessage").stringValue = "Tutorial Complete!";
        serializedPanel.FindProperty("hideOnCompletion").boolValue = false;
        serializedPanel.ApplyModifiedProperties();

        // Select the created object
        Selection.activeGameObject = panelRoot;
        EditorGUIUtility.PingObject(panelRoot);

        EditorUtility.DisplayDialog("Success", 
            "Quest Panel created in scene!\n\n" +
            "Don't forget to assign the Quest Entry Prefab in the inspector.", "OK");
    }
}

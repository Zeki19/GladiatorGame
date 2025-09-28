using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueManager))]
public class DialogueManagerEditor : Editor
{
    // Constants section
    private SerializedProperty _dialogueCanvas;
    private SerializedProperty _dialogueBoxUIText;
    private SerializedProperty _speakerBoxUIName;
    private SerializedProperty _speakerBoxUIImage;
    private SerializedProperty _audioSource;
    private SerializedProperty _animator;

    // Typing settings
    private SerializedProperty _typingDelay;
    private SerializedProperty _typingSound;

    // Dialogue list
    private SerializedProperty _dialogues;

    // Foldout toggle
    private bool _showConstants = true;

    private void OnEnable()
    {
        _dialogueCanvas = serializedObject.FindProperty("dialogueCanvas");
        _dialogueBoxUIText = serializedObject.FindProperty("dialogueBoxUIText");
        _speakerBoxUIName = serializedObject.FindProperty("speakerBoxUIName");
        _speakerBoxUIImage = serializedObject.FindProperty("speakerBoxUIImage");
        _audioSource = serializedObject.FindProperty("audioSource");
        _animator          = serializedObject.FindProperty("animator");

        _typingDelay = serializedObject.FindProperty("typingDelay");
        _typingSound = serializedObject.FindProperty("typingSound");

        _dialogues = serializedObject.FindProperty("dialogues");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // --- Constants Section ---
        _showConstants = EditorGUILayout.Foldout(_showConstants, "Constants", true);
        if (_showConstants)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(_dialogueCanvas, new GUIContent("Dialogue Canvas"));
            EditorGUILayout.PropertyField(_dialogueBoxUIText, new GUIContent("Dialogue box UI Text"));
            EditorGUILayout.PropertyField(_speakerBoxUIImage, new GUIContent("Speaker box UI Image"));
            EditorGUILayout.PropertyField(_speakerBoxUIName, new GUIContent("Speaker box UI Name"));
            EditorGUILayout.PropertyField(_audioSource, new GUIContent("Audio Source"));
            EditorGUILayout.PropertyField(_animator,          new GUIContent("Animator")); 
            EditorGUILayout.EndVertical();
        }

        // --- Typing Settings ---
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Typing Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(_typingDelay);
        EditorGUILayout.PropertyField(_typingSound);
        EditorGUILayout.EndVertical();

        // --- Dialogue List ---
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Dialogue Entries", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_dialogues, new GUIContent("Dialogues"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
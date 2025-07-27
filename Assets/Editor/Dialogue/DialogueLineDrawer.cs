using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueLine))]
public class DialogueLineDrawer : PropertyDrawer
{
   public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float spacing = 2f;
        float y = position.y;
        float lineHeight = EditorGUIUtility.singleLineHeight;

        // Get all properties
        var speakerNameProp   = property.FindPropertyRelative("speakerName");
        var speakerImageProp  = property.FindPropertyRelative("speakerImage");
        var sentenceProp      = property.FindPropertyRelative("sentence");
        var cameraShakeProp   = property.FindPropertyRelative("cameraShake");
        var shakeAtStartProp  = property.FindPropertyRelative("shakeAtTheStart");
        var soundClipProp     = property.FindPropertyRelative("soundToPlay");
        var playAtStartProp   = property.FindPropertyRelative("playAtTheStart");

        // Set default values for toggleable children
        if (!property.hasMultipleDifferentValues)
        {
            if (!shakeAtStartProp.hasMultipleDifferentValues && !shakeAtStartProp.boolValue)
                shakeAtStartProp.boolValue = true;

            if (!playAtStartProp.hasMultipleDifferentValues && !playAtStartProp.boolValue)
                playAtStartProp.boolValue = true;
        }

        // Foldout title (use speaker name if present)
        string title = string.IsNullOrEmpty(speakerNameProp.stringValue) ? "Dialogue Line" : speakerNameProp.stringValue;
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, y, position.width, lineHeight),
            property.isExpanded,
            title,
            true
        );
        y += lineHeight + spacing;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // --- Speaker Section ---
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), "Speaker", EditorStyles.boldLabel);
            y += lineHeight + spacing;

            EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), speakerNameProp);
            y += lineHeight + spacing;

            EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), speakerImageProp);
            y += lineHeight + spacing;

            // --- Sentence Section ---
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), "Sentence", EditorStyles.boldLabel);
            y += lineHeight + spacing;

            float textHeight = EditorGUI.GetPropertyHeight(sentenceProp, true);
            EditorGUI.PropertyField(new Rect(position.x, y, position.width, textHeight), sentenceProp, true);
            y += textHeight + spacing;

            // --- Camera Shake Section ---
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), "Camera Shake", EditorStyles.boldLabel);
            y += lineHeight + spacing;

            EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), cameraShakeProp);
            y += lineHeight + spacing;

            if (cameraShakeProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), shakeAtStartProp);
                EditorGUI.indentLevel--;
                y += lineHeight + spacing;
            }

            // --- Audio Section ---
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), "Audio Info", EditorStyles.boldLabel);
            y += lineHeight + spacing;

            EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), soundClipProp);
            y += lineHeight + spacing;

            if (soundClipProp.objectReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(new Rect(position.x, y, position.width, lineHeight), playAtStartProp);
                EditorGUI.indentLevel--;
                y += lineHeight + spacing;
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float spacing = 2f;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float height = lineHeight + spacing; // Foldout

        if (!property.isExpanded)
            return height;

        var sentenceProp     = property.FindPropertyRelative("sentence");
        var cameraShakeProp  = property.FindPropertyRelative("cameraShake");
        var soundClipProp    = property.FindPropertyRelative("soundToPlay");

        float textHeight = EditorGUI.GetPropertyHeight(sentenceProp, true);

        height += 4 * (lineHeight + spacing); // Speaker + label headers
        height += textHeight + spacing;
        height += 2 * (lineHeight + spacing); // Camera shake + label

        if (cameraShakeProp.boolValue)
            height += lineHeight + spacing;

        height += 2 * (lineHeight + spacing); // Audio info + clip

        if (soundClipProp.objectReferenceValue != null)
            height += lineHeight + spacing;

        return height;
    }
}

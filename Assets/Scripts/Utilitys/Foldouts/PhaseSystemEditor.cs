using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhaseSystem))]
public class PhaseSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PhaseSystem script = (PhaseSystem)target;

        // How to make regular fields
        /*
            script.someName = EditorGUILayout.TextField("Name", script.someName);
            script.someID = EditorGUILayout.IntField("ID", script.someID);
        */
        EditorGUILayout.Space();

        // Foldout for Group A
        script.showTankSettings = EditorGUILayout.Foldout(script.showTankSettings, "Tank Phase", true);
        if (script.showTankSettings)
        {
            EditorGUI.indentLevel++;
            script.tankPercentage = Mathf.Clamp(EditorGUILayout.IntField("Tank Percentage", script.tankPercentage), 0, 100);
            script.tankAttack = EditorGUILayout.FloatField("Attack", script.tankAttack);
            script.tankDefence = EditorGUILayout.FloatField("Defence", script.tankDefence);
            script.tankSpeed = EditorGUILayout.FloatField("Speed", script.tankSpeed);
            EditorGUI.indentLevel--;
        }

        // Foldout for Group B
        script.showBalancedSettings = EditorGUILayout.Foldout(script.showBalancedSettings, "Balanced Phase", true);
        if (script.showBalancedSettings)
        {
            EditorGUI.indentLevel++;
            script.balancedPercentage = Mathf.Clamp(EditorGUILayout.IntField("Balanced Percentage", script.balancedPercentage), 0, 100);
            script.balancedAttack = EditorGUILayout.FloatField("Attack", script.balancedAttack);
            script.balancedDefence = EditorGUILayout.FloatField("Defence", script.balancedDefence);
            script.balancedSpeed = EditorGUILayout.FloatField("Speed", script.balancedSpeed);
            EditorGUI.indentLevel--;
        }
        // Foldout for Group C
        script.showGlassCannonSettings = EditorGUILayout.Foldout(script.showGlassCannonSettings, "GlassCannon Phase", true);
        if (script.showGlassCannonSettings)
        {
            EditorGUI.indentLevel++;
            script.glassCannonPercentage = Mathf.Clamp(EditorGUILayout.IntField("GlassCannon Percentage", script.glassCannonPercentage), 0, 100);
            script.glassCannonAttack = EditorGUILayout.FloatField("Attack", script.glassCannonAttack);
            script.glassCannonDefence = EditorGUILayout.FloatField("Defence", script.glassCannonDefence);
            script.glassCannonSpeed = EditorGUILayout.FloatField("Speed", script.glassCannonSpeed);
            EditorGUI.indentLevel--;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }
}

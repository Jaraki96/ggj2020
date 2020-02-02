using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
[CanEditMultipleObjects]
public class HealthEditor : Editor {
    SerializedProperty enumProp;
    SerializedProperty fontProp;
    SerializedProperty vecProp;
    SerializedProperty bgColorProp;
    SerializedProperty gradProp;
    SerializedProperty textureProp;

    private void OnEnable() {
        enumProp = serializedObject.FindProperty("displayMode");
        fontProp = serializedObject.FindProperty("font");
        vecProp = serializedObject.FindProperty("offset");
        bgColorProp = serializedObject.FindProperty("backgroundColor");
        gradProp = serializedObject.FindProperty("healthGradient");
        textureProp = serializedObject.FindProperty("texture");
    }
    public override void OnInspectorGUI() {
        Health health = (Health)target;
        serializedObject.Update();
        health.maxHealth = EditorGUILayout.IntField("Max Health", health.maxHealth);
        EditorGUILayout.LabelField("Current Health: " + health.GetCurrentHealth().ToString("F" + health.precision));
        EditorGUILayout.LabelField("Dead: " + health.IsDead());
        health.destroyOnDeath = EditorGUILayout.Toggle("Destroy On Death", health.destroyOnDeath);
        health.render = EditorGUILayout.Toggle("Render", health.render);
        if (health.render) {
            EditorGUILayout.PropertyField(enumProp);
            health.width = EditorGUILayout.FloatField("Width", health.width);
            health.height = EditorGUILayout.FloatField("Height", health.height);
            EditorGUILayout.PropertyField(vecProp);
            EditorGUILayout.PropertyField(gradProp);
            EditorGUILayout.PropertyField(bgColorProp);
            EditorGUILayout.PropertyField(textureProp);
            if (health.displayMode != Health.DisplayMode.NONE) {
                EditorGUILayout.PropertyField(fontProp);
                health.fontSize = EditorGUILayout.IntField("Font Size", health.fontSize);
                health.precision = EditorGUILayout.IntSlider("Precision", health.precision, 0, 3);
            }
        }
        if (GUILayout.Button("Damage")) {
            health.TestDamage();
        }
        if (GUILayout.Button("Kill")) {
            health.Kill();
        }
        if (GUILayout.Button("Reset")) {
            health.Reset();
        }
        serializedObject.ApplyModifiedProperties();


    }
}

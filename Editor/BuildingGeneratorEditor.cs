using UnityEngine;
using UnityEditor;

namespace SimpleBuildingRules {
    [CustomEditor(typeof(BuildingBuilder))]
    public class BuildingGeneratorEditor : Editor
    {
        SerializedProperty buildingRulesProp;

        public void OnEnable() {
            buildingRulesProp = serializedObject.FindProperty ("buildingRules");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField (buildingRulesProp, new GUIContent ("Building Rules"));
            serializedObject.ApplyModifiedProperties ();
            if (GUILayout.Button("Generate")) {
                ((BuildingBuilder) serializedObject.targetObject).Build();
            }
        }
    }
}

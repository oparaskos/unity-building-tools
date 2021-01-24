
using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObjects {
    [MenuItem("Assets/Create/Simple Building Rules")]
    public static void CreateNewSimpleBuildingRules()
    {
        SimpleBuildingRules.BuildingRules asset = ScriptableObject.CreateInstance<SimpleBuildingRules.BuildingRules>();

        AssetDatabase.CreateAsset(asset, "Assets/NewSimpleBuildingRules.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
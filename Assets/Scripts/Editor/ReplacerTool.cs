using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ReplacerTool : EditorWindow
{
    private GameObject _prefab = null;

    [MenuItem("Window/Replacer")]
    static void Init(){
        var window = (ReplacerTool)EditorWindow.GetWindow(typeof(ReplacerTool));
        window.Show();
    }

    private void OnGUI(){
        _prefab = EditorGUILayout.ObjectField(_prefab, typeof(GameObject), false) as GameObject;
        if (GUILayout.Button("Replace")) ReplaceSelected();
    }

    private void ReplaceSelected(){
        if (PrefabStageUtility.GetCurrentPrefabStage() == null) return;
        var selected = Selection.objects;
        
        var parent = ((GameObject)selected[0]).transform.root;
        var stage = PrefabStageUtility.GetPrefabStage(parent.gameObject);
        var root = PrefabUtility.LoadPrefabContents(stage.assetPath);

        for (int i = selected.Length - 1; i >= 0; i--){
            var oldObj = ((GameObject)selected[i]).transform;
            var newObj = (GameObject)PrefabUtility.InstantiatePrefab(_prefab, oldObj.parent);

            newObj.transform.localPosition = oldObj.localPosition;
            newObj.transform.localRotation = oldObj.localRotation;

            DestroyImmediate(selected[i]);
        }

        EditorUtility.SetDirty(root);

        try { PrefabUtility.SaveAsPrefabAsset(root, stage.assetPath); } 
        finally { PrefabUtility.UnloadPrefabContents(root); }

        EditorSceneManager.MarkSceneDirty(stage.scene);
    }
}
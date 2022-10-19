using UnityEditor;
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
        var selected = Selection.objects;
        
        for (int i = 0; i < selected.Length; i++){

        }
    }
}
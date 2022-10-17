using UnityEditor;
using UnityEngine;

namespace Attributes
{
    public class ReadOnly : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(ReadOnly))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var guiState = GUI.enabled;

            GUI.enabled = false;

            EditorGUI.PropertyField(position, property, label);

            GUI.enabled = guiState;

        }
    }
}
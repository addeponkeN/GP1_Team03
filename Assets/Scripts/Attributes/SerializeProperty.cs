using UnityEngine;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace Attributes
{
    public class SerializeProperty : PropertyAttribute
    {
        public string Name;

        public SerializeProperty(string name)
        {
            Name = name;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(SerializeProperty))]
    public class SerializePropertyDrawer : PropertyDrawer
    {
        private PropertyInfo _propInfo;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target = property.serializedObject.targetObject;

            if(_propInfo == null)
            {
                _propInfo = target.GetType().GetProperty(((SerializeProperty)attribute).Name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }

            var value = _propInfo.GetValue(target, null);
            
            EditorGUI.BeginChangeCheck();

            value = EditorGUI.FloatField(position, label, (float)value);

            if(EditorGUI.EndChangeCheck() && _propInfo != null)
            {
                Undo.RecordObject(target, "Inspector");
                
                _propInfo.SetValue(target, value, null);
            }

            Debug.Log("prop drawer");
            
        }
    }

#endif
}
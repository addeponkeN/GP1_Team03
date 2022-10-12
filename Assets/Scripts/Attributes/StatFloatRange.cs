using System;
using UnityEditor;
using UnityEngine;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class StatFloatRange : PropertyAttribute
    {
        public readonly float Min;
        public readonly float Max;

        public StatFloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }

    [CustomPropertyDrawer(typeof(StatFloatRange))]
    public class StatFloatDrawer : PropertyDrawer
    {
        private float _value;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var range = (StatFloatRange)attribute;

            // var asset = new SerializedObject(property.objectReferenceValue);
            // var prop = asset.FindProperty("Value");
            
            var prop = property.FindPropertyRelative("Value");
            
            _value = EditorGUI.Slider(position, label, _value, range.Min, range.Max);

            prop.floatValue = _value;
            
            property.serializedObject.Update();
            property.serializedObject.ApplyModifiedProperties();
                
            // prop.floatValue = _value;
            // property.floatValue = _value;
        }
    }
}
























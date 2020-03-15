using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [CustomPropertyDrawer(typeof(Condition))]
    public class ConditionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //创建一个属性包装器，用于将常规GUI控件与SerializedProperty一起使用
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                Utility.SetLabelWidth(50);
                position.height = Utility.GetHeightLong(2);

                Rect r1 = new Rect(position.x, position.y + Utility.OneSpacing, position.width, Utility.OneHeight);
                Rect r2 = new Rect(r1.x, r1.y + Utility.GetHeightMiddle(1), r1.width, Utility.OneHeight);
                
                SerializedProperty nameProperty = property.FindPropertyRelative("key");
                SerializedProperty valueProperty = property.FindPropertyRelative("value");
                
                nameProperty.stringValue = EditorGUI.TextField(r1, "名称", nameProperty.stringValue);
                valueProperty.intValue = EditorGUI.IntSlider(r2, "初始值", valueProperty.intValue, -100, 100);
            }
        }
    }
}
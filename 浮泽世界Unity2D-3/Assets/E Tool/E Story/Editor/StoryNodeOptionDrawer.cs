using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [CustomPropertyDrawer(typeof(StoryNodeOption))]
    public class StoryNodeOptionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //创建一个属性包装器，用于将常规GUI控件与SerializedProperty一起使用
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = 60;
                position.height = EditorGUIUtility.singleLineHeight;

                Rect r1 = new Rect(position)
                {
                    y = position.y + EditorGUIUtility.standardVerticalSpacing
                };
                Rect r2 = new Rect(r1)
                {
                    y = r1.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                };

                SerializedProperty sp1 = property.FindPropertyRelative("id");
                SerializedProperty sp2 = property.FindPropertyRelative("description");

                EditorGUI.PropertyField(r1, sp1, true);
                sp2.stringValue = EditorGUI.TextField(r2, "描述", sp2.stringValue);
            }
        }
    }
}
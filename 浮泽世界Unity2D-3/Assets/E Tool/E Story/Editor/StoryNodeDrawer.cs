using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [CustomPropertyDrawer(typeof(StoryNode))]
    public class StoryNodeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //创建一个属性包装器，用于将常规GUI控件与SerializedProperty一起使用
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                //设置属性名宽度 Name HP
                EditorGUIUtility.labelWidth = 60;
                //输入框高度，默认一行的高度
                position.height = EditorGUIUtility.singleLineHeight;

                Rect nameRect = new Rect(position)
                {
                    y = position.y + EditorGUIUtility.standardVerticalSpacing
                };
                Rect valueRect = new Rect(nameRect)
                {
                    y = nameRect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                };

                //找到每个属性的序列化值
                SerializedProperty positionProperty = property.FindPropertyRelative("Position");
                SerializedProperty summaryProperty = property.FindPropertyRelative("Summary");

                positionProperty.stringValue = EditorGUI.TextField(nameRect, "位置", positionProperty.stringValue);
                summaryProperty.stringValue = EditorGUI.TextField(valueRect, "摘要", summaryProperty.stringValue);
            }
        }
    }
}
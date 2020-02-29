using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [CustomPropertyDrawer(typeof(Sentence))]
    public class SentenceDrawer : PropertyDrawer
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
                Rect r3 = new Rect(r2)
                {
                    y = r2.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                };

                //找到每个属性的序列化值
                SerializedProperty sp1 = property.FindPropertyRelative("character");
                SerializedProperty sp2 = property.FindPropertyRelative("avatar");
                SerializedProperty sp3 = property.FindPropertyRelative("words");

                sp1.stringValue = EditorGUI.TextField(r1, "角色名称", sp1.stringValue);
                sp2.objectReferenceValue = EditorGUI.ObjectField(r2, "角色表情", sp2.objectReferenceValue, typeof(Sprite));
                sp3.stringValue = EditorGUI.TextField(r3, "对话内容", sp3.stringValue);
            }
        }
    }
}
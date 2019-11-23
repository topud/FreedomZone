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
                Rect r4 = new Rect(r3)
                {
                    y = r3.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                };

                //找到每个属性的序列化值
                SerializedProperty sp1 = property.FindPropertyRelative("id");
                SerializedProperty sp1_1 = sp1.FindPropertyRelative("chapter");
                SerializedProperty sp1_2 = sp1.FindPropertyRelative("scene");
                SerializedProperty sp1_3 = sp1.FindPropertyRelative("part");
                SerializedProperty sp1_4 = sp1.FindPropertyRelative("branch");
                SerializedProperty sp2 = property.FindPropertyRelative("description");
                SerializedProperty sp3 = property.FindPropertyRelative("sentences");
                SerializedProperty sp4 = property.FindPropertyRelative("nodeOptions");

                EditorGUI.LabelField(r1, string.Format("节点编号：{0}-{1}-{2}-{3}  对话数量：{4}  选项数量：{5}",
                    sp1_1.intValue, sp1_2.intValue, sp1_3.intValue, sp1_4.intValue, sp3.arraySize, sp4.arraySize));
                sp2.stringValue = EditorGUI.TextField(r2, "节点简介", sp2.stringValue);
            }
        }
    }
}
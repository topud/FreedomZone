using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [CustomPropertyDrawer(typeof(PlotItem))]
    public class PlotItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //创建一个属性包装器，用于将常规GUI控件与SerializedProperty一起使用
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                Utility.SetLabelWidth(50);
                position.height = Utility.GetHeightLong(3);

                Rect r1 = new Rect(position.x + Utility.GetHeightMiddle(3), position.y + Utility.OneSpacing, position.width - Utility.GetHeightMiddle(3), Utility.OneHeight);
                Rect r2 = new Rect(position.x, r1.y, Utility.GetHeightShort(3), Utility.GetHeightShort(3));
                Rect r3 = new Rect(r1.x, r1.y + Utility.GetHeightMiddle(1), r1.width, Utility.GetHeightShort(2));
                Rect r3_0 = new Rect(r3.x, r3.y, Utility.LabelWidth, Utility.OneHeight);
                Rect r3_1 = new Rect(r3.x + Utility.LabelWidth + Utility.OneSpacing, r3.y, r3.width - Utility.LabelWidth - Utility.OneSpacing, Utility.GetHeightShort(2));

                //找到每个属性的序列化值
                SerializedProperty sp1 = property.FindPropertyRelative("role");
                SerializedProperty sp2 = property.FindPropertyRelative("avatar");
                SerializedProperty sp3 = property.FindPropertyRelative("words");

                sp1.stringValue = EditorGUI.TextField(r1, /*"角色名称",*/ sp1.stringValue);
                sp2.objectReferenceValue = EditorGUI.ObjectField(r2, sp2.objectReferenceValue, typeof(Sprite));
                //EditorGUI.LabelField(r3_0, "对话内容");
                sp3.stringValue = EditorGUI.TextArea(r3, sp3.stringValue.ToString());
            }
        }
    }
}
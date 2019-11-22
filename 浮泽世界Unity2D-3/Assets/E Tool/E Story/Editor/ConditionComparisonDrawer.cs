using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [CustomPropertyDrawer(typeof(ConditionComparison))]
    public class ConditionComparisonDrawer : PropertyDrawer
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
                Rect comparisonRect = new Rect(nameRect)
                {
                    y = nameRect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                };
                Rect valueRect = new Rect(comparisonRect)
                {
                    y = comparisonRect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                };


                //找到每个属性的序列化值
                SerializedProperty nameProperty = property.FindPropertyRelative("KeyIndex");
                SerializedProperty comparisonProperty = property.FindPropertyRelative("Comparison");
                SerializedProperty valueProperty = property.FindPropertyRelative("Value");

                //绘制name
                nameProperty.intValue = EditorGUI.IntField(nameRect, "条件", nameProperty.intValue);
                comparisonProperty.enumValueIndex = (int)(Comparison)EditorGUI.EnumPopup(comparisonRect, "对比方式", (Comparison)comparisonProperty.enumValueIndex);
                valueProperty.intValue = EditorGUI.IntSlider(valueRect, "目标值", valueProperty.intValue, -100, 100);
            }
        }
    }
}
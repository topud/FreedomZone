using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [CustomPropertyDrawer(typeof(PlotNode))]
    public class PlotNodeDrawer : PropertyDrawer
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
                SerializedProperty sp1 = property.FindPropertyRelative("id");
                SerializedProperty sp1_1 = sp1.FindPropertyRelative("chapter");
                SerializedProperty sp1_2 = sp1.FindPropertyRelative("scene");
                SerializedProperty sp1_3 = sp1.FindPropertyRelative("part");
                SerializedProperty sp1_4 = sp1.FindPropertyRelative("branch");
                SerializedProperty sp2 = property.FindPropertyRelative("description");
                SerializedProperty sp3 = property.FindPropertyRelative("plot");
                SerializedProperty sp4 = property.FindPropertyRelative("nextOptionNodes");
                SerializedProperty sp5 = property.FindPropertyRelative("nextPlotNode");
                SerializedProperty sp5_1 = sp5.FindPropertyRelative("chapter");
                SerializedProperty sp5_2 = sp5.FindPropertyRelative("scene");
                SerializedProperty sp5_3 = sp5.FindPropertyRelative("part");
                SerializedProperty sp5_4 = sp5.FindPropertyRelative("branch");

                int a,b,c,d,e = 0;
                a = sp1_1.intValue;
                b = sp1_2.intValue;
                c = sp1_3.intValue;
                d = sp1_4.intValue;
                 e = sp4.arraySize;
                if (sp5_1.intValue == 0 && sp5_2.intValue == 0 && sp5_3.intValue == 0 && sp5_4.intValue == 0)
                {
                }
                else
                {
                    e++;
                }

                EditorGUI.LabelField(r1, string.Format("节点编号：{0}-{1}-{2}-{3}  分支数量：{4}",a, b, c, d, e));
                sp2.stringValue = EditorGUI.TextField(r2, "节点简介", sp2.stringValue);
                sp3.objectReferenceValue = EditorGUI.ObjectField(r3, "剧情内容", sp3.objectReferenceValue, typeof(Plot));
            }
        }
    }
}
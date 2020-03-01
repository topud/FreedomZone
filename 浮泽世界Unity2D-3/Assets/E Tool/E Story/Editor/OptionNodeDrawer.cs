using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace E.Tool
{
    [CustomPropertyDrawer(typeof(OptionNode))]
    public class OptionNodeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //创建一个属性包装器，用于将常规GUI控件与SerializedProperty一起使用
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUIUtility.labelWidth = Utility.LabelWidth;
                position.height = Utility.GetHeightLong(2);

                Rect r1 = new Rect(position.x , position.y + Utility.OneSpacing, position.width, Utility.OneHeight);
                Rect r2 = new Rect(r1.x, r1.y + Utility.GetHeightMiddle(1), r1.width, Utility.OneHeight);
               
                //找到每个属性的序列化值
                SerializedProperty sp1 = property.FindPropertyRelative("id");
                SerializedProperty sp2 = property.FindPropertyRelative("description");
                SerializedProperty sp3 = property.FindPropertyRelative("comparisons");
                SerializedProperty sp4 = property.FindPropertyRelative("changes");
                SerializedProperty sp5 = property.FindPropertyRelative("nextPlotNode");
                SerializedProperty sp5_1 = sp5.FindPropertyRelative("chapter");
                SerializedProperty sp5_2 = sp5.FindPropertyRelative("scene");
                SerializedProperty sp5_3 = sp5.FindPropertyRelative("part");
                SerializedProperty sp5_4 = sp5.FindPropertyRelative("branch");

                int a,b,c,d,e,f,g = 0;
                a = sp1.intValue;
                b = sp3.arraySize;
                c = sp4.arraySize;
                d = sp5_1.intValue;
                e = sp5_2.intValue;
                f = sp5_3.intValue;
                g = sp5_4.intValue;

                string title = string.Format("选项编号：{0}  通过条件：{1}  数值变动：{2}  目标剧情节点：{3}-{4}-{5}-{6}", a, b, c, d, e, f, g);
                EditorGUI.LabelField(r1, title.Replace("0-0-0-0","无"));
                sp2.stringValue = EditorGUI.TextField(r2,  sp2.stringValue);
            }
        }
    }
}
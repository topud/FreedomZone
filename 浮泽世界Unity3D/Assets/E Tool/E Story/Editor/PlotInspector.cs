using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;
using UnityEditorInternal;

namespace E.Tool
{
    [CustomEditor(typeof(Plot))]
    public class PlotInspector : Editor
    {
        private Plot Target;
        private static ReorderableList plotItemList;

        private void OnEnable()
        {
            Target = (Plot)target;
            
            plotItemList = new ReorderableList(serializedObject, serializedObject.FindProperty("sentences"), true, true, true, true)
            {
                //元素高度
                elementHeight = Utility.GetHeightLong(3),

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty sp0 = plotItemList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, sp0, GUIContent.none);
                },

                //标题
                drawHeaderCallback = (Rect rect) =>
                {
                    string title = string.Format("对话列表 ({0})", plotItemList.count);
                    GUI.Label(rect, title);
                },

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty sp0 = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty sp1 = sp0.FindPropertyRelative("role");
                        SerializedProperty sp2 = sp0.FindPropertyRelative("words");
                        sp1.stringValue = "角色名称";
                        sp2.stringValue = "发言内容";
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    }
                    EditorUtility.SetDirty(Target);
                },

                //删除
                onRemoveCallback = (ReorderableList list) =>
                {
                    if (EditorUtility.DisplayDialog("警告", "是否要删除这条发言？", "是", "否"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                },
            };

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            plotItemList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
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
        private static ReorderableList sens;

        private void OnEnable()
        {
            Target = (Plot)target;
            
            float svs = EditorGUIUtility.standardVerticalSpacing;
            float slh = EditorGUIUtility.singleLineHeight;
            sens = new ReorderableList(serializedObject, serializedObject.FindProperty("sentences"), true, true, true, true)
            {
                //设置单个元素的高度
                elementHeight = slh * 3 + svs * 4,

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty itemData = sens.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, itemData, GUIContent.none);
                },

                //标题
                drawHeaderCallback = (Rect rect) =>
                {
                    string id = string.Format("剧情片段");
                    GUI.Label(rect, id);
                },

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    if (list.list != null)
                    {
                        list.list.Add(new Sentence("角色名称", "发言内容"));
                        list.index = list.list.Count - 1;
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
            sens.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
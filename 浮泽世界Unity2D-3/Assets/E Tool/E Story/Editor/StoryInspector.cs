using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;
using UnityEditorInternal;

namespace E.Tool
{
    [CustomEditor(typeof(Story))]
    public class StoryInspector : Editor
    {
        private Story Target;
        private ReorderableList conditionList;
        private ReorderableList plotNodeList;
        private ReorderableList optionNodeList;
        private bool isShowBaseInfo = true;
        private bool isShowConditionList = true;
        private bool isShowPlotNodeList = true;
        private bool isShowOptionNodeList = true;

        private void OnEnable()
        {
            Target = (Story)target;

            conditionList = new ReorderableList(serializedObject, serializedObject.FindProperty("conditions"), true, false, true, true)
            {
                //元素高度
                elementHeight = Utility.GetHeightLong(2),

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty sp0 = conditionList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, sp0, GUIContent.none);
                },

                //背景色
                //reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
                //    GUI.backgroundColor = Color.yellow;
                //};
                showDefaultBackground = true,

                //标题
                //drawHeaderCallback = (Rect rect) =>
                //{
                    //string title = string.Format("条件列表 ({0})", conditionList.count);
                    //GUI.Label(rect, title);
                //},

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty sp0 = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty sp1 = sp0.FindPropertyRelative("key");
                        SerializedProperty sp2 = sp0.FindPropertyRelative("value");
                        sp1.stringValue = "新条件";
                        sp2.intValue = 0;
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    }
                },

                //删除
                onRemoveCallback = (ReorderableList list) =>
                {
                    if (EditorUtility.DisplayDialog("警告", "是否要删除这个条件？", "是", "否"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                },

                //鼠标抬起回调
                onMouseUpCallback = (ReorderableList list) =>
                {
                    //Debug.Log("MouseUP");
                },

                //当选择元素回调
                onSelectCallback = (ReorderableList list) =>
                {
                   // Debug.Log(list.index);
                }
            };
            plotNodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("plotNodes"), true, false, false, true)
            {
                //元素高度
                elementHeight = Utility.GetHeightLong(3),

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty sp0 = plotNodeList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, sp0, GUIContent.none);
                },

                //背景色
                //reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
                //    GUI.backgroundColor = Color.yellow;
                //};
                showDefaultBackground = true,

                //标题
                //drawHeaderCallback = (Rect rect) =>
                //{
                //    string title = string.Format("剧情节点列表 ({0})", plotNodeList.count);
                //    GUI.Label(rect, title);
                //},

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                },

                //删除
                onRemoveCallback = (ReorderableList list) =>
                {
                    if (EditorUtility.DisplayDialog("警告", "是否要删除这个节点？", "是", "否"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                },

                //鼠标抬起回调
                onMouseUpCallback = (ReorderableList list) =>
                {
                },

                //当选择元素回调
                onSelectCallback = (ReorderableList list) =>
                {
                }
            };
            optionNodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("optionNodes"), true, false, false, true)
            {
                //元素高度
                elementHeight = Utility.GetHeightLong(2),

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty sp0 = optionNodeList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, sp0, GUIContent.none);
                },

                //背景色
                //reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
                //    GUI.backgroundColor = Color.yellow;
                //};
                showDefaultBackground = true,

                //标题
                //drawHeaderCallback = (Rect rect) =>
                //{
                //    string title = string.Format("选项节点列表 ({0})", optionNodeList.count);
                //    GUI.Label(rect, title);
                //},

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                },

                //删除
                onRemoveCallback = (ReorderableList list) =>
                {
                    if (EditorUtility.DisplayDialog("警告", "是否要删除这个节点？", "是", "否"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                },

                //鼠标抬起回调
                onMouseUpCallback = (ReorderableList list) =>
                {
                },

                //当选择元素回调
                onSelectCallback = (ReorderableList list) =>
                {
                }
            };

        }
        public override void OnInspectorGUI()
        {
            //EditorGUI.indentLevel = 0;

            if (GUILayout.Button("打开 E Story 故事树编辑器"))
            {
                //StoryWindow.Open();
                StoryWindow.Instance.OpenStory(Target);
            }
            isShowBaseInfo = EditorGUILayout.BeginFoldoutHeaderGroup(isShowBaseInfo, string.Format("基础信息"));
            if (isShowBaseInfo)
            {
                EditorGUIUtility.labelWidth = 55;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("故事描述");
                Target.description = EditorGUILayout.TextArea(Target.description);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.Update();
            isShowConditionList = EditorGUILayout.BeginFoldoutHeaderGroup(isShowConditionList, string.Format("数值条件 ({0})", conditionList.count));
            if (isShowConditionList)
            {
                conditionList.DoLayoutList();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            isShowPlotNodeList = EditorGUILayout.BeginFoldoutHeaderGroup(isShowPlotNodeList, string.Format("剧情节点 ({0})", plotNodeList.count));
            if (isShowPlotNodeList)
            {
                plotNodeList.DoLayoutList();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            isShowOptionNodeList = EditorGUILayout.BeginFoldoutHeaderGroup(isShowOptionNodeList, string.Format("选项节点 ({0})", optionNodeList.count));
            if (isShowOptionNodeList)
            {
                optionNodeList.DoLayoutList();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
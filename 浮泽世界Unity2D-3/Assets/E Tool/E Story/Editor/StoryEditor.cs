// ========================================================
// 作者：E Star
// 创建时间：2019-03-10 17:03:03
// 当前版本：1.0
// 作用描述：自定义Inspector面板显示ScriptableStory的样式
// 挂载目标：无
// ========================================================
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
    public class StoryEditor : Editor
    {
        private Story Target;
        private ReorderableList NodesList;
        private ReorderableList ConditionList;

        private void OnEnable()
        {
            Target = (Story)target;

            NodesList = new ReorderableList(serializedObject, serializedObject.FindProperty("Nodes"), true, true, true, true)
            {
                //设置单个元素的高度
                elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3,

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty itemData = NodesList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, itemData, GUIContent.none);
                },

                //背景色
                //reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
                //    GUI.backgroundColor = Color.yellow;
                //};
                showDefaultBackground = true,

                //标题
                drawHeaderCallback = (Rect rect) =>
                {
                    GUI.Label(rect, "节点");
                },

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty positionProperty = itemData.FindPropertyRelative("Position");
                        SerializedProperty summaryProperty = itemData.FindPropertyRelative("Summary");
                        positionProperty.stringValue = "（未知地点）";
                        summaryProperty.stringValue = "（摘要）";
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    }
                },

                //删除
                onRemoveCallback = (ReorderableList list) =>
                {
                    if (EditorUtility.DisplayDialog("警告", "是否真的要删除这个节点？", "是", "否"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                },

                //鼠标抬起回调
                onMouseUpCallback = (ReorderableList list) =>
                {
                    Debug.Log("MouseUP");
                },

                //当选择元素回调
                onSelectCallback = (ReorderableList list) =>
                {
                    //打印选中元素的索引
                    Debug.Log(list.index);
                }
            };

            ConditionList = new ReorderableList(serializedObject, serializedObject.FindProperty("Conditions"), true, true, true, true)
            {
                //设置单个元素的高度
                elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3,

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty itemData = ConditionList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, itemData, GUIContent.none);
                },

                //背景色
                //reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
                //    GUI.backgroundColor = Color.yellow;
                //};
                showDefaultBackground = true,

                //标题
                drawHeaderCallback = (Rect rect) =>
                {
                    GUI.Label(rect, "条件");
                },

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty nameProperty = itemData.FindPropertyRelative("Key");
                        SerializedProperty valueProperty = itemData.FindPropertyRelative("DefaultValue");
                        nameProperty.stringValue = "未命名";
                        valueProperty.intValue = 0;
                    }
                    else
                    {
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    }
                },

                //删除
                onRemoveCallback = (ReorderableList list) =>
                {
                    if (EditorUtility.DisplayDialog("警告", "是否真的要删除这个条件？", "是", "否"))
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    }
                },

                //鼠标抬起回调
                onMouseUpCallback = (ReorderableList list) =>
                {
                    Debug.Log("MouseUP");
                },

                //当选择元素回调
                onSelectCallback = (ReorderableList list) =>
                {
                    //打印选中元素的索引
                    Debug.Log(list.index);
                }
            };
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("【故事描述】");
            Target.Describe = EditorGUILayout.TextArea(Target.Describe);

            serializedObject.Update();
            NodesList.DoLayoutList();
            ConditionList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
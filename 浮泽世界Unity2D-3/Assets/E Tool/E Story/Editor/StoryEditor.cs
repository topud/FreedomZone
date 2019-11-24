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
        private ReorderableList nodeList;
        private ReorderableList conditionList;

        private void OnEnable()
        {
            Target = (Story)target;

            nodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("nodes"), true, true, false, true)
            {
                //设置单个元素的高度
                elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3,

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty itemData = nodeList.serializedProperty.GetArrayElementAtIndex(index);
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
                    GUI.Label(rect, "节点列表");
                },

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty sp1 = itemData.FindPropertyRelative("description");
                        SerializedProperty sp2 = itemData.FindPropertyRelative("layout");
                        sp1.stringValue = "（请输入节点简介）";
                        sp2.rectIntValue = new RectInt(100,100, StoryEditorWindowPreference.NodeSize.x, StoryEditorWindowPreference.NodeSize.y);
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

            conditionList = new ReorderableList(serializedObject, serializedObject.FindProperty("conditions"), true, true, true, true)
            {
                //设置单个元素的高度
                elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3,

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty itemData = conditionList.serializedProperty.GetArrayElementAtIndex(index);
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
                    GUI.Label(rect, "条件列表");
                },

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    if (list.serializedProperty != null)
                    {
                        list.serializedProperty.arraySize++;
                        list.index = list.serializedProperty.arraySize - 1;

                        SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                        SerializedProperty nameProperty = itemData.FindPropertyRelative("key");
                        SerializedProperty valueProperty = itemData.FindPropertyRelative("value");
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

            if (GUILayout.Button("打开E Story编辑器"))
            {
                StoryEditorWindow.Open();
            }

            EditorGUILayout.LabelField("故事描述");
            Target.description = EditorGUILayout.TextArea(Target.description);

            EditorGUILayout.Space(5);

            serializedObject.Update();
            nodeList.DoLayoutList();
            conditionList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
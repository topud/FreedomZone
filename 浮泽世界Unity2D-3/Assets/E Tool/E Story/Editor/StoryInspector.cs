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

        private void OnEnable()
        {
            Target = (Story)target;

            float svs = EditorGUIUtility.standardVerticalSpacing;
            float slh = EditorGUIUtility.singleLineHeight;
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
                    if (EditorUtility.DisplayDialog("警告", "是否要删除这个条件？", "是", "否"))
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
            plotNodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("plotNodes"), true, true, false, false)
            {
                //设置单个元素的高度
                elementHeight = slh * 3 + svs * 4,

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty itemData = plotNodeList.serializedProperty.GetArrayElementAtIndex(index);
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
                    GUI.Label(rect, "剧情节点列表");
                },

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    //if (list.serializedProperty != null)
                    //{
                    //    list.serializedProperty.arraySize++;
                    //    list.index = list.serializedProperty.arraySize - 1;

                    //    SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                    //    SerializedProperty sp1 = itemData.FindPropertyRelative("description");
                    //    SerializedProperty sp2 = itemData.FindPropertyRelative("layout");
                    //    sp1.stringValue = "（请输入节点简介）";
                    //    sp2.rectIntValue = new RectInt(100,100, StoryWindowPreference.NodeSize.x, StoryWindowPreference.NodeSize.y);
                    //}
                    //else
                    //{
                    //    ReorderableList.defaultBehaviours.DoAddButton(list);
                    //}
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
                    //Debug.Log("MouseUP");
                },

                //当选择元素回调
                onSelectCallback = (ReorderableList list) =>
                {
                    //打印选中元素的索引
                    //Debug.Log(list.index);
                }
            };
            optionNodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("optionNodes"), true, true, false, false)
            {
                //设置单个元素的高度
                elementHeight = slh * 2 + svs * 3,

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    SerializedProperty itemData = optionNodeList.serializedProperty.GetArrayElementAtIndex(index);
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
                    GUI.Label(rect, "选项节点列表");
                },

                //创建
                onAddCallback = (ReorderableList list) =>
                {
                    //if (list.serializedProperty != null)
                    //{
                    //    list.serializedProperty.arraySize++;
                    //    list.index = list.serializedProperty.arraySize - 1;

                    //    SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
                    //    SerializedProperty sp1 = itemData.FindPropertyRelative("description");
                    //    SerializedProperty sp2 = itemData.FindPropertyRelative("layout");
                    //    sp1.stringValue = "（请输入节点简介）";
                    //    sp2.rectIntValue = new RectInt(100, 100, StoryWindowPreference.NodeSize.x, StoryWindowPreference.NodeSize.y);
                    //}
                    //else
                    //{
                    //    ReorderableList.defaultBehaviours.DoAddButton(list);
                    //}
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
                    //Debug.Log("MouseUP");
                },

                //当选择元素回调
                onSelectCallback = (ReorderableList list) =>
                {
                    //打印选中元素的索引
                    //Debug.Log(list.index);
                }
            };

        }
        public override void OnInspectorGUI()
        {

            if (GUILayout.Button("打开E Story编辑器"))
            {
                StoryWindow.Open();
                StoryWindow.OpenStory(Target);
            }

            EditorGUILayout.LabelField("故事描述");
            Target.description = EditorGUILayout.TextArea(Target.description);

            EditorGUILayout.Space(5);

            serializedObject.Update();
            conditionList.DoLayoutList();
            plotNodeList.DoLayoutList();
            optionNodeList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
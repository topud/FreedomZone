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

            conditionList = new ReorderableList(serializedObject, serializedObject.FindProperty("conditions"), true, true, true, true)
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
                drawHeaderCallback = (Rect rect) =>
                {
                    string title = string.Format("条件列表 ({0})", conditionList.count);
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
                        SerializedProperty sp1 = sp0.FindPropertyRelative("key");
                        SerializedProperty sp2 = sp0.FindPropertyRelative("value");
                        sp1.stringValue = "未命名";
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
            plotNodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("plotNodes"), true, true, false, true)
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
                drawHeaderCallback = (Rect rect) =>
                {
                    string title = string.Format("剧情节点列表 ({0})", plotNodeList.count);
                    GUI.Label(rect, title);
                },

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
            optionNodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("optionNodes"), true, true, false, true)
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
                drawHeaderCallback = (Rect rect) =>
                {
                    string title = string.Format("选项节点列表 ({0})", optionNodeList.count);
                    GUI.Label(rect, title);
                },

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
            if (GUILayout.Button("打开 E Story 编辑窗口"))
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
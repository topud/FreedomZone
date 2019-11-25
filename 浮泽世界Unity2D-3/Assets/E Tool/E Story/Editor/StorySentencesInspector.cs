using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;
using UnityEditorInternal;

namespace E.Tool
{
    [CustomEditor(typeof(Sentences))]
    public class StorySentencesInspector : Editor
    {
        private Sentences Target;
        private static ReorderableList sens;

        private void OnEnable()
        {
            Target = (Sentences)target;
            
            float svs = EditorGUIUtility.standardVerticalSpacing;
            float slh = EditorGUIUtility.singleLineHeight;
            sens = new ReorderableList(Target.sentences, typeof(Sentence), true, true, true, true)
            {
                //设置单个元素的高度
                elementHeight = slh * 3 + svs * 4,

                //绘制
                drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                {
                    Sentence data = (Sentence)sens.list[index];
                    float f = slh * 3 + svs * 2;
                    Rect r1 = new Rect(rect.x, rect.y + svs, f, f);
                    data.avatar = (Sprite)EditorGUI.ObjectField(r1, data.avatar, typeof(Sprite));
                    Rect r2 = new Rect(rect.x + f + svs, rect.y + svs, rect.width - f, slh);
                    data.character = EditorGUI.TextField(r2, data.character);
                    Rect r3 = new Rect(rect.x + f + svs, rect.y + slh + svs * 2, rect.width - f, svs + slh * 2);
                    data.words = EditorGUI.TextField(r3, data.words);
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
// ========================================================
// 作者：E Star
// 创建时间：2019-03-10 17:03:03
// 当前版本：1.0
// 作用描述：自定义Inspector面板显示StoryContent的样式
// 挂载目标：无
// ========================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;

namespace E.Tool
{
    [CustomEditor(typeof(StoryNode))]
    public class StoryNodeEditor : Editor
    {
        private StoryNode Target;

        private void OnEnable()
        {
            //Target = (StoryNode)target;
        }

        public override void OnInspectorGUI()
        {
            //EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            //节点编号
            EditorGUILayout.LabelField("节点编号 {章节-场景-片段-分支}");
            EditorGUILayout.BeginHorizontal();
            int chapter = EditorGUILayout.IntField(Target.ID.Chapter);
            int scene = EditorGUILayout.IntField(Target.ID.Scene);
            int part = EditorGUILayout.IntField(Target.ID.Part);
            int branch = EditorGUILayout.IntField(Target.ID.Branch);
            EditorGUILayout.EndHorizontal();
            Target.ID = new NodeID(chapter, scene, part, branch);
            //if (!id.Equals(Target.ID))
            //{
            //    Target.SetNodeID(Target, id);
            //}
            Target.IsMainNode = EditorGUILayout.Toggle("故事主线", Target.IsMainNode);
            //节点布局
            EditorGUILayout.LabelField("节点布局 {X坐标-Y坐标-宽-高}");
            EditorGUILayout.BeginHorizontal();
            int rx = EditorGUILayout.IntField(Target.Rect.x);
            int ry = EditorGUILayout.IntField(Target.Rect.y);
            int rw = EditorGUILayout.IntField(Target.Rect.width);
            int rh = EditorGUILayout.IntField(Target.Rect.height);
            EditorGUILayout.EndHorizontal();
            Target.Rect = new RectInt(rx, ry, rw, rh);
            //节点类型
            Target.Type = (NodeType)EditorGUILayout.EnumPopup("节点类型", Target.Type);
            //Target.SetNodeType(Target, type);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("【基本信息】");
            EditorGUILayout.LabelField("发生时间 {年-月-日-时-分-秒}");
            EditorGUILayout.BeginHorizontal();
            int y = EditorGUILayout.IntField(Target.Time.Year);
            int mo = EditorGUILayout.IntField(Target.Time.Month);
            int d = EditorGUILayout.IntField(Target.Time.Day);
            int h = EditorGUILayout.IntField(Target.Time.Hour);
            int mi = EditorGUILayout.IntField(Target.Time.Minute);
            int s = EditorGUILayout.IntField(Target.Time.Second);
            Target.Time = new DateTime(y, mo, d, h, mi, s);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("发生地点");
            Target.Position = EditorGUILayout.TextField(Target.Position);
            EditorGUILayout.LabelField("内容概述");
            Target.Summary = EditorGUILayout.TextArea(Target.Summary);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("【内容详情】");
            Target.ContentType = (ContentType)EditorGUILayout.EnumPopup(Target.ContentType);
            //设置内容折叠
            switch (Target.ContentType)
            {
                case ContentType.剧情对话:
                    if (Target.Sentences.Count > 0)
                    {
                        for (int i = 0; i < Target.Sentences.Count; i++)
                        {
                            Sentence item = Target.Sentences[i];

                            string temp = item.Expression == null ? "旁白" : "未命名角色";
                            string temp1 = item.Speaker.Length > 0 ? item.Speaker : temp;
                            string temp2 = item.Words.Length > 10 ? item.Words.Substring(0, 10) + "..." : item.Words;
                            item.IsFold = EditorGUILayout.Foldout(item.IsFold, temp1 + "：" + temp2, true);
                            if (item.IsFold)
                            {
                                EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
                                EditorGUILayout.BeginHorizontal();
                                item.Speaker = EditorGUILayout.TextField(item.Speaker);
                                item.Expression = (Sprite)EditorGUILayout.ObjectField(item.Expression, typeof(Sprite));
                                EditorGUILayout.EndHorizontal();

                                item.Words = EditorGUILayout.TextArea(item.Words);

                                EditorGUILayout.BeginHorizontal();
                                //item.IsReaded = EditorGUILayout.ToggleLeft("已阅读过此句话", item.IsReaded);
                                if (GUILayout.Button("X"))
                                {
                                    Target.Sentences.Remove(item);
                                    i--;
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.EndVertical();
                            }
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("请点击按钮添加对话", MessageType.Warning);
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);
                    EditorGUILayout.LabelField("");
                    if (GUILayout.Button("+"))
                    {
                        Target.Sentences.Add(new Sentence());
                    }
                    EditorGUILayout.EndHorizontal();
                    break;
                case ContentType.过场动画:
                    break;
                default:
                    break;
            }





            serializedObject.ApplyModifiedProperties();
        }
    }
}
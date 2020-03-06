using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using E.Tool;

namespace E.Tool
{
    public class StoryWindowPreference : EditorWindow
    {
        /// <summary>
        /// 视图尺寸
        /// </summary>
        public static Vector2Int ViewSize
        {
            get
            {
                if (!EditorPrefs.HasKey("ViewWidth")) EditorPrefs.SetInt("ViewWidth", 3000);
                if (!EditorPrefs.HasKey("ViewHeight")) EditorPrefs.SetInt("ViewHeight", 3000);
                return new Vector2Int(EditorPrefs.GetInt("ViewWidth"), EditorPrefs.GetInt("ViewHeight"));
            }
            set
            {
                EditorPrefs.SetInt("ViewWidth", value.x);
                EditorPrefs.SetInt("ViewHeight", value.y);
            }
        }
        /// <summary>
        /// 节点尺寸
        /// </summary>
        public static int NodeWidth
        {
            get
            {
                if (!EditorPrefs.HasKey("NodeWidth")) EditorPrefs.SetInt("NodeWidth", 300);
                if (!EditorPrefs.HasKey("NodeHeight")) EditorPrefs.SetInt("NodeHeight", 80);
                return EditorPrefs.GetInt("NodeWidth");
            }
            set
            {
                EditorPrefs.SetInt("NodeWidth", value);
            }
        }
        /// <summary>
        /// 背景颜色
        /// </summary>
        public static Color Background
        {
            get
            {
                if (!EditorPrefs.HasKey("BackgroundR")) EditorPrefs.SetFloat("BackgroundR", 0.9f);
                if (!EditorPrefs.HasKey("BackgroundG")) EditorPrefs.SetFloat("BackgroundG", 0.9f);
                if (!EditorPrefs.HasKey("BackgroundB")) EditorPrefs.SetFloat("BackgroundB", 0.9f);
                if (!EditorPrefs.HasKey("BackgroundA")) EditorPrefs.SetFloat("BackgroundA", 1);
                return new Color(EditorPrefs.GetFloat("BackgroundR"), EditorPrefs.GetFloat("BackgroundG"), EditorPrefs.GetFloat("BackgroundB"), EditorPrefs.GetFloat("BackgroundA"));

            }
            set
            {
                EditorPrefs.SetFloat("BackgroundR", value.r);
                EditorPrefs.SetFloat("BackgroundG", value.g);
                EditorPrefs.SetFloat("BackgroundB", value.b);
                EditorPrefs.SetFloat("BackgroundA", value.a);
            }
        }
        /// <summary>
        /// 选中节点颜色
        /// </summary>
        public static Color SelectNode
        {
            get
            {
                if (!EditorPrefs.HasKey("SelectNodeR")) EditorPrefs.SetFloat("SelectNodeR", 0.9f);
                if (!EditorPrefs.HasKey("SelectNodeG")) EditorPrefs.SetFloat("SelectNodeG", 0.85f);
                if (!EditorPrefs.HasKey("SelectNodeB")) EditorPrefs.SetFloat("SelectNodeB", 0.5f);
                if (!EditorPrefs.HasKey("SelectNodeA")) EditorPrefs.SetFloat("SelectNodeA", 1);
                return new Color(EditorPrefs.GetFloat("SelectNodeR"), EditorPrefs.GetFloat("SelectNodeG"), EditorPrefs.GetFloat("SelectNodeB"), EditorPrefs.GetFloat("SelectNodeA"));
            }
            set
            {
                EditorPrefs.SetFloat("SelectNodeR", value.r);
                EditorPrefs.SetFloat("SelectNodeG", value.g);
                EditorPrefs.SetFloat("SelectNodeB", value.b);
                EditorPrefs.SetFloat("SelectNodeA", value.a);
            }
        }
        /// <summary>
        /// 主线颜色
        /// </summary>
        public static Color MainLine
        {
            get
            {
                if (!EditorPrefs.HasKey("MainLineR")) EditorPrefs.SetFloat("MainLineR", 0);
                if (!EditorPrefs.HasKey("MainLineG")) EditorPrefs.SetFloat("MainLineG", 0.7f);
                if (!EditorPrefs.HasKey("MainLineB")) EditorPrefs.SetFloat("MainLineB", 0.4f);
                if (!EditorPrefs.HasKey("MainLineA")) EditorPrefs.SetFloat("MainLineA", 1);
                return new Color(EditorPrefs.GetFloat("MainLineR"), EditorPrefs.GetFloat("MainLineG"), EditorPrefs.GetFloat("MainLineB"), EditorPrefs.GetFloat("MainLineA"));
            }
            set
            {
                EditorPrefs.SetFloat("MainLineR", value.r);
                EditorPrefs.SetFloat("MainLineG", value.g);
                EditorPrefs.SetFloat("MainLineB", value.b);
                EditorPrefs.SetFloat("MainLineA", value.a);
            }
        }
        /// <summary>
        /// 支线颜色
        /// </summary>
        public static Color BranchLine
        {
            get
            {
                if (!EditorPrefs.HasKey("BranchLineR")) EditorPrefs.SetFloat("BranchLineR", 0.7f);
                if (!EditorPrefs.HasKey("BranchLineG")) EditorPrefs.SetFloat("BranchLineG", 0.6f);
                if (!EditorPrefs.HasKey("BranchLineB")) EditorPrefs.SetFloat("BranchLineB", 0);
                if (!EditorPrefs.HasKey("BranchLineA")) EditorPrefs.SetFloat("BranchLineA", 1);
                return new Color(EditorPrefs.GetFloat("BranchLineR"), EditorPrefs.GetFloat("BranchLineG"), EditorPrefs.GetFloat("BranchLineB"), EditorPrefs.GetFloat("BranchLineA"));
            }
            set
            {
                EditorPrefs.SetFloat("BranchLineR", value.r);
                EditorPrefs.SetFloat("BranchLineG", value.g);
                EditorPrefs.SetFloat("BranchLineB", value.b);
                EditorPrefs.SetFloat("BranchLineA", value.a);
            }
        }
        /// <summary>
        /// 上次打开的故事
        /// </summary>
        public static Story LastOpendStory
        {
            get
            {
                if (EditorPrefs.HasKey("LastOpendStory"))
                { 
                    string path = EditorPrefs.GetString("LastOpendStory");
                    Story story = AssetDatabase.LoadAssetAtPath<Story>(path);
                    return story;
                }
                return null;
            }
            set
            {
                string path = AssetDatabase.GetAssetPath(value);
                EditorPrefs.SetString("LastOpendStory", path);
            }
        }
        
        [PreferenceItem("E Story")]
        private static void OnPreference()
        {
            EditorGUILayout.LabelField("布局", EditorStyles.boldLabel);
            Vector2Int v2 = EditorGUILayout.Vector2IntField("画布尺寸", ViewSize);
            if (v2.x < 1000)
            {
                v2.x = 1000;
            }
            if (v2.y < 1000)
            {
                v2.y = 1000;
            }
            ViewSize = v2;
            int nw = EditorGUILayout.DelayedIntField("节点宽度", NodeWidth);
            if (nw < 280)
            {
                nw = 280;
            }
            NodeWidth = nw;

            EditorGUILayout.LabelField("颜色", EditorStyles.boldLabel);
            Background = EditorGUILayout.ColorField("背景颜色", Background);
            SelectNode = EditorGUILayout.ColorField("选中节点", SelectNode);
            MainLine = EditorGUILayout.ColorField("主线结点连接线", MainLine);
            BranchLine = EditorGUILayout.ColorField("支线结点连接线", BranchLine);

            EditorGUILayout.Space(10);
            if (GUILayout.Button("重置"))
            {
                Reset();
            }
        }
        
        private static void Reset()
        {
            EditorPrefs.SetInt("ViewWidth", 3000);
            EditorPrefs.SetInt("ViewHeight", 3000);
            EditorPrefs.SetInt("NodeWidth", 300);

            EditorPrefs.SetFloat("BackgroundR", 0.9f);
            EditorPrefs.SetFloat("BackgroundG", 0.9f);
            EditorPrefs.SetFloat("BackgroundB", 0.9f);
            EditorPrefs.SetFloat("BackgroundA", 1);
            EditorPrefs.SetFloat("SelectNodeR", 0.9f);
            EditorPrefs.SetFloat("SelectNodeG", 0.85f);
            EditorPrefs.SetFloat("SelectNodeB", 0.5f);
            EditorPrefs.SetFloat("SelectNodeA", 1);
            EditorPrefs.SetFloat("MainLineR", 0);
            EditorPrefs.SetFloat("MainLineG", 0.7f);
            EditorPrefs.SetFloat("MainLineB", 0.4f);
            EditorPrefs.SetFloat("MainLineA", 1);
            EditorPrefs.SetFloat("BranchLineR", 0.7f);
            EditorPrefs.SetFloat("BranchLineG", 0.6f);
            EditorPrefs.SetFloat("BranchLineB", 0);
            EditorPrefs.SetFloat("BranchLineA", 1);

            Debug.Log("E Story 预设已重置");
        }
    }
}
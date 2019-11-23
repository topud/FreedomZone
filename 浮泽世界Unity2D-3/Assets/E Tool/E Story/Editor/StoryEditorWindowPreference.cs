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
    public class StoryEditorWindowPreference : EditorWindow
    {
        /// <summary>
        /// 故事资产文件夹路径
        /// </summary>
        public static string StoryResourcesFolder
        {
            get
            {
                if (!EditorPrefs.HasKey("StoryResourcesFolder")) EditorPrefs.SetString("StoryResourcesFolder", "Assets/E Tool/E Story/Resources/Example Storys");
                return EditorPrefs.GetString("StoryResourcesFolder");
            }
            set
            {
                EditorPrefs.SetString("StoryResourcesFolder", value);
            }
        }
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
        public static Vector2Int NodeSize
        {
            get
            {
                if (!EditorPrefs.HasKey("NodeWidth")) EditorPrefs.SetInt("NodeWidth", 200);
                if (!EditorPrefs.HasKey("NodeHeight")) EditorPrefs.SetInt("NodeHeight", 60);
                return new Vector2Int(EditorPrefs.GetInt("NodeWidth"), EditorPrefs.GetInt("NodeHeight"));
            }
            set
            {
                EditorPrefs.SetInt("NodeWidth", value.x);
                EditorPrefs.SetInt("NodeHeight", value.y);
            }
        }
        /// <summary>
        /// 默认节点颜色
        /// </summary>
        public static Color NormalNode
        {
            get
            {
                if (!EditorPrefs.HasKey("NormalNodeR")) EditorPrefs.SetFloat("NormalNodeR", 0.9f);
                if (!EditorPrefs.HasKey("NormalNodeG")) EditorPrefs.SetFloat("NormalNodeG", 0.9f);
                if (!EditorPrefs.HasKey("NormalNodeB")) EditorPrefs.SetFloat("NormalNodeB", 0.9f);
                if (!EditorPrefs.HasKey("NormalNodeA")) EditorPrefs.SetFloat("NormalNodeA", 1);
                return new Color(EditorPrefs.GetFloat("NormalNodeR"), EditorPrefs.GetFloat("NormalNodeG"), EditorPrefs.GetFloat("NormalNodeB"), EditorPrefs.GetFloat("NormalNodeA"));

            }
            set
            {
                EditorPrefs.SetFloat("NormalNodeR", value.r);
                EditorPrefs.SetFloat("NormalNodeG", value.g);
                EditorPrefs.SetFloat("NormalNodeB", value.b);
                EditorPrefs.SetFloat("NormalNodeA", value.a);
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
                if (!EditorPrefs.HasKey("MainLineB")) EditorPrefs.SetFloat("MainLineB", 0);
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
                if (!EditorPrefs.HasKey("BranchLineG")) EditorPrefs.SetFloat("BranchLineG", 0);
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
        
        [PreferenceItem("E Story")]
        private static void OnPreference()
        {
            EditorGUILayout.LabelField("路径", EditorStyles.boldLabel);
            StoryResourcesFolder = EditorGUILayout.TextField("资产文件默认存放路径", StoryResourcesFolder);

            EditorGUILayout.LabelField("布局", EditorStyles.boldLabel);
            ViewSize = EditorGUILayout.Vector2IntField("画布尺寸", ViewSize);
            NodeSize = EditorGUILayout.Vector2IntField("节点尺寸", NodeSize);

            EditorGUILayout.LabelField("颜色", EditorStyles.boldLabel);
            NormalNode = EditorGUILayout.ColorField("默认节点", NormalNode);
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
            EditorPrefs.SetString("ViewWidth", "Assets/E Tool/E Story/Resources/Example Storys");

            EditorPrefs.SetInt("ViewWidth", 3000);
            EditorPrefs.SetInt("ViewHeight", 3000);
            EditorPrefs.SetInt("NodeWidth", 200);
            EditorPrefs.SetInt("NodeHeight", 60);

            EditorPrefs.SetFloat("NormalNodeR", 0.9f);
            EditorPrefs.SetFloat("NormalNodeG", 0.9f);
            EditorPrefs.SetFloat("NormalNodeB", 0.9f);
            EditorPrefs.SetFloat("NormalNodeA", 1);
            EditorPrefs.SetFloat("SelectNodeR", 0.9f);
            EditorPrefs.SetFloat("SelectNodeG", 0.85f);
            EditorPrefs.SetFloat("SelectNodeB", 0.5f);
            EditorPrefs.SetFloat("SelectNodeA", 1);
            EditorPrefs.SetFloat("MainLineR", 0);
            EditorPrefs.SetFloat("MainLineG", 0.7f);
            EditorPrefs.SetFloat("MainLineB", 0);
            EditorPrefs.SetFloat("MainLineA", 1);
            EditorPrefs.SetFloat("BranchLineR", 0.7f);
            EditorPrefs.SetFloat("BranchLineG", 0);
            EditorPrefs.SetFloat("BranchLineB", 0);
            EditorPrefs.SetFloat("BranchLineA", 1);

            Debug.Log("E Story 预设已重置");
        }
    }
}
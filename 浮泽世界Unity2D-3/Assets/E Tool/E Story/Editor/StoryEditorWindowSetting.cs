using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using E.Tool;

namespace E.Tool
{
    [Serializable]
    public class StoryEditorWindowSetting : StaticDataDictionary<StoryEditorWindowSetting>
    {
        [Header("资源文件夹")]
        [Tooltip("在故事编辑窗口内点击右键创建的资源将会在此目录下创建对应文件")]
        public string StoryResourcesFolder = "Assets/E Tool/E Story/Resources/Example Storys";

        [Header("个性化界面")]
        public int ViewWidth = 3000;
        public int ViewHeight = 3000;
        public Vector2Int DefaultNodeSize = new Vector2Int(250, 90);
        public Color NormalNode = new Color(0.9f, 0.9f, 0.9f);
        public Color SelectNode = new Color(0.9f, 0.85f, 0.5f);
        public Color MainLine = new Color(0, 0.7f, 0);
        public Color BranchLine = new Color(0.7f, 0, 0);
        public Color BGLine = new Color(0, 0, 0, 0.1f);

        public void Reset()
        {
            StoryResourcesFolder = "Assets/E Tool/E Story/Resources/Example Storys";

            ViewWidth = 3000;
            ViewHeight = 3000;
            DefaultNodeSize = new Vector2Int(250, 90);
            NormalNode = new Color(0.9f, 0.9f, 0.9f);
            SelectNode = new Color(0.9f, 0.85f, 0.5f);
            MainLine = new Color(0, 0.7f, 0);
            BranchLine = new Color(0.7f, 0, 0);
            BGLine = new Color(0, 0, 0, 0.1f);
        }
    }

    public class StoryEditorSettingWindow : EditorWindow
    {
        private static StoryEditorWindowSetting setting;

        [PreferenceItem("E Writer")]
        private static void OnSetting()
        {
            if (!setting)
            {
                setting = StoryEditorWindowSetting.GetValues()[0];
            }

            EditorGUILayout.LabelField("布局", EditorStyles.boldLabel);
            setting.ViewWidth = EditorGUILayout.IntField("画布宽度", setting.ViewWidth);
            setting.ViewHeight = EditorGUILayout.IntField("画布高度", setting.ViewHeight);
            setting.DefaultNodeSize = EditorGUILayout.Vector2IntField("默认节点尺寸", setting.DefaultNodeSize);

            EditorGUILayout.LabelField("颜色", EditorStyles.boldLabel);
            setting.BGLine = EditorGUILayout.ColorField("背景网格", setting.BGLine);
            setting.NormalNode = EditorGUILayout.ColorField("默认节点", setting.NormalNode);
            setting.SelectNode = EditorGUILayout.ColorField("选中节点", setting.SelectNode);
            setting.MainLine = EditorGUILayout.ColorField("主线结点连接线", setting.MainLine);
            setting.BranchLine = EditorGUILayout.ColorField("支线结点连接线", setting.BranchLine);


            //EditorGUI.BeginChangeCheck();
            //EditorValues.IsProjectOwner = EditorGUILayout.Toggle("Is Owner", EditorValues.IsProjectOwner);
        }
    }
}
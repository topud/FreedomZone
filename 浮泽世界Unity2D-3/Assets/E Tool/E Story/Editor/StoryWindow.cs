using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;
using System.Linq;
using System.Reflection;
using System.Collections;
using UnityEditorInternal;
using UnityEngine.AddressableAssets;

namespace E.Tool
{
    public class StoryWindow : EditorWindow
    {
        //运行数据
        /// <summary>
        /// 故事编辑器窗口
        /// </summary>
        public static StoryWindow instance;
        /// <summary>
        /// 当前节点
        /// </summary>
        private static StoryNode currentNode;
        /// <summary>
        /// 上节点
        /// </summary>
        private static StoryNode upNode;
        /// <summary>
        /// 下节点
        /// </summary>
        private static StoryNode downNode;
        /// <summary>
        /// 当前故事索引
        /// </summary>
        private static int currentStoryIndex = -1;

        /// <summary>
        /// 窗口实例
        /// </summary>
        public static StoryWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GetWindow<StoryWindow>();
                }
                return instance;
            }
        }
        /// <summary>
        /// 故事集合
        /// </summary>
        public static IList<Story> Storys
        {
            get => Addressables.LoadAssets<Story>("Story", null).Result;
        }
        /// <summary>
        /// 当前节点
        /// </summary>
        public static StoryNode CurrentNode
        {
            get => currentNode;
            set
            {
                if (currentNode == value) return;
                currentNode = value;

            }
        }
        /// <summary>
        /// 当前故事
        /// </summary>
        public static Story CurrentStory
        {
            get
            {
                if (CurrentStoryIndex >= 0 && CurrentStoryIndex < Storys.Count)
                {
                    return Storys[CurrentStoryIndex];
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 当前故事索引
        /// </summary>
        public static int CurrentStoryIndex
        {
            get
            {
                return currentStoryIndex;
            }
            set
            {
                if (currentStoryIndex != value)
                {
                    currentStoryIndex = value;
                }
            }
        }
        /// <summary>
        /// 当前故事名称集合
        /// </summary>
        public static string[] StoryNames
        {
            get
            {
                int count = Storys.Count;
                string[] keys;
                if (count > 0)
                {
                    keys = new string[count];
                    for (int i = 0; i < Storys.Count; i++)
                    {
                        keys[i] = Storys[i].name;
                    }
                }
                else
                {
                    keys = new string[0];
                }
                return keys;
            }
        }
        
        //窗口样式
        /// <summary>
        /// 滚动条位置
        /// </summary>
        private static Vector2Int scrollPos;
        /// <summary>
        /// X拖拽偏移
        /// </summary>
        private static int xOffset;
        /// <summary>
        /// Y拖拽偏移
        /// </summary>
        private static int yOffset;
        /// <summary>
        /// 窗口尺寸
        /// </summary>
        private static Rect View
        {
            get => new Rect(0, 0, StoryWindowPreference.ViewSize.x, StoryWindowPreference.ViewSize.y);
        }
        /// <summary>
        /// 光标位置
        /// </summary>
        private static Vector2Int mousePos;
        /// <summary>
        /// 单行高度
        /// </summary>
        private static float LineHeight
        { get => EditorGUIUtility.singleLineHeight; }
        /// <summary>
        /// 行间隔
        /// </summary>
        private static float LineSpacing
        { get => EditorGUIUtility.standardVerticalSpacing; }
        /// <summary>
        /// 单行高度
        /// </summary>
        private static float OneLine
        { get => LineHeight + LineSpacing; }

        //打开
        /// <summary>
        /// 打开窗口
        /// </summary>
        [MenuItem("Window/E Tool/E Story %#w")]
        public static void Open()
        {
            Initialize();
            Refresh();
            Debug.Log("已打开 E Story");
        }
        /// <summary>
        /// 打开故事
        /// </summary>
        /// <param name="story"></param>
        private static void OpenStory(Story story)
        {
            if (story != null)
            {
                for (int i = 0; i < Storys.Count; i++)
                {
                    if (Storys[i] == story)
                    {
                        OpenStory(i);
                        break;
                    }
                }
                Selection.activeObject = CurrentStory;
                Instance.ShowNotification(new GUIContent("已打开 " + CurrentStory.name));
            }
            else
            {
                Debug.LogError("未打开故事：对象为空");
            }
        }
        /// <summary>
        /// 打开故事
        /// </summary>
        /// <param name="story"></param>
        private static void OpenStory(int index)
        {
            if (index >= 0 && index < Storys.Count)
            {
                CurrentStoryIndex = index;
                Selection.activeObject = CurrentStory;
                Instance.ShowNotification(new GUIContent("已打开 " + CurrentStory.name));
            }
            else
            {
                Debug.LogError("未打开故事：对象为空");
            }
        }

        //关闭
        /// <summary>
        /// 关闭故事
        /// </summary>
        private static void CloseCurrentStory()
        {
            if (CurrentStory == null)
            {
                Instance.ShowNotification(new GUIContent("当前未打开故事"));
            }
            else
            {
                Instance.ShowNotification(new GUIContent("已关闭 " + CurrentStory.name));
                CurrentStoryIndex = -1;
            }
        }

        //初始化
        /// <summary>
        /// 重置窗口
        /// </summary>
        private static void Initialize()
        {
            Instance.titleContent = new GUIContent("E Story");
            Instance.minSize = new Vector2(450, 400);

            scrollPos = Vector2Int.zero;
            xOffset = 0;
            yOffset = 0;
        }

        //刷新
        /// <summary>
        /// 刷新窗口
        /// </summary>
        private static void Refresh()
        {
            ClearTempConnect();

            string names = "已载入故事 {";
            for (int i = 0; i < Storys.Count; i++)
            {
                if (i < Storys.Count - 1)
                {
                    names += Storys[i].name + ", ";
                }
                else
                {
                    names += Storys[i].name;
                }
            }
            names += "}";
            if (Storys.Count > 0)
            {
                Debug.Log(names);
            }
            else
            {
                Debug.Log("未找到任何故事，请确保其位于Resources文件夹内。");
            }
        }
        /// <summary>
        /// 刷新鼠标坐标
        /// </summary>
        private static void RefreshMousePosition()
        {
            mousePos = new Vector2Int((int)((Event.current.mousePosition.x) + scrollPos.x), (int)((Event.current.mousePosition.y) + scrollPos.y - OneLine - LineSpacing));
        }

        //创建
        /// <summary>
        /// 创建故事
        /// </summary>
        private static void CreateStory()
        {
            string path = EditorUtility.SaveFilePanelInProject("创建故事", "新故事", "Asset", "保存故事", StoryWindowPreference.StoryResourcesFolder);
            if (path == "") return;
            Story story = CreateInstance<Story>();
            AssetDatabase.CreateAsset(story, path);
            OpenStory(story);
            //AssetDatabase.Refresh();
        }
        /// <summary>
        /// 创建故事节点
        /// </summary>
        private static void CreateNode()
        {
            if (CurrentStory != null)
            {
                CurrentStory.CreateNode(new RectInt(mousePos.x, mousePos.y, StoryWindowPreference.NodeSize.x, StoryWindowPreference.NodeSize.y));
            }
            else
            {
                Instance.ShowNotification(new GUIContent("未指定故事"));
                if (EditorUtility.DisplayDialog("E Writer", "你需要先打开一个故事才能创建节点", "好的", "关闭"))
                {
                }
            }
        }
        /// <summary>
        /// 创建内容
        /// </summary>
        private static void CreateSentences()
        {
            if (CurrentNode != null)
            {
                string path = EditorUtility.SaveFilePanelInProject("创建剧情片段", "新剧情片段", "Asset", "保存剧情片段", StoryWindowPreference.StoryResourcesFolder);
                if (path == "") return;
                Sentences sentences = CreateInstance<Sentences>();
                AssetDatabase.CreateAsset(sentences, path);
                CurrentNode.sentences = sentences;
                //AssetDatabase.Refresh();
            }
        }

        //删除
        /// <summary>
        /// 删除故事
        /// </summary>
        private static void DeleteCurrentStory()
        {
            if (CurrentStory != null)
            {
                string str = "确认要删除故事 {" + CurrentStory.name + "} 以及其所有节点吗？本地文件也将一并删除，这将无法恢复。";
                if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
                {
                    if (CurrentStory.nodes != null)
                    {
                        CurrentStory.nodes.Clear();
                        Debug.Log("已删除所有 {" + CurrentStory.name + "} 的故事节点");
                    }
                    string path = GetCurrentStoryPath();
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();
                    Debug.Log("已删除故事 {" + path + "}");
                    CurrentStoryIndex = -1;
                }
            }
            else
            {
                Instance.ShowNotification(new GUIContent("当前未打开故事"));
            }
        }

        //保存
        /// <summary>
        /// 删除故事
        /// </summary>
        private static void SaveCurrentStory()
        {
            if (CurrentStory != null)
            {
                AssetDatabase.SaveAssets();
                Instance.ShowNotification(new GUIContent("已保存 " + CurrentStory.name));
            }
            else
            {
                Instance.ShowNotification(new GUIContent("当前未打开故事"));
            }
        }

        //获取
        /// <summary>
        /// 获取当前故事的路径
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentStoryPath()
        {
            if (CurrentStory != null)
            {
                UnityEngine.Object ob = Selection.activeObject;
                Selection.activeObject = CurrentStory;
                string[] strs = Selection.assetGUIDs;
                Selection.activeObject = ob;
                return AssetDatabase.GUIDToAssetPath(strs[0]);
            }
            else return null;
        }

        //执行
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="obj"></param>
        private static void DoMethod(object obj)
        {
            switch (obj)
            {
                case 1:
                    CreateSentences();
                    break;
                case 2:
                    CreateNode();
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    CurrentStory.ClearNodeUpChoices(CurrentNode);
                    CurrentStory.ClearNodeDownChoices(CurrentNode);
                    break;
                case 6:
                    break;
                case 7:
                    CurrentStory.RemoveNode(CurrentNode);
                    break;
                case 8:
                    break;
                case 9:
                    CurrentStory.SetNodeType(CurrentNode, NodeType.起始节点);
                    break;
                case 10:
                    CurrentStory.SetNodeType(CurrentNode, NodeType.中间节点);
                    break;
                case 11:
                    CurrentStory.SetNodeType(CurrentNode, NodeType.结局节点);
                    break;
                case 12:
                    CurrentStory.ClearNodes();
                    break;

                default:
                    Debug.LogError("此右键菜单无实现方法");
                    break;
            }
        }

        //检查
        /// <summary>
        /// 检测鼠标事件
        /// </summary>
        private static void CheckMouseEvent()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (CurrentStory != null && CurrentStory.nodes != null)
                    {
                        //获取当前选中节点
                        bool isInNode = false;
                        foreach (StoryNode item in CurrentStory.nodes)
                        {
                            if (item.layout.Contains(mousePos))
                            {
                                xOffset = mousePos.x - item.layout.x;
                                yOffset = mousePos.y - item.layout.y;
                                isInNode = true;

                                //选中此节点
                                CurrentNode = item;
                                if (CurrentNode.sentences != null)
                                {
                                    Selection.activeObject = CurrentNode.sentences;
                                }
                                break;
                            }
                        }

                        if (isInNode)
                        {
                        }
                        else
                        {
                            CurrentNode = null;
                            Selection.activeObject = CurrentStory;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    break;
                case EventType.MouseMove:
                    RefreshMousePosition();
                    break;
                case EventType.MouseDrag:
                    RefreshMousePosition();
                    if (CurrentStory)
                    {
                        if (CurrentStory.nodes.Contains(CurrentNode))
                        {
                            if (CurrentNode.layout.Contains(mousePos))
                            {
                                CurrentNode.layout = new RectInt(mousePos.x - xOffset, mousePos.y - yOffset, CurrentNode.layout.width, CurrentNode.layout.height);
                            }
                        }
                    }
                    break;
                case EventType.KeyDown:
                    break;
                case EventType.KeyUp:
                    if (Event.current.keyCode == KeyCode.Delete && CurrentNode != null)
                    {
                        CurrentStory.RemoveNode(CurrentNode);
                    }
                    break;
                case EventType.ScrollWheel:
                    break;
                case EventType.Repaint:
                    break;
                case EventType.Layout:
                    break;
                case EventType.DragUpdated:
                    break;
                case EventType.DragPerform:
                    break;
                case EventType.DragExited:
                    break;
                case EventType.Ignore:
                    break;
                case EventType.Used:
                    break;
                case EventType.ValidateCommand:
                    break;
                case EventType.ExecuteCommand:
                    break;
                case EventType.ContextClick:
                    if (upNode == null && downNode == null)
                    {
                        DrawContextMenu();
                        //设置该事件被使用
                        Event.current.Use();
                    }
                    else
                    {
                        ClearTempConnect();
                    }
                    break;
                case EventType.MouseEnterWindow:
                    break;
                case EventType.MouseLeaveWindow:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 检查节点连接
        /// </summary>
        private static void CheckNodeConnect()
        {
            if (upNode != null && downNode != null)
            {
                if (upNode != downNode)
                {
                    if (upNode.nodeOptions == null)
                    {
                        upNode.nodeOptions = new List<StoryNodeOption>();
                    }
                    bool isHave = false;
                    foreach (StoryNodeOption item in upNode.nodeOptions)
                    {
                        if (item.id.Equals(downNode.id))
                        {
                            isHave = true;
                            break;
                        }
                    }
                    if (!isHave)
                    {
                        StoryNodeOption sc = new StoryNodeOption(downNode.id);
                        upNode.nodeOptions.Add(sc);
                    }
                    else
                    {
                        Instance.ShowNotification(new GUIContent("已存在连接"));
                    }
                }
                else
                {
                    Instance.ShowNotification(new GUIContent("不可连接自己"));
                }
                ClearTempConnect();
            }
        }

        //清除
        /// <summary>
        /// 清除节点临时连接
        /// </summary>
        private static void ClearTempConnect()
        {
            upNode = null;
            downNode = null;
        }

        //绘制
        /// <summary>
        /// 绘制窗口背景
        /// </summary>
        private static void DrawBG()
        {
            int xMin = (int)(scrollPos.x);
            int xMax = (int)(scrollPos.x + Instance.position.width);
            int yMin = (int)(scrollPos.y);
            int yMax = (int)(scrollPos.y + Instance.position.height);
            int xStart = xMin - xMin % 100;
            int yStart = yMin - yMin % 100;
            //画垂线
            for (int i = xStart; i <= xMax; i += 100)
            {
                Vector3 start = new Vector3(i, scrollPos.y, 0);
                Vector3 end = new Vector3(i, scrollPos.y + Instance.position.height, 0);
                DrawLine(start, end);
            }
            //画平线
            for (int i = yStart; i <= yMax; i += 100)
            {
                Vector3 start = new Vector3(scrollPos.x, i, 0);
                Vector3 end = new Vector3(scrollPos.x + Instance.position.width, i, 0);
                DrawLine(start, end);
            }
        }
        /// <summary>
        /// 绘制窗口头
        /// </summary>
        private static void DrawHead()
        {
            Rect panel = new Rect(0, 0, Instance.position.width, OneLine + LineSpacing);
            EditorGUI.DrawRect(panel, StoryWindowPreference.NormalNode);

            //故事列表下拉框
            Rect r1 = new Rect(LineSpacing, LineSpacing, 160, LineHeight);
            int index =  EditorGUI.Popup(r1, CurrentStoryIndex, StoryNames);
            if (index != CurrentStoryIndex)
            {
                OpenStory(index);
            }

            //快捷按钮
            if (GUI.Button(new Rect(160 + LineSpacing * 2, LineSpacing, 40, LineHeight), "创建"))
            {
                CreateStory();
            }
            if (GUI.Button(new Rect(200 + LineSpacing * 3, LineSpacing, 40, LineHeight), "删除"))
            {
                DeleteCurrentStory();
            }
            if (GUI.Button(new Rect(240 + LineSpacing * 4, LineSpacing, 40, LineHeight), "关闭"))
            {
                CloseCurrentStory();
            }
            if (GUI.Button(new Rect(280 + LineSpacing * 5, LineSpacing, 40, LineHeight), "保存"))
            {
                SaveCurrentStory();
            }
            if (GUI.Button(new Rect(320 + LineSpacing * 6, LineSpacing, 40, LineHeight), "刷新"))
            {
                Refresh();
            }

            if (GUI.Button(new Rect(Instance.position.width - 40 - LineSpacing, LineSpacing, 40, LineHeight), "设置"))
            {
                Instance.ShowNotification(new GUIContent("请前往 Edit -> Preferences -> E Story 进行编辑"));
            }
        }
        /// <summary>
        /// 绘制窗口足
        /// </summary>
        private static void DrawFoot()
        {
            Rect panel = new Rect(0, Instance.position.height - OneLine - LineSpacing, Instance.position.width, OneLine + LineSpacing);
            EditorGUI.DrawRect(panel, StoryWindowPreference.NormalNode);

            string mousePos = "X: " + StoryWindow.mousePos.x + "  Y: " + StoryWindow.mousePos.y;
            Rect r1 = new Rect(LineSpacing, panel.y + LineSpacing, 110, LineHeight);
            EditorGUI.LabelField(r1, mousePos);

            string currenStoryPath = GetCurrentStoryPath();
            if (currenStoryPath == null)
            {
                currenStoryPath = "无（创建或打开一个故事）";
            }
            Rect r2 = new Rect(LineSpacing * 2 + 110, panel.y + LineSpacing, panel.width - (LineSpacing * 3 + 110), LineHeight);
            EditorGUI.LabelField(r2, "当前故事：" + currenStoryPath);
        }
        /// <summary>
        /// 绘制右键菜单
        /// </summary>
        private static void DrawContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            if (CurrentStory != null)
            {
                menu.AddItem(new GUIContent("创建节点"), false, DoMethod, 2);

                if (CurrentNode != null)
                {
                    menu.AddItem(new GUIContent("创建剧情片段"), false, DoMethod, 1);
                    menu.AddItem(new GUIContent("删除节点"), false, DoMethod, 7);
                    menu.AddItem(new GUIContent("删除所有节点"), false, DoMethod, 12);
                    menu.AddItem(new GUIContent("设为起始节点"), false, DoMethod, 9);
                    menu.AddItem(new GUIContent("设为中间节点"), false, DoMethod, 10);
                    menu.AddItem(new GUIContent("设为结局节点"), false, DoMethod, 11);
                    menu.AddItem(new GUIContent("清除连接"), false, DoMethod, 5);
                }
                else
                {
                    if (CurrentStory.nodes != null)
                    {
                        menu.AddItem(new GUIContent("删除所有节点"), false, DoMethod, 12);
                    }
                }
            }
            menu.ShowAsContext();
        }
        /// <summary>
        /// 绘制故事
        /// </summary>
        private static void DrawStory()
        {
            if (CurrentStory != null)
            {
                if (CurrentStory.nodes != null)
                {
                    foreach (StoryNode item in CurrentStory.nodes)
                    {
                        DrawNode(item);
                    }
                }
            }
        }
        /// <summary>
        /// 绘制节点
        /// </summary>
        private static void DrawNode(StoryNode node)
        {
            if (node != null)
            {
                int lines = 0;
                foreach (StoryNodeOption item in node.nodeOptions)
                {
                    lines += 1 + item.conditions.Count;
                }
                node.layout = new RectInt(node.layout.x, node.layout.y, StoryWindowPreference.NodeSize.x, (int)(StoryWindowPreference.NodeSize.y + OneLine * lines + LineSpacing));
                Rect re = new Rect(node.layout.x, node.layout.y, node.layout.width, node.layout.height);
                //节点背景
                if (CurrentNode != null)
                {
                    if (CurrentNode == node)
                    {
                        EditorGUI.DrawRect(re, StoryWindowPreference.SelectNode);
                    }
                    else
                    {
                        EditorGUI.DrawRect(re, StoryWindowPreference.NormalNode);
                    }
                }
                else
                {
                    EditorGUI.DrawRect(re, StoryWindowPreference.NormalNode);
                }
                //节点类型
                switch (node.Type)
                {
                    case NodeType.中间节点:
                        DrawUpButton(node);
                        DrawDownButton(node);
                        break;
                    case NodeType.起始节点:
                        DrawDownButton(node);
                        break;
                    case NodeType.结局节点:
                        DrawUpButton(node);
                        break;
                    default:
                        break;
                }

                //编号
                Rect r1 = new Rect(node.layout.x + LineSpacing, node.layout.y + LineSpacing, node.layout.width - LineSpacing * 2, LineHeight);
                EditorGUI.LabelField(new Rect(r1.x, r1.y, 60, LineHeight), "节点编号");
                int chapter = EditorGUI.IntField(new Rect(r1.x + 60+ LineSpacing, r1.y, 30, LineHeight), node.id.chapter);
                int scene = EditorGUI.IntField(new Rect(r1.x + 60 + LineSpacing + (30 + LineSpacing) * 1, r1.y, 30, LineHeight), node.id.scene);
                int part = EditorGUI.IntField(new Rect(r1.x + 60 + LineSpacing + (30 + LineSpacing) * 2, r1.y, 30, LineHeight), node.id.part);
                int branch = EditorGUI.IntField(new Rect(r1.x + 60 + LineSpacing + (30 + LineSpacing) * 3, r1.y, 30, LineHeight), node.id.branch);
                NodeID id = new NodeID(chapter, scene, part, branch);
                if (!id.Equals(node.id))
                {
                    CurrentStory.SetNodeID(node, id);
                }

                //主线
                Rect r2 = new Rect(node.layout.x + node.layout.width - 60 - LineSpacing, node.layout.y + LineSpacing, 60, LineHeight);
                bool ismain = EditorGUI.ToggleLeft(r2, "主线", node.isMainNode);
                if (ismain != node.isMainNode)
                {
                    node.isMainNode = ismain;
                }

                //简介
                Rect r4 = new Rect(r1.x, r1.y + OneLine, 60, LineHeight);
                EditorGUI.LabelField(r4, "节点简介");
                Rect r3 = new Rect(r1.x + 60 + LineSpacing, r1.y + OneLine, r1.width- 60- LineSpacing, StoryWindowPreference.NodeSize.y - OneLine*2 - LineSpacing);
                node.description = EditorGUI.TextArea(r3, node.description);

                //内容
                Rect r5 = new Rect(r4.x, r3.y + r3.height +LineSpacing, 60, LineHeight);
                EditorGUI.LabelField(r5, "剧情片段");
                Rect r6 = new Rect(r3.x, r3.y + +r3.height + LineSpacing, r3.width, LineHeight);
                node.sentences = (Sentences)EditorGUI.ObjectField(r6, node.sentences, typeof(Sentences));
                
                //分支
                DrawNodeOptions(node);
            }
        }
        /// <summary>
        /// 绘制下个节点
        /// </summary>
        private static void DrawNodeOptions(StoryNode node)
        {
            for (int i = 0; i < node.nodeOptions.Count; i++)
            {
                if (CurrentStory.ContainsID(node.nodeOptions[i].id))
                {
                    DrawNodeOption(node, node.nodeOptions[i], out bool isRemove);
                    if (isRemove)
                    {
                        node.nodeOptions.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    node.nodeOptions.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// 绘制分支选项
        /// </summary>
        private static int DrawNodeOption(StoryNode node, StoryNodeOption nodeOption, out bool isRemove)
        {
            int index = node.nodeOptions.IndexOf(nodeOption);

            int yOffsetLine = 0;
            for (int i = index - 1; i > -1; i--)
            {
                yOffsetLine += node.nodeOptions[i].conditions.Count + 1;
            }
            Rect nodeOptionRect = new Rect(node.layout.x, node.layout.y + StoryWindowPreference.NodeSize.y + OneLine * yOffsetLine+ LineSpacing, node.layout.width, OneLine * (1 + nodeOption.conditions.Count));
            //Rect nodeOptionRect = new Rect(nodeEditPanel.x, nodeEditPanel.y + OneLine * (yOffsetLine + 6) + LineSpacing, nodeEditPanel.width, OneLine * (1 + nodeOption.conditions.Count));
            NodeID nextNodeID = nodeOption.id;

            //删除按钮
            isRemove = GUI.Button(new Rect(nodeOptionRect.x + nodeOptionRect.width, nodeOptionRect.y, LineHeight, LineHeight), "X");

            //编号
            string label = string.Format("{0}-{1}-{2}-{3}", nextNodeID.chapter, nextNodeID.scene, nextNodeID.part, nextNodeID.branch);
            Rect r2 = new Rect(nodeOptionRect.x + LineSpacing, nodeOptionRect.y, 60, LineHeight);
            EditorGUI.LabelField(r2, label);

            //描述
            Rect r1 = new Rect(nodeOptionRect.x + 60 + LineSpacing * 2, nodeOptionRect.y, nodeOptionRect.width - 60 - LineHeight - LineSpacing * 4, LineHeight);
            nodeOption.description = EditorGUI.TextField(r1, nodeOption.description);

            //条件
            Rect r3 = new Rect(nodeOptionRect.x + nodeOptionRect.width - LineHeight - LineSpacing, nodeOptionRect.y, LineHeight, LineHeight);
            if (GUI.Button(r3, "+"))
            {
                nodeOption.conditions.Add(new ConditionComparison());
            }

            for (int j = 0; j < nodeOption.conditions.Count; j++)
            {
                Rect re = new Rect(nodeOptionRect.x + 60 + LineSpacing * 2, nodeOptionRect.y + OneLine * (j + 1), nodeOptionRect.width - 60 - LineSpacing * 2, LineHeight);

                Rect re1 = new Rect(re.x, re.y, 80, LineHeight);
                nodeOption.conditions[j].keyIndex = EditorGUI.Popup(re1, nodeOption.conditions[j].keyIndex, CurrentStory.ConditionKeys);

                Rect re2 = new Rect(re.x + 80 + LineSpacing, re.y, 80, LineHeight);
                nodeOption.conditions[j].comparison = (Comparison)EditorGUI.EnumPopup(re2, GUIContent.none, nodeOption.conditions[j].comparison);

                Rect re3 = new Rect(re.x + 160 + LineSpacing * 2, re.y, re.width - 160 - LineSpacing * 4 - LineHeight, LineHeight);
                nodeOption.conditions[j].value = EditorGUI.IntField(re3, GUIContent.none, nodeOption.conditions[j].value);

                Rect re4 = new Rect(re.x + re.width - LineHeight - LineSpacing, re.y, LineHeight, LineHeight);
                if (GUI.Button(re4, "X"))
                {
                    nodeOption.conditions.RemoveAt(j);
                    j--;
                }
            }


            //绘制连接曲线
            StoryNode nnode = CurrentStory.GetNode(nextNodeID);
            Vector2 start = new Vector2(nodeOptionRect.x + nodeOptionRect.width + 20, nodeOptionRect.y + 10);
            Vector2 end = new Vector2(nnode.layout.x - 20, nnode.layout.y + StoryWindowPreference.NodeSize.y / 2);
            DrawCurve(start, end, node.isMainNode && nnode.isMainNode, Vector3.right, Vector3.left);

            return (int)nodeOptionRect.height;
        }
        /// <summary>
        /// 绘制节点上按钮
        /// </summary>
        /// <param name="node"></param>
        private static void DrawUpButton(StoryNode node)
        {
            Rect rect = new Rect(node.layout.x - LineHeight, node.layout.y, LineHeight, StoryWindowPreference.NodeSize.y);
            if (GUI.Button(rect, "←"))
            {
                downNode = node;
                CheckNodeConnect();
            }
        }
        /// <summary>
        /// 绘制节点下按钮
        /// </summary>
        /// <param name="node"></param>
        private static void DrawDownButton(StoryNode node)
        {
            Rect rect = new Rect(node.layout.x + node.layout.width, node.layout.y, LineHeight, StoryWindowPreference.NodeSize.y);
            if (GUI.Button(rect, "→"))
            {
                upNode = node;
                CheckNodeConnect();
            }
        }
        /// <summary>
        /// 绘制节点预览连接线
        /// </summary>
        private static void DrawTempCurve()
        {
            if (upNode != null)
            {
                Vector2 start = new Vector2(upNode.layout.x + upNode.layout.width + OneLine, upNode.layout.y + StoryWindowPreference.NodeSize.y / 2);
                Vector2 end = mousePos;
                DrawCurve(start, end, true, Vector3.right, Vector3.left);
            }
            if (downNode != null)
            {
                Vector2 start = mousePos;
                Vector2 end = new Vector2(downNode.layout.x - OneLine, downNode.layout.y + StoryWindowPreference.NodeSize.y / 2);
                DrawCurve(start, end, true, Vector3.right, Vector3.left);
            }
        }
        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="start">起始节点窗口</param>
        /// <param name="end">结束节点窗口</param>
        /// <param name="isMainLine">是否绘制成主线高亮颜色</param>
        private static void DrawCurve(Vector2 start, Vector2 end, bool isMainLine, Vector3 startDirc, Vector3 endDirc)
        {
            Vector3 startPos = new Vector3(start.x, start.y, 0);
            Vector3 endPos = new Vector3(end.x, end.y, 0);
            Vector3 startTan = startPos + startDirc * 50;
            Vector3 endTan = endPos + endDirc * 50;
            //绘制阴影
            for (int i = 0; i < 3; i++)
            {
                // Handles.DrawBezier(startPos, endPos, startTan, endTan, m_Shadow, null, (i + 1) * 5);
            }
            //绘制线条
            if (isMainLine)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, StoryWindowPreference.MainLine, null, 4);
            }
            else
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, StoryWindowPreference.BranchLine, null, 3);
            }
        }
        /// <summary>
        /// 绘制直线
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private static void DrawLine(Vector3 start, Vector3 end)
        {
            Handles.DrawBezier(start, end, start, end, new Color(0, 0, 0, 0.1f), null, 2);
        }

        //mono
        private void OnEnable()
        {
            Instance.wantsMouseMove = true;
            if (Storys.Count > 0)
            {
                OpenStory(0);
            }
        }
        private void Update()
        {
            Repaint();
        }
        private void OnGUI()
        {
            CheckMouseEvent();

            Rect rect = new Rect(0, OneLine + LineSpacing, position.width, position.height - (OneLine * 2 + LineSpacing * 2));
            Vector2 v = GUI.BeginScrollView(rect, scrollPos, View);
            scrollPos = new Vector2Int((int)v.x, (int)v.y);
            DrawBG();
            DrawTempCurve();
            DrawStory();
            GUI.EndScrollView();

            DrawHead();
            DrawFoot();
        }
    }
}
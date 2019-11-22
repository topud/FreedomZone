// ========================================================
// 作者：E Star
// 创建时间：2019-02-22 01:31:07
// 当前版本：1.0
// 作用描述：E Story 编辑器窗口类
// 挂载目标：无
// ========================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace E.Tool
{
    public class StoryEditorWindow : EditorWindow
    {
        //运行数据
        /// <summary>
        /// 故事编辑器窗口
        /// </summary>
        public static StoryEditorWindow instance;
        /// <summary>
        /// 故事列表
        /// </summary>
        private static List<Story> storys;
        /// <summary>
        /// 当前故事
        /// </summary>
        private static Story CurrentStory;
        /// <summary>
        /// 当前节点
        /// </summary>
        private static StoryNode CurrentNode;
        /// <summary>
        /// 上节点
        /// </summary>
        private static StoryNode UpNode;
        /// <summary>
        /// 下节点
        /// </summary>
        private static StoryNode DownNode;
        /// <summary>
        /// 当前故事索引
        /// </summary>
        private static int selectStoryIndex = -1;
        /// <summary>
        /// 是否显示节点编辑面板
        /// </summary>
        private static bool showNodeEditPanel = false;

        /// <summary>
        /// 窗口实例
        /// </summary>
        public static StoryEditorWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GetWindow<StoryEditorWindow>();
                }
                return instance;
            }
        }
        /// <summary>
        /// 故事集合
        /// </summary>
        public static List<Story> Storys
        {
            get
            {
                if (storys == null)
                {
                    storys = Story.GetValues();
                }
                return storys;
            }
        }
        /// <summary>
        /// 当前故事索引
        /// </summary>
        public static int SelectStoryIndex
        {
            get
            {
                return selectStoryIndex;
            }
            set
            {
                if (selectStoryIndex != value)
                {
                    selectStoryIndex = value;
                    if (selectStoryIndex == -1)
                    {
                        CloseCurrentStory();
                    }
                    else
                    {
                        OpenStory(Storys[selectStoryIndex]);
                    }
                }
            }
        }
        /// <summary>
        /// 当前故事名称集合
        /// </summary>
        public static string[] storyNames
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
        /// 窗口尺寸
        /// </summary>
        private static Rect View;
        /// <summary>
        /// 节点编辑面板
        /// </summary>
        private static Rect NodeEditPanel;
        /// <summary>
        /// 滚动条位置
        /// </summary>
        private static Vector2Int ScrollPos;
        /// <summary>
        /// 光标位置
        /// </summary>
        private static Vector2Int MousePos;
        /// <summary>
        /// X拖拽偏移
        /// </summary>
        private static int Xoffset;
        /// <summary>
        /// Y拖拽偏移
        /// </summary>
        private static int Yoffset;


        //打开
        /// <summary>
        /// 打开窗口
        /// </summary>
        [MenuItem("Tools/E Story/打开编辑器窗口 %#w", false, 0)]
        public static void Open()
        {
            Initialize();
            Refresh();
            Debug.Log("已打开故事编辑器");
        }
        /// <summary>
        /// 打开故事
        /// </summary>
        /// <param name="story"></param>
        private static void OpenStory(Story story)
        {
            if (story != null)
            {
                CurrentStory = story;
                Selection.activeObject = CurrentStory;
                Instance.ShowNotification(new GUIContent("已打开故事：" + CurrentStory.name));
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
            CurrentStory = null;
        }

        //初始化
        /// <summary>
        /// 重置窗口
        /// </summary>
        private static void Initialize()
        {
            Instance.titleContent = new GUIContent("E Story");

            View = new Rect(0, 0, StoryEditorWindowPreference.ViewSize.x, StoryEditorWindowPreference.ViewSize.y);
            ScrollPos = Vector2Int.zero;
            MousePos = Vector2Int.zero;
            Xoffset = 0;
            Yoffset = 0;
        }

        //刷新
        /// <summary>
        /// 刷新窗口
        /// </summary>
        private static void Refresh()
        {
            ClearTempConnect();
            RefreshStorys();

            string names = "已载入故事 {";
            for (int i = 0; i < Storys.Count; i++)
            {
                if (i < Storys.Count-1)
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
                CurrentStory = null;
            }
        }
        /// <summary>
        /// 刷新故事结合
        /// </summary>
        private static void RefreshStorys()
        {
            storys = Story.Dictionary.Values.ToList();
        }
        /// <summary>
        /// 刷新鼠标坐标
        /// </summary>
        private static void RefreshMousePosition()
        {
            MousePos = new Vector2Int((int)((Event.current.mousePosition.x) + ScrollPos.x), (int)((Event.current.mousePosition.y) + ScrollPos.y));
        }
        /// <summary>
        /// 刷新节点高度
        /// </summary>
        /// <param name="node"></param>
        private static int RefreshNodeHeight(StoryNode node)
        {
            int baseHeight = StoryEditorWindowPreference.NodeSize.y;
            if (node != null)
            {
                switch (node.ContentType)
                {
                    case ContentType.剧情对话:
                        baseHeight = 110;
                        break;
                    case ContentType.过场动画:
                        baseHeight = 110;
                        break;
                    default:
                        break;
                }
            }
            int addHeight = 0;
            if (node.NextNodes != null)
            {
                addHeight = node.NextNodes.Count * 20;
            }
            node.Rect.height = baseHeight + addHeight;
            return baseHeight;
        }

        //创建
        [MenuItem("Tools/E Story/创建故事", false, 1)]
        /// <summary>
        /// 创建故事
        /// </summary>
        private static void CreateStory()
        {
            string path = EditorUtility.SaveFilePanelInProject("创建故事", "新故事", "Asset", "保存故事", StoryEditorWindowPreference.StoryResourcesFolder);
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
                CurrentStory.CreateNode(new RectInt(MousePos.x, MousePos.y, StoryEditorWindowPreference.NodeSize.x, StoryEditorWindowPreference.NodeSize.y));
            }
            else
            {
                Instance.ShowNotification(new GUIContent("未指定故事"));
                if (EditorUtility.DisplayDialog("E Writer", "你需要先打开一个故事才能创建节点", "好的", "关闭"))
                {
                }
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
                    if (CurrentStory.Nodes != null)
                    {
                        CurrentStory.Nodes.Clear();
                        Debug.Log("已删除所有 {" + CurrentStory.name + "} 的故事节点");
                    }
                    string path = GetCurrentStoryPath();
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();
                    Debug.Log("已删除故事 {" + path + "}");
                }
            }
            else
            {
                Instance.ShowNotification(new GUIContent("未指定故事"));
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
                    //CurrentStory.RemoveNodeContent(CurrentNode);
                    break;
                case 7:
                    CurrentStory.RemoveNode(CurrentNode);
                    break;
                case 8:
                    SelectStoryIndex = -1;
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
                    RefreshMousePosition();
                    if (CurrentStory != null && CurrentStory.Nodes != null)
                    {
                        //获取当前选中节点
                        bool isInNode = false;
                        foreach (StoryNode item in CurrentStory.Nodes)
                        {
                            if (item.Rect.Contains(MousePos))
                            {
                                Xoffset = MousePos.x - item.Rect.x;
                                Yoffset = MousePos.y - item.Rect.y;
                                isInNode = true;
                                //选中此节点
                                CurrentNode = item;
                                //只展开此节点
                                foreach (StoryNode it in CurrentStory.Nodes)
                                {
                                    it.IsFold = false;
                                }
                                item.IsFold = true;
                                //选中节点内容资源
                                if (CurrentNode != null)
                                {
                                    showNodeEditPanel = true;
                                }
                                else
                                {
                                    showNodeEditPanel = false;
                                    Selection.activeObject = CurrentStory;
                                }
                                break;
                            }
                        }

                        if (isInNode)
                        {
                            //将选中节点置顶显示
                            CurrentStory.Nodes.Remove(CurrentNode);
                            CurrentStory.Nodes.Insert(CurrentStory.Nodes.Count, CurrentNode);
                        }
                        else
                        {
                            if (NodeEditPanel.Contains(MousePos))
                            {
                            }
                            else
                            {
                                CurrentNode = null;
                                Selection.activeObject = CurrentStory;
                            }
                        }
                    }
                    break;
                case EventType.MouseUp:
                    RefreshMousePosition();
                    break;
                case EventType.MouseMove:
                    RefreshMousePosition();
                    break;
                case EventType.MouseDrag:
                    RefreshMousePosition();
                    if (CurrentStory)
                    {
                        if (CurrentStory.Nodes.Contains(CurrentNode))
                        {
                            if (CurrentNode.Rect.Contains(MousePos))
                            {
                                CurrentNode.Rect = new RectInt(MousePos.x - Xoffset, MousePos.y - Yoffset, CurrentNode.Rect.width, CurrentNode.Rect.height);
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
                    RefreshMousePosition();
                    if (UpNode == null && DownNode == null)
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
            if (UpNode != null && DownNode != null)
            {
                if (UpNode != DownNode)
                {
                    if (UpNode.NextNodes == null)
                    {
                        UpNode.NextNodes = new List<NextNode>();
                    }
                    bool isHave = false;
                    foreach (NextNode item in UpNode.NextNodes)
                    {
                        if (item.ID.Equals(DownNode.ID))
                        {
                            isHave = true;
                            break;
                        }
                    }
                    if (!isHave)
                    {
                        NextNode sc = new NextNode("", DownNode.ID);
                        UpNode.NextNodes.Add(sc);
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
            UpNode = null;
            DownNode = null;
        }

        //绘制
        /// <summary>
        /// 绘制窗口背景
        /// </summary>
        private static void DrawBG()
        {
            int xMin = (int)(ScrollPos.x);
            int xMax = (int)(ScrollPos.x + Instance.position.width);
            int yMin = (int)(ScrollPos.y);
            int yMax = (int)(ScrollPos.y + Instance.position.height);
            int xStart = xMin - xMin % 100;
            int yStart = yMin - yMin % 100;
            //画垂线
            for (int i = xStart; i <= xMax; i += 100)
            {
                Vector3 start = new Vector3(i, ScrollPos.y, 0);
                Vector3 end = new Vector3(i, ScrollPos.y + Instance.position.height, 0);
                DrawLine(start, end);
            }
            //画平线
            for (int i = yStart; i <= yMax; i += 100)
            {
                Vector3 start = new Vector3(ScrollPos.x, i, 0);
                Vector3 end = new Vector3(ScrollPos.x + Instance.position.width, i, 0);
                DrawLine(start, end);
            }

            View = new Rect(0, 0, StoryEditorWindowPreference.ViewSize.x, StoryEditorWindowPreference.ViewSize.y);
        }
        /// <summary>
        /// 绘制窗口头
        /// </summary>
        private static void DrawHead()
        {
            Rect panel = new Rect(0, 0, Instance.position.width, 20);
            EditorGUI.DrawRect(panel, StoryEditorWindowPreference.NormalNode);

            //故事列表下拉框
            SelectStoryIndex = EditorGUI.Popup(new Rect(0, 0, 160, 20), SelectStoryIndex, storyNames);

            //快捷按钮
            if (GUI.Button(new Rect(160, 0, 40, 20), "创建"))
            {
                CreateStory();
            }
            if (GUI.Button(new Rect(200, 0, 40, 20), "删除"))
            {
                DeleteCurrentStory();
            }
            if (GUI.Button(new Rect(240, 0, 40, 20), "保存"))
            {
                AssetDatabase.SaveAssets();
            }
            if (GUI.Button(new Rect(280, 0, 40, 20), "刷新"))
            {
                Refresh();
            }

            if (GUI.Button(new Rect(Instance.position.width - 40, 0, 40, 20), "设置"))
            {
                Instance.ShowNotification(new GUIContent("请前往 Edit -> Preferences -> E Writer 进行编辑"));
            }
        }
        /// <summary>
        /// 绘制窗口足
        /// </summary>
        private static void DrawFoot()
        {
            Rect panel = new Rect(0, Instance.position.height - 20, Instance.position.width, 20);
            EditorGUI.DrawRect(panel, StoryEditorWindowPreference.NormalNode);

            string mousePos = "X: " + MousePos.x + "  Y: " + MousePos.y;
            EditorGUI.LabelField(new Rect(5, Instance.position.height - 20, 110, 20), mousePos);
            string currenStoryPath = GetCurrentStoryPath();
            if (currenStoryPath == null)
            {
                currenStoryPath = "无";
            }
            EditorGUI.LabelField(new Rect(115, Instance.position.height - 20, Instance.position.width, 20), "当前故事：" + currenStoryPath);
        }
        /// <summary>
        /// 绘制节点编辑面板
        /// </summary>
        private static void DrawNodeEditPanel()
        {
            if (showNodeEditPanel)
            {
                NodeEditPanel = new Rect(Instance.position.width - 330, 30, 300, 300);
                EditorGUI.DrawRect(NodeEditPanel, StoryEditorWindowPreference.NormalNode);

                if (CurrentNode != null)
                {
                    //主线
                    bool ismain = EditorGUI.ToggleLeft(new Rect(NodeEditPanel.x + NodeEditPanel.width - 45, NodeEditPanel.y + 25, 100, 16), "主线", CurrentNode.IsMainNode);
                    if (ismain != CurrentNode.IsMainNode)
                    {
                        CurrentNode.IsMainNode = ismain;
                    }

                    //编号
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 40, NodeEditPanel.y + 5, 30, 16), "章节");
                    int chapter = EditorGUI.IntField(new Rect(NodeEditPanel.x + 40, NodeEditPanel.y + 25, 30, 16), CurrentNode.ID.Chapter);
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 75, NodeEditPanel.y + 5, 30, 16), "场景");
                    int scene = EditorGUI.IntField(new Rect(NodeEditPanel.x + 75, NodeEditPanel.y + 25, 30, 16), CurrentNode.ID.Scene);
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 110, NodeEditPanel.y + 5, 30, 16), "片段");
                    int part = EditorGUI.IntField(new Rect(NodeEditPanel.x + 110, NodeEditPanel.y + 25, 30, 16), CurrentNode.ID.Part);
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 145, NodeEditPanel.y + 5, 30, 16), "分支");
                    int branch = EditorGUI.IntField(new Rect(NodeEditPanel.x + 145, NodeEditPanel.y + 25, 30, 16), CurrentNode.ID.Branch);
                    NodeID id = new NodeID(chapter, scene, part, branch);
                    if (!id.Equals(CurrentNode.ID))
                    {
                        CurrentStory.SetNodeID(CurrentNode, id);
                    }

                    //形式
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 5, NodeEditPanel.y + 65, 40, 16), "形式");
                    CurrentNode.ContentType = (ContentType)EditorGUI.EnumPopup(new Rect(NodeEditPanel.x + 40, NodeEditPanel.y + 65, 135, 20), GUIContent.none, CurrentNode.ContentType);
                    
                    //时间
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 5, NodeEditPanel.y + 85, 40, 16), "时间");
                    int y = EditorGUI.IntField(new Rect(NodeEditPanel.x + 40, NodeEditPanel.y + 85, 35, 16), CurrentNode.Time.Year);
                    int mo = EditorGUI.IntField(new Rect(NodeEditPanel.x + 78f, NodeEditPanel.y + 85, 30, 16), CurrentNode.Time.Month);
                    int d = EditorGUI.IntField(new Rect(NodeEditPanel.x + 111, NodeEditPanel.y + 85, 30, 16), CurrentNode.Time.Day);
                    int h = EditorGUI.IntField(new Rect(NodeEditPanel.x + 145, NodeEditPanel.y + 85, 30, 16), CurrentNode.Time.Hour);
                    int mi = EditorGUI.IntField(new Rect(NodeEditPanel.x + 180, NodeEditPanel.y + 85, 30, 16), CurrentNode.Time.Minute);
                    int s = EditorGUI.IntField(new Rect(NodeEditPanel.x + 215, NodeEditPanel.y + 85, 30, 16), CurrentNode.Time.Second);
                    CurrentNode.Time = new DateTime(y, mo, d, h, mi, s);
                    
                    //地点
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 5, NodeEditPanel.y + 105, 40, 16), "地点");
                    CurrentNode.Position = EditorGUI.TextField(new Rect(NodeEditPanel.x + 40, NodeEditPanel.y + 105, NodeEditPanel.width - 45, 16), CurrentNode.Position);
                    
                    //概述
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 5, NodeEditPanel.y + 125, 40, 16), "概述");
                    CurrentNode.Summary = EditorGUI.TextArea(new Rect(NodeEditPanel.x + 40, NodeEditPanel.y + 125, NodeEditPanel.width - 45, 42), CurrentNode.Summary);

                }
                else
                {
                    EditorGUI.LabelField(new Rect(NodeEditPanel.x + 20, NodeEditPanel.y + 20, 120, 20), "请选择一个节点");
                }
            }
        }
        /// <summary>
        /// 绘制右键菜单
        /// </summary>
        private static void DrawContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            if (CurrentStory != null)
            {
                menu.AddItem(new GUIContent("关闭当前故事"), false, DoMethod, 8);
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("创建节点"), false, DoMethod, 2);

                if (CurrentNode != null)
                {
                    menu.AddItem(new GUIContent("删除节点"), false, DoMethod, 7);
                    menu.AddItem(new GUIContent("删除所有节点"), false, DoMethod, 12);
                    menu.AddItem(new GUIContent("设为起始节点"), false, DoMethod, 9);
                    menu.AddItem(new GUIContent("设为中间节点"), false, DoMethod, 10);
                    menu.AddItem(new GUIContent("设为结局节点"), false, DoMethod, 11);
                    menu.AddItem(new GUIContent("清除连接"), false, DoMethod, 5);
                }
                else
                {
                    if (CurrentStory.Nodes != null)
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
                if (CurrentStory.Nodes != null)
                {
                    foreach (StoryNode item in CurrentStory.Nodes)
                    {
                        DrawNode(item);
                    }
                }
            }
            else
            {
                Instance.ShowNotification(new GUIContent("创建或打开一个故事"));
            }
        }
        /// <summary>
        /// 绘制节点
        /// </summary>
        private static void DrawNode(StoryNode node)
        {
            if (node != null)
            {
                Rect nodeRect = new Rect(node.Rect.x, node.Rect.y, node.Rect.width, node.Rect.height);

                //节点背景
                if (CurrentNode != null)
                {
                    if (CurrentNode == node)
                    {
                        EditorGUI.DrawRect(nodeRect, StoryEditorWindowPreference.SelectNode);
                    }
                    else
                    {
                        EditorGUI.DrawRect(nodeRect, StoryEditorWindowPreference.NormalNode);
                    }
                }
                else
                {
                    EditorGUI.DrawRect(nodeRect, StoryEditorWindowPreference.NormalNode);
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

                //主线
                bool ismain = EditorGUI.ToggleLeft(new Rect(nodeRect.x + nodeRect.width - 45, nodeRect.y + 5, 100, 16), "主线", node.IsMainNode);
                if (ismain != node.IsMainNode)
                {
                    node.IsMainNode = ismain;
                }

                //编号
                string id = string.Format("{0}-{1}-{2}-{3}", node.ID.Chapter, node.ID.Scene, node.ID.Part, node.ID.Branch);
                EditorGUI.LabelField(new Rect(nodeRect.x + 5, nodeRect.y + 5, 200, 16), id);

                //时间
                string time = string.Format("{0}/{1}/{2} {3}:{4}:{5}", node.Time.Year, node.Time.Month,
                    node.Time.Day, node.Time.Hour, node.Time.Minute, node.Time.Second);
                EditorGUI.LabelField(new Rect(nodeRect.x + 5, nodeRect.y + 25, 200, 16), time);

                //地点
                EditorGUI.LabelField(new Rect(nodeRect.x + 5, nodeRect.y + 45, 200, 16), node.Position);

                //概述
                node.Summary = EditorGUI.TextArea(new Rect(nodeRect.x + 5, nodeRect.y + 65, 200, 32), node.Summary);

                int baseHeight = RefreshNodeHeight(node);

                //后续节点
                for (int i = 0; i < node.NextNodes.Count; i++)
                {
                    NodeID nextNodeID = node.NextNodes[i].ID;
                    if (CurrentStory.ContainsID(nextNodeID))
                    {
                        int addWidth = (nextNodeID.Chapter / 10 + nextNodeID.Scene / 10 + nextNodeID.Part / 10 + nextNodeID.Branch / 10) * 7;

                        string label = string.Format("{0}-{1}-{2}-{3}", nextNodeID.Chapter, nextNodeID.Scene, nextNodeID.Part, nextNodeID.Branch);
                        EditorGUI.LabelField(new Rect(nodeRect.x + 5, nodeRect.y + 20 * i + baseHeight, 60 + addWidth, 16), label);

                        node.NextNodes[i].Describe = EditorGUI.TextField(new Rect(nodeRect.x + 65 + addWidth, nodeRect.y + 20 * i + baseHeight, nodeRect.width - 95 - addWidth, 16), node.NextNodes[i].Describe);

                        for (int j = 0; j < node.NextNodes[i].Conditions.Count; j++)
                        {
                            node.NextNodes[i].Conditions[j].SetIndex
                                ( EditorGUI.Popup(new Rect(nodeRect.x + 10, nodeRect.y + 20 * i + baseHeight + 20 * (j + 1), 80 + addWidth, 16),
                                node.NextNodes[i].Conditions[j].KeyIndex, CurrentStory.ConditionKeys) );
                            node.NextNodes[i].Conditions[j].SetComparison
                                ((Comparison)EditorGUI.EnumPopup(new Rect(nodeRect.x + 90, nodeRect.y + 20 * i + baseHeight + 20 * (j + 1), 80 + addWidth, 16),
                                GUIContent.none, node.NextNodes[i].Conditions[j].Comparison));
                            node.NextNodes[i].Conditions[j].SetValue
                                (EditorGUI.IntField(new Rect(nodeRect.x + 170, nodeRect.y + 20 * i + baseHeight + 20 * (j + 1), 30 + addWidth, 16),
                                GUIContent.none, node.NextNodes[i].Conditions[j].Value));
                        }


                        if (GUI.Button(new Rect(nodeRect.x + nodeRect.width - 25, nodeRect.y + 20 * i + baseHeight, 20, 16), "X"))
                        {
                            node.NextNodes.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        node.NextNodes.RemoveAt(i);
                    }
                }
            }
        }
        /// <summary>
        /// 绘制节点上按钮
        /// </summary>
        /// <param name="node"></param>
        private static void DrawUpButton(StoryNode node)
        {
            if (GUI.Button(new Rect(node.Rect.x + node.Rect.width / 2 - 10, node.Rect.y - 20, 20, 20), "↑"))
            {
                DownNode = node;
                CheckNodeConnect();
            }
        }
        /// <summary>
        /// 绘制节点下按钮
        /// </summary>
        /// <param name="node"></param>
        private static void DrawDownButton(StoryNode node)
        {
            if (GUI.Button(new Rect(node.Rect.x + node.Rect.width / 2 - 10, node.Rect.y + node.Rect.height, 20, 20), "↓"))
            {
                UpNode = node;
                CheckNodeConnect();
            }
        }
        /// <summary>
        /// 绘制节点连接线
        /// </summary>
        private static void DrawNodeCurve()
        {
            if (CurrentStory != null)
            {
                if (CurrentStory.Nodes != null)
                {
                    foreach (StoryNode item in CurrentStory.Nodes)
                    {
                        if (item.NextNodes != null)
                        {
                            foreach (NextNode choice in item.NextNodes)
                            {
                                if (CurrentStory.ContainsID(choice.ID))
                                {
                                    StoryNode node = CurrentStory.GetNode(choice.ID);
                                    if (item.IsMainNode && node.IsMainNode)
                                    {
                                        DrawCurve(new Rect(item.Rect.x, item.Rect.y, item.Rect.width, item.Rect.height), new Rect(node.Rect.x, node.Rect.y, node.Rect.width, node.Rect.height), true);
                                    }
                                    else
                                    {
                                        DrawCurve(new Rect(item.Rect.x, item.Rect.y, item.Rect.width, item.Rect.height), new Rect(node.Rect.x, node.Rect.y, node.Rect.width, node.Rect.height), false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 绘制节点预览连接线
        /// </summary>
        private static void DrawTempCurve()
        {
            if (UpNode != null)
            {
                DrawCurve(new Rect(UpNode.Rect.x, UpNode.Rect.y, UpNode.Rect.width, UpNode.Rect.height), new Rect(MousePos.x, MousePos.y + 20, 0, 0), true);
            }
            if (DownNode != null)
            {
                DrawCurve(new Rect(MousePos.x, MousePos.y - 20, 0, 0), new Rect(DownNode.Rect.x, DownNode.Rect.y, DownNode.Rect.width, DownNode.Rect.height), true);
            }
        }
        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="start">起始节点窗口</param>
        /// <param name="end">结束节点窗口</param>
        /// <param name="isMainLine">是否绘制成主线高亮颜色</param>
        private static void DrawCurve(Rect start, Rect end, bool isMainLine)
        {
            Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height + 20, 0);
            Vector3 endPos = new Vector3(end.x + end.width / 2, end.y - 20, 0);
            Vector3 startTan = startPos + Vector3.up * 50;
            Vector3 endTan = endPos + Vector3.down * 50;
            //绘制阴影
            for (int i = 0; i < 3; i++)
            {
                // Handles.DrawBezier(startPos, endPos, startTan, endTan, m_Shadow, null, (i + 1) * 5);
            }
            //绘制线条
            if (isMainLine)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, StoryEditorWindowPreference.MainLine, null, 4);
            }
            else
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, StoryEditorWindowPreference.BranchLine, null, 3);
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
        private void Update()
        {
            Repaint();
        }
        private void OnGUI()
        {
            CheckMouseEvent();

            Vector2 v = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height - 20), ScrollPos, View);
            ScrollPos = new Vector2Int((int)v.x ,(int)v.y);
            DrawBG();
            DrawNodeCurve();
            DrawTempCurve();
            DrawStory();
            GUI.EndScrollView();

            DrawHead();
            DrawFoot();
            DrawNodeEditPanel();
        }
        private void OnFocus()
        {
            if (!Instance.wantsMouseMove)
            {
                Instance.wantsMouseMove = true;
            }
        }
        private void OnValidate()
        {
        }
        private void OnProjectChange()
        {
            Refresh();
        }
    }
}
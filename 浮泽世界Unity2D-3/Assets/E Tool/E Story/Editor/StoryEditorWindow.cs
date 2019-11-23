using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;
using System.Linq;
using System.Reflection;
using System.Collections;
using UnityEditorInternal;

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
        /// 当前故事
        /// </summary>
        private static Story currentStory;
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
        private static int selectStoryIndex = -1;
        /// <summary>
        /// 是否显示节点编辑面板
        /// </summary>
        private static bool showNodeEditPanel = false;

        private static ReorderableList nodeOptionList;

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
            get => Story.GetValues();
        }
        /// <summary>
        /// 当前节点
        /// </summary>
        public static StoryNode CurrentNode
        {
            get => currentNode;
            set
            {
                currentNode = value;

                nodeOptionList = new ReorderableList(CurrentNode.nodeOptions, typeof(StoryNodeOption), true, true, false, true)
                {
                    //设置单个元素的高度
                    elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3,

                    //绘制
                    drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                    {
                        StoryNodeOption data = (StoryNodeOption)nodeOptionList.list[index];
                        data.description = EditorGUI.TextField(rect, data.description);
                    },

                    //背景色
                    //reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {
                    //    GUI.backgroundColor = Color.yellow;
                    //};
                    showDefaultBackground = true,

                    //标题
                    drawHeaderCallback = (Rect rect) =>
                    {
                        GUI.Label(rect, "选项列表");
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
                            sp1.stringValue = "（请输入节点简介）";
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
        /// 节点编辑面板
        /// </summary>
        private static Rect nodeEditPanel;
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
            get => new Rect(0, 0, StoryEditorWindowPreference.ViewSize.x, StoryEditorWindowPreference.ViewSize.y);
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
                currentStory = story;
                Selection.activeObject = currentStory;
                Instance.ShowNotification(new GUIContent("已打开 " + currentStory.name));
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
            if (currentStory == null)
            {
                Instance.ShowNotification(new GUIContent("当前未打开故事"));
            }
            else
            {
                Instance.ShowNotification(new GUIContent("已关闭 " + currentStory.name));
                currentStory = null;
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
                currentStory = null;
            }

            Instance.ShowNotification(new GUIContent("已刷新"));
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
            if (currentStory != null)
            {
                currentStory.CreateNode(new RectInt(mousePos.x, mousePos.y, StoryEditorWindowPreference.NodeSize.x, StoryEditorWindowPreference.NodeSize.y));
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
            if (currentStory != null)
            {
                string str = "确认要删除故事 {" + currentStory.name + "} 以及其所有节点吗？本地文件也将一并删除，这将无法恢复。";
                if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
                {
                    if (currentStory.Nodes != null)
                    {
                        currentStory.Nodes.Clear();
                        Debug.Log("已删除所有 {" + currentStory.name + "} 的故事节点");
                    }
                    string path = GetCurrentStoryPath();
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();
                    Debug.Log("已删除故事 {" + path + "}");
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
            if (currentStory != null)
            {
                AssetDatabase.SaveAssets();
                Instance.ShowNotification(new GUIContent("已保存 " + currentStory.name));
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
            if (currentStory != null)
            {
                UnityEngine.Object ob = Selection.activeObject;
                Selection.activeObject = currentStory;
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
                    currentStory.ClearNodeUpChoices(CurrentNode);
                    currentStory.ClearNodeDownChoices(CurrentNode);
                    break;
                case 6:
                    break;
                case 7:
                    currentStory.RemoveNode(CurrentNode);
                    break;
                case 8:
                    break;
                case 9:
                    currentStory.SetNodeType(CurrentNode, NodeType.起始节点);
                    break;
                case 10:
                    currentStory.SetNodeType(CurrentNode, NodeType.中间节点);
                    break;
                case 11:
                    currentStory.SetNodeType(CurrentNode, NodeType.结局节点);
                    break;
                case 12:
                    currentStory.ClearNodes();
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
                    if (currentStory != null && currentStory.Nodes != null)
                    {
                        //获取当前选中节点
                        bool isInNode = false;
                        foreach (StoryNode item in currentStory.Nodes)
                        {
                            if (item.layout.Contains(mousePos))
                            {
                                xOffset = mousePos.x - item.layout.x;
                                yOffset = mousePos.y - item.layout.y;
                                isInNode = true;
                                //选中此节点
                                CurrentNode = item;
                                break;
                            }
                        }

                        if (isInNode)
                        {
                            showNodeEditPanel = true;
                            //将选中节点置顶显示
                            //currentStory.Nodes.Remove(currentNode);
                            //currentStory.Nodes.Insert(currentStory.Nodes.Count, currentNode);
                        }
                        else
                        {
                            if (nodeEditPanel.Contains(mousePos))
                            {
                            }
                            else
                            {
                                showNodeEditPanel = false;
                            }
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
                    if (currentStory)
                    {
                        if (currentStory.Nodes.Contains(CurrentNode))
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
                        currentStory.RemoveNode(CurrentNode);
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
            EditorGUI.DrawRect(panel, StoryEditorWindowPreference.NormalNode);

            //故事列表下拉框
            Rect r1 = new Rect(LineSpacing, LineSpacing, 160, LineHeight);
            SelectStoryIndex = EditorGUI.Popup(r1, SelectStoryIndex, StoryNames);

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
            EditorGUI.DrawRect(panel, StoryEditorWindowPreference.NormalNode);

            string mousePos = "X: " + StoryEditorWindow.mousePos.x + "  Y: " + StoryEditorWindow.mousePos.y;
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
        /// 绘制节点编辑面板
        /// </summary>
        private static void DrawNodeEditPanel()
        {
            if (showNodeEditPanel)
            {
                nodeEditPanel = new Rect(Instance.position.width - 320, 30, 300, 300);
                EditorGUI.DrawRect(nodeEditPanel, StoryEditorWindowPreference.NormalNode);

                if (CurrentNode != null)
                {
                    //编号
                    Rect r1 = new Rect(nodeEditPanel.x + LineSpacing, nodeEditPanel.y + LineSpacing, nodeEditPanel.width - LineSpacing * 2, LineHeight);
                    EditorGUI.LabelField(r1, "章节-场景-片段-分支");
                    int chapter = EditorGUI.IntField(new Rect(r1.x, r1.y + OneLine, 30, LineHeight), CurrentNode.id.chapter);
                    int scene = EditorGUI.IntField(new Rect(r1.x + (30 + LineSpacing) * 1, r1.y + OneLine, 30, LineHeight), CurrentNode.id.scene);
                    int part = EditorGUI.IntField(new Rect(r1.x + (30 + LineSpacing) * 2, r1.y + OneLine, 30, LineHeight), CurrentNode.id.part);
                    int branch = EditorGUI.IntField(new Rect(r1.x + (30 + LineSpacing) * 3, r1.y + OneLine, 30, LineHeight), CurrentNode.id.branch);
                    NodeID id = new NodeID(chapter, scene, part, branch);
                    if (!id.Equals(CurrentNode.id))
                    {
                        currentStory.SetNodeID(CurrentNode, id);
                    }

                    //主线
                    Rect r2 = new Rect(nodeEditPanel.x + nodeEditPanel.width - 50 - LineSpacing, nodeEditPanel.y + LineSpacing, 50, LineHeight);
                    bool ismain = EditorGUI.ToggleLeft(r2, "主线", CurrentNode.isMainNode);
                    if (ismain != CurrentNode.isMainNode)
                    {
                        CurrentNode.isMainNode = ismain;
                    }

                    //简介
                    Rect r3 = new Rect(r1.x, r1.y + OneLine * 2, 80, LineHeight);
                    EditorGUI.LabelField(r3, "节点简介");
                    Rect r4 = new Rect(r1.x, r1.y + OneLine * 3, r1.width, LineHeight * 2 + LineSpacing);
                    CurrentNode.description = EditorGUI.TextArea(r4, CurrentNode.description);

                    //

                    //for (int j = 0; j < node.NextNodes[i].Conditions.Count; j++)
                    //{
                    //    node.NextNodes[i].Conditions[j].SetIndex
                    //        ( EditorGUI.Popup(new Rect(nodeRect.x + 10, nodeRect.y + 20 * i + baseHeight + 20 * (j + 1), 80 + addWidth, 16),
                    //        node.NextNodes[i].Conditions[j].KeyIndex, CurrentStory.ConditionKeys) );
                    //    node.NextNodes[i].Conditions[j].SetComparison
                    //        ((Comparison)EditorGUI.EnumPopup(new Rect(nodeRect.x + 90, nodeRect.y + 20 * i + baseHeight + 20 * (j + 1), 80 + addWidth, 16),
                    //        GUIContent.none, node.NextNodes[i].Conditions[j].Comparison));
                    //    node.NextNodes[i].Conditions[j].SetValue
                    //        (EditorGUI.IntField(new Rect(nodeRect.x + 170, nodeRect.y + 20 * i + baseHeight + 20 * (j + 1), 30 + addWidth, 16),
                    //        GUIContent.none, node.NextNodes[i].Conditions[j].Value));
                    //}

                    Rect r5 = new Rect(r1.x, r1.y + OneLine * 5, r1.width, 200);
                    nodeOptionList.DoList(r5);
                }
                else
                {
                    EditorGUI.LabelField(new Rect(nodeEditPanel.x + 20, nodeEditPanel.y + 20, 120, 20), "请选择一个节点");
                }
            }
        }
        /// <summary>
        /// 绘制右键菜单
        /// </summary>
        private static void DrawContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            if (currentStory != null)
            {
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
                    if (currentStory.Nodes != null)
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
            if (currentStory != null)
            {
                if (currentStory.Nodes != null)
                {
                    foreach (StoryNode item in currentStory.Nodes)
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
                Rect nodeRect = new Rect(node.layout.x, node.layout.y, node.layout.width, node.layout.height);

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

                //编号
                Rect r1 = new Rect(nodeRect.x + LineSpacing, nodeRect.y + LineSpacing, nodeRect.width - LineSpacing * 2, LineHeight);
                string id = string.Format("{0}-{1}-{2}-{3}", node.id.chapter, node.id.scene, node.id.part, node.id.branch);
                EditorGUI.LabelField(r1, id);

                //主线
                Rect r2 = new Rect(nodeRect.x + nodeRect.width - 60 - LineSpacing, nodeRect.y + LineSpacing, 60, LineHeight);
                bool ismain = EditorGUI.ToggleLeft(r2, "主线", node.isMainNode);
                if (ismain != node.isMainNode)
                {
                    node.isMainNode = ismain;
                }

                //简介
                Rect r3 = new Rect(r1.x, r1.y + OneLine, r1.width, LineHeight * 2);
                node.description = EditorGUI.TextArea(r3, node.description);

                //分支
                node.layout.height = StoryEditorWindowPreference.NodeSize.y + DrawNextNodes(node);
                node.layout.width = StoryEditorWindowPreference.NodeSize.x;
            }
        }
        /// <summary>
        /// 绘制下个节点
        /// </summary>
        private static int DrawNextNodes(StoryNode node)
        {
            int height = 0;
            for (int i = 0; i < node.nodeOptions.Count; i++)
            {
                if (currentStory.ContainsID(node.nodeOptions[i].id))
                {
                    height += DrawNextNode(node, node.nodeOptions[i], out bool isRemove);
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
            return height;
        }
        /// <summary>
        /// 绘制下个节点
        /// </summary>
        private static int DrawNextNode(StoryNode node, StoryNodeOption nextNode, out bool isRemove)
        {
            int height = 20;
            int index = node.nodeOptions.IndexOf(nextNode);
            Rect nextNodeRect = new Rect(node.layout.x, node.layout.y + StoryEditorWindowPreference.NodeSize.y + OneLine * index, node.layout.width, OneLine);
            NodeID nextNodeID = nextNode.id;

            //删除按钮
            isRemove = GUI.Button(new Rect(nextNodeRect.x + nextNodeRect.width, nextNodeRect.y, OneLine, OneLine), "X");

            //描述
            Rect r1 = new Rect(nextNodeRect.x + LineSpacing, nextNodeRect.y, nextNodeRect.width - 60 - LineSpacing, LineHeight);
            nextNode.description = EditorGUI.TextField(r1, nextNode.description);

            //编号
            string label = string.Format("{0}-{1}-{2}-{3}", nextNodeID.chapter, nextNodeID.scene, nextNodeID.part, nextNodeID.branch);
            Rect r2 = new Rect(nextNodeRect.x + r1.width + LineSpacing * 2, nextNodeRect.y, 60, LineHeight);
            EditorGUI.LabelField(r2, label);

            //绘制连接曲线
            StoryNode nnode = currentStory.GetNode(nextNodeID);
            Vector2 start = new Vector2(nextNodeRect.x + nextNodeRect.width + 20, nextNodeRect.y + 10);
            Vector2 end = new Vector2(nnode.layout.x - 20, nnode.layout.y + 10);
            DrawCurve(start, end, node.isMainNode && nnode.isMainNode, Vector3.right, Vector3.left);

            return height;
        }
        /// <summary>
        /// 绘制节点上按钮
        /// </summary>
        /// <param name="node"></param>
        private static void DrawUpButton(StoryNode node)
        {
            Rect rect = new Rect(node.layout.x - OneLine, node.layout.y, OneLine, OneLine);
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
            Rect rect = new Rect(node.layout.x + node.layout.width, node.layout.y + StoryEditorWindowPreference.NodeSize.y - OneLine, OneLine, OneLine);
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
                Vector2 start = new Vector2(upNode.layout.x + upNode.layout.width + OneLine, upNode.layout.y + StoryEditorWindowPreference.NodeSize.y - OneLine/2);
                Vector2 end = mousePos;
                DrawCurve(start, end , true, Vector3.right, Vector3.left);
            }
            if (downNode != null)
            {
                Vector2 start = mousePos;
                Vector2 end = new Vector2(downNode.layout.x - OneLine, downNode.layout.y + OneLine/2);
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
        private void OnEnable()
        {
            if (Storys.Count > 0)
            {
                SelectStoryIndex = 0;
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
            scrollPos = new Vector2Int((int)v.x ,(int)v.y);
            DrawBG();
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
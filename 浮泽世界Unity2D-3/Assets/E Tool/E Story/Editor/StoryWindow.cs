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
    public class StoryWindow : EditorWindow
    {
        //运行数据
        /// <summary>
        /// 故事编辑器窗口
        /// </summary>
        private static StoryWindow instance;
        /// <summary>
        /// 当前节点
        /// </summary>
        private static Node currentNode;
        /// <summary>
        /// 上节点
        /// </summary>
        private static Node upNode;
        /// <summary>
        /// 下节点
        /// </summary>
        private static Node downNode;

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
        /// 当前故事
        /// </summary>
        public static Story CurrentStory;
        /// <summary>
        /// 当前故事的路径
        /// </summary>
        /// <returns></returns>
        public static string CurrentStoryPath
        {
            get
            {
                if (CurrentStory != null)
                {
                    return AssetDatabase.GetAssetPath(CurrentStory);
                }
                else return null;
            }
        }
        /// <summary>
        /// 当前节点
        /// </summary>
        public static Node CurrentNode
        {
            get => currentNode;
            set
            {
                if (currentNode == value) return;
                currentNode = value;

            }
        }
        
        //窗口样式
        /// <summary>
        /// 光标位置
        /// </summary>
        private static Vector2Int mousePos;
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
        /// 顶部面板
        /// </summary>
        private static Rect Top
        {
            get => new Rect(0, 0, Instance.position.width, Utility.GetHeightLong(1));
        }
        /// <summary>
        /// 中部面板
        /// </summary>
        private static Rect Center
        {
            get => new Rect(0, Utility.GetHeightLong(1), Instance.position.width, Instance.position.height - Utility.GetHeightLong(1));
        }
        /// <summary>
        /// 底部面板
        /// </summary>
        private static Rect Buttom
        {
            get => new Rect(0, Instance.position.height - Utility.GetHeightLong(1), Instance.position.width, Utility.GetHeightLong(1));
        }

        //mono
        private void Update()
        {
            Repaint();
        }
        private void OnGUI()
        {
            CheckMouseEvent();

            Vector2 v = GUI.BeginScrollView(Center, scrollPos, View);
            scrollPos = new Vector2Int((int)v.x, (int)v.y);
            DrawBG();
            DrawTempCurve();
            DrawStory();
            GUI.EndScrollView();

            DrawHead();
            DrawFoot();
        }
        private void OnFocus()
        {
            Instance.wantsMouseMove = true;
        }

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
        public static void OpenStory(Story story)
        {
            if (story != null)
            {
                story.CheckNull();
                CurrentStory = story;
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
                CurrentStory = null;
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
        }
        /// <summary>
        /// 刷新鼠标坐标
        /// </summary>
        private static void RefreshMousePosition()
        {
            mousePos = new Vector2Int((int)((Event.current.mousePosition.x) + scrollPos.x), (int)((Event.current.mousePosition.y) + scrollPos.y - Utility.GetHeightLong(1)));
        }

        //创建
        /// <summary>
        /// 创建故事
        /// </summary>
        private static void CreateStory()
        {
            string path = EditorUtility.SaveFilePanelInProject("创建故事", "新故事", "Asset", "保存故事", Application.dataPath);
            if (path == "") return;
            Story story = CreateInstance<Story>();
            AssetDatabase.CreateAsset(story, path);
            CurrentStory = story;
            OpenStory(story);
            //AssetDatabase.Refresh();
        }
        /// <summary>
        /// 创建故事节点
        /// </summary>
        private static void CreatePlotNode()
        {
            if (CurrentStory != null)
            {
                CurrentStory.CreatePlotNode(new RectInt(mousePos.x, mousePos.y, StoryWindowPreference.NodeSize.x, StoryWindowPreference.NodeSize.y));
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
        /// 创建剧情节点
        /// </summary>
        private static void CreateOptionNode()
        {
            if (CurrentStory != null)
            {
                CurrentStory.CreateOptionNode(new RectInt(mousePos.x, mousePos.y, StoryWindowPreference.NodeSize.x, StoryWindowPreference.NodeSize.y));
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
        private static void CreatePlot()
        {
            if (CurrentNode != null)
            {
                string path = EditorUtility.SaveFilePanelInProject("创建剧情片段", "新剧情片段", "Asset", "保存剧情片段", Application.dataPath);
                if (path == "") return;
                Plot plot = CreateInstance<Plot>();
                AssetDatabase.CreateAsset(plot, path);
                (CurrentNode as PlotNode).plot = plot;
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
                    string path = CurrentStoryPath;
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();
                    Debug.Log("已删除故事 {" + path + "}");
                    CurrentStory = null;
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
                EditorUtility.SetDirty(CurrentStory);
                AssetDatabase.SaveAssets();
                Instance.ShowNotification(new GUIContent("已保存 " + CurrentStory.name));
            }
            else
            {
                Instance.ShowNotification(new GUIContent("当前未打开故事"));
            }
        }

        ///获取

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
                    CreatePlot();
                    break;
                case 2:
                    CreatePlotNode();
                    break;
                case 3:
                    CreateOptionNode();
                    break;
                case 4:
                    break;
                case 5:
                    CurrentStory.ClearNodeUpChoices(CurrentNode);
                    break;
                case 6:
                    CurrentStory.ClearNodeDownChoices(CurrentNode);
                    break;
                case 7:
                    CurrentStory.ClearNodeUpChoices(CurrentNode);
                    CurrentStory.ClearNodeDownChoices(CurrentNode);
                    break;
                case 8:
                    CurrentStory.RemoveNode(CurrentNode);
                    break;
                case 9:
                    CurrentStory.SetNodeType(CurrentNode as PlotNode, PlotType.起始剧情);
                    break;
                case 10:
                    CurrentStory.SetNodeType(CurrentNode as PlotNode, PlotType.过渡剧情);
                    break;
                case 11:
                    CurrentStory.SetNodeType(CurrentNode as PlotNode, PlotType.结局剧情);
                    break;
                case 12:
                    CurrentStory.ClearPlotNodes();
                    break;
                case 13:
                    CurrentStory.ClearOptionNodes();
                    break;
                case 14:
                    CurrentStory.ClearAllNodes();
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
                    if (CurrentStory != null && CurrentStory.plotNodes != null)
                    {
                        //获取当前选中节点
                        bool isInNode = false;
                        foreach (PlotNode item in CurrentStory.plotNodes)
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
                        foreach (OptionNode item in CurrentStory.optionNodes)
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
                        }
                        else
                        {
                            CurrentNode = null;
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
                        if (CurrentStory.plotNodes.Contains(CurrentNode))
                        {
                            if (CurrentNode.layout.Contains(mousePos))
                            {
                                CurrentNode.layout = new RectInt(mousePos.x - xOffset, mousePos.y - yOffset, CurrentNode.layout.width, CurrentNode.layout.height);
                            }
                        }
                        if (CurrentStory.optionNodes.Contains(CurrentNode))
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
                        //Event.current.Use();
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
            CurrentStory.CheckNull();
            if (upNode != null && downNode != null)
            {
                if (upNode.GetType() == typeof(PlotNode))
                {
                    PlotNode pnu = upNode as PlotNode;
                    if (downNode.GetType() == typeof(PlotNode))
                    {
                        PlotNode pnd = downNode as PlotNode;

                        if (pnu == pnd)
                        {
                            Instance.ShowNotification(new GUIContent("不可连接自己"));
                        }
                        else
                        {
                            pnu.CheckNull();
                            pnu.nextPlotNode = pnd.id;
                        }
                    }
                    else
                    {
                        OptionNode ond = downNode as OptionNode;

                        bool isHave = false;
                        foreach (int item in pnu.nextOptionNodes)
                        {
                            if (item == ond.id)
                            {
                                isHave = true;
                                break;
                            }
                        }

                        if (isHave)
                        {
                            Instance.ShowNotification(new GUIContent("已存在连接"));
                        }
                        else
                        {
                            pnu.nextOptionNodes.Add(ond.id);
                        }
                    }
                }
                else
                {
                    OptionNode onu = upNode as OptionNode;
                    if (downNode.GetType() == typeof(PlotNode))
                    {
                        PlotNode pnd = downNode as PlotNode;

                        onu.nextPlotNode = pnd.id;
                    }
                    else
                    {
                        OptionNode ond = downNode as OptionNode;

                        if (onu == ond)
                        {
                            Instance.ShowNotification(new GUIContent("不可连接自己"));
                        }
                        else
                        {
                            Instance.ShowNotification(new GUIContent("选项节点不能链接选项节点"));
                        }
                    }
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
            int btnWidth = 40;
            Rect r1 = new Rect(Top.width - btnWidth - Utility.OneSpacing, Utility.OneSpacing, btnWidth, Utility.OneHeight);
            Rect r2 = new Rect(r1.x - btnWidth - Utility.OneSpacing, Utility.OneSpacing, btnWidth, Utility.OneHeight);
            Rect r3 = new Rect(r2.x - btnWidth - Utility.OneSpacing, Utility.OneSpacing, btnWidth, Utility.OneHeight);
            Rect r4 = new Rect(r3.x - btnWidth - Utility.OneSpacing, Utility.OneSpacing, btnWidth, Utility.OneHeight);
            Rect r5 = new Rect(r4.x - btnWidth - Utility.OneSpacing, Utility.OneSpacing, btnWidth, Utility.OneHeight);
            Rect r6 = new Rect(r5.x - btnWidth - Utility.OneSpacing, Utility.OneSpacing, btnWidth, Utility.OneHeight);
            Rect r7 = new Rect(Utility.OneSpacing * 2 + 60, Utility.OneSpacing, r6.x - Utility.OneSpacing * 3 - 60, Utility.OneHeight);
            Rect r8 = new Rect(Utility.OneSpacing, Utility.OneSpacing, 60, Utility.OneHeight);

            //顶部背景
            EditorGUI.DrawRect(Top, StoryWindowPreference.NormalNode);

            //故事
            EditorGUI.LabelField(r8, "当前故事");
            CurrentStory = (Story)EditorGUI.ObjectField( r7, CurrentStory, typeof(Story));

            //按钮
            if (GUI.Button(r6, "创建"))
            {
                CreateStory();
            }
            if (GUI.Button(r5, "删除"))
            {
                DeleteCurrentStory();
            }
            if (GUI.Button(r4, "关闭"))
            {
                CloseCurrentStory();
            }
            if (GUI.Button(r3, "保存"))
            {
                SaveCurrentStory();
            }
            if (GUI.Button(r2, "刷新"))
            {
                Refresh();
            }
            if (GUI.Button(r1, "设置"))
            {
                Instance.ShowNotification(new GUIContent("请前往 Edit -> Preferences -> E Story 进行编辑"));
            }
        }
        /// <summary>
        /// 绘制窗口足
        /// </summary>
        private static void DrawFoot()
        {
            //EditorGUI.DrawRect(Buttom, StoryWindowPreference.NormalNode);
            string mousePos = "X: " + StoryWindow.mousePos.x + "  Y: " + StoryWindow.mousePos.y;
            Rect r1 = new Rect(Utility.OneSpacing, Center.y + Center.height - Utility.GetHeightMiddle(2) + 10, 120, Utility.OneHeight);
            EditorGUI.LabelField(r1, mousePos);
        }
        /// <summary>
        /// 绘制右键菜单
        /// </summary>
        private static void DrawContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            if (CurrentStory != null)
            {
                menu.AddItem(new GUIContent("创建剧情节点"), false, DoMethod, 2);
                menu.AddItem(new GUIContent("创建选项节点"), false, DoMethod, 3);

                if (CurrentNode != null)
                {
                    if (CurrentNode.GetType() == typeof(PlotNode))
                    {
                        menu.AddItem(new GUIContent("创建剧情片段"), false, DoMethod, 1);

                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent("设为起始节点"), false, DoMethod, 9);
                        menu.AddItem(new GUIContent("设为中间节点"), false, DoMethod, 10);
                        menu.AddItem(new GUIContent("设为结局节点"), false, DoMethod, 11);
                    }

                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("清除上行连接"), false, DoMethod, 5);
                    menu.AddItem(new GUIContent("清除下行连接"), false, DoMethod, 6);
                    menu.AddItem(new GUIContent("清除所有连接"), false, DoMethod, 7);

                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("删除选中节点"), false, DoMethod, 8);
                }
                else
                {
                    menu.AddSeparator("");
                }
                menu.AddItem(new GUIContent("删除所有剧情节点"), false, DoMethod, 12);
                menu.AddItem(new GUIContent("删除所有选项节点"), false, DoMethod, 13);
                menu.AddItem(new GUIContent("删除所有节点"), false, DoMethod, 14);
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
                if (CurrentStory.plotNodes != null)
                {
                    foreach (PlotNode item in CurrentStory.plotNodes)
                    {
                        DrawPlotNode(item);
                    }
                }
                if (CurrentStory.optionNodes != null)
                {
                    foreach (OptionNode item in CurrentStory.optionNodes)
                    {
                        DrawOptionNode(item);
                    }
                }
            }
        }
        /// <summary>
        /// 绘制剧情节点
        /// </summary>
        private static void DrawPlotNode(PlotNode node)
        {
            if (node != null)
            {
                int labelWidth = 60;
                int inputWidth = 30;
                node.layout = new RectInt(node.layout.x, node.layout.y, StoryWindowPreference.NodeSize.x, StoryWindowPreference.NodeSize.y);
                Rect r0 = new Rect(node.layout.x, node.layout.y, node.layout.width, node.layout.height);

                Rect r1 = new Rect(r0.x + Utility.OneSpacing, r0.y + Utility.OneSpacing, r0.width - Utility.OneSpacing * 2, Utility.OneHeight);
                Rect r1_0 = new Rect(r1.x, r1.y, labelWidth, Utility.OneHeight);
                Rect r1_1 = new Rect(r1_0.x + labelWidth + Utility.OneSpacing, r1.y, inputWidth, Utility.OneHeight);
                Rect r1_2 = new Rect(r1_1.x + inputWidth + Utility.OneSpacing, r1.y, inputWidth, Utility.OneHeight);
                Rect r1_3 = new Rect(r1_2.x + inputWidth + Utility.OneSpacing, r1.y, inputWidth, Utility.OneHeight);
                Rect r1_4 = new Rect(r1_3.x + inputWidth + Utility.OneSpacing, r1.y, inputWidth, Utility.OneHeight);
                Rect r1_5 = new Rect(r1_4.x + inputWidth + Utility.OneSpacing, r1.y, labelWidth, Utility.OneHeight);

                Rect r2 = new Rect(r1.x, r1.y + Utility.GetHeightMiddle(1), r1.width, StoryWindowPreference.NodeSize.y - Utility.GetHeightLong(1) * 2);
                Rect r2_0 = new Rect(r2.x, r2.y, labelWidth, Utility.OneHeight);
                Rect r2_1 = new Rect(r2.x + labelWidth + Utility.OneSpacing, r2.y, r2.width - labelWidth - Utility.OneSpacing, r2.height);

                Rect r3 = new Rect(r2.x, r0.y + r0.height - Utility.GetHeightMiddle(1), r2.width, Utility.OneHeight);
                Rect r3_0 = new Rect(r3.x, r3.y, labelWidth, Utility.OneHeight);
                Rect r3_1 = new Rect(r3.x + labelWidth + Utility.OneSpacing, r3.y, r3.width - labelWidth - Utility.OneSpacing, Utility.OneHeight);

                //节点背景
                if (CurrentNode != null)
                {
                    EditorGUI.DrawRect(r0, CurrentNode == node ? StoryWindowPreference.SelectNode : StoryWindowPreference.NormalNode);
                }
                else
                {
                    EditorGUI.DrawRect(r0, StoryWindowPreference.NormalNode);
                }

                //节点类型链接按钮
                switch (node.type)
                {
                    case PlotType.过渡剧情:
                        DrawUpButton(node);
                        DrawDownButton(node);
                        break;
                    case PlotType.起始剧情:
                        DrawDownButton(node);
                        break;
                    case PlotType.结局剧情:
                        DrawUpButton(node);
                        break;
                    default:
                        break;
                }

                //编号
                EditorGUI.LabelField(r1_0, "剧情编号");
                int chapter = EditorGUI.IntField(r1_1, node.id.chapter);
                int scene = EditorGUI.IntField(r1_2, node.id.scene);
                int part = EditorGUI.IntField(r1_3, node.id.part);
                int branch = EditorGUI.IntField(r1_4, node.id.branch);
                PlotID id = new PlotID(chapter, scene, part, branch);
                if (!id.Equals(node.id))
                {
                    CurrentStory.SetPlotID(node, id);
                }
                //主线
                bool ismain = EditorGUI.ToggleLeft(r1_5, "主线", node.isMainPlot);
                if (ismain != node.isMainPlot)
                {
                    node.isMainPlot = ismain;
                }

                //简介
                EditorGUI.LabelField(r2_0, "剧情简介");
                node.description = EditorGUI.TextArea(r2_1, node.description);

                //内容
                EditorGUI.LabelField(r3_0, "剧情片段");
                node.plot = (Plot)EditorGUI.ObjectField(r3_1, node.plot, typeof(Plot));

                //连接线
                if (node.nextOptionNodes != null)
                {
                    foreach (int item in node.nextOptionNodes)
                    {
                        OptionNode on = CurrentStory.GetNode(item);
                        if (on == null) continue;
                        Vector2 start = new Vector2(r0.x + r0.width + Utility.OneHeight, r0.y + r0.height / 2);
                        Vector2 end = new Vector2(on.layout.x - Utility.OneHeight, on.layout.y + on.layout.height / 2);

                        PlotNode pn = CurrentStory.GetNode(on.nextPlotNode);
                        bool isMainPlot = false;
                        if (pn != null)
                        {
                            isMainPlot = node.isMainPlot && pn.isMainPlot;
                        }
                        DrawCurve(start, end, isMainPlot, Vector3.right, Vector3.left);
                    }
                }
                if (!node.nextPlotNode.Equals(new PlotID()))
                {
                    PlotNode pn = CurrentStory.GetNode(node.nextPlotNode);
                    if (pn == null) return;
                    Vector2 start = new Vector2(r0.x + r0.width + Utility.OneHeight, r0.y + r0.height / 2);
                    Vector2 end = new Vector2(pn.layout.x - Utility.OneHeight, pn.layout.y + pn.layout.height / 2);

                    bool isMainPlot = node.isMainPlot && pn.isMainPlot;
                    DrawCurve(start, end, isMainPlot, Vector3.right, Vector3.left);
                }
            }
        }
        /// <summary>
        /// 绘制选项节点
        /// </summary>
        private static void DrawOptionNode(OptionNode node)
        {
            int labelWidth = 60;
            int cp = node.comparisons.Count;
            int ch = node.changes.Count;
            int lines = cp + ch;
            node.layout = new RectInt(node.layout.x, node.layout.y, StoryWindowPreference.NodeSize.x, (int)(Utility.GetHeightMiddle(1) * (lines + 3) + Utility.OneSpacing));
            Rect r0 = new Rect(node.layout.x, node.layout.y, node.layout.width, node.layout.height);

            Rect r1 = new Rect(r0.x + Utility.OneSpacing, r0.y + Utility.OneSpacing, r0.width - Utility.OneSpacing * 2, Utility.OneHeight);
            Rect r1_0 = new Rect(r1.x, r1.y, labelWidth, Utility.OneHeight);
            Rect r1_1 = new Rect(r1.x + labelWidth + Utility.OneSpacing, r1.y, r1.width - labelWidth - Utility.OneSpacing, Utility.OneHeight);

            Rect r2 = new Rect(r1.x, r1.y + Utility.GetHeightMiddle(1), r1.width, Utility.GetHeightMiddle(1) * (cp + 1) - Utility.OneSpacing);
            Rect r2_0 = new Rect(r2.x, r2.y, labelWidth, Utility.OneHeight);
            Rect r2_1 = new Rect(r2.x + labelWidth + Utility.OneSpacing, r2.y, r2.width - labelWidth - Utility.OneSpacing, Utility.OneHeight);
            Rect r2_2 = new Rect(r2_1.x, r2_1.y + Utility.GetHeightMiddle(1), r2_1.width, r2.height - Utility.GetHeightMiddle(1));

            Rect r3 = new Rect(r2.x, r2.y + r2.height + Utility.OneSpacing, r2.width, Utility.GetHeightMiddle(1) * (ch + 1) - Utility.OneSpacing);
            Rect r3_0 = new Rect(r3.x, r3.y, labelWidth, Utility.OneHeight);
            Rect r3_1 = new Rect(r3.x + labelWidth + Utility.OneSpacing, r3.y, r3.width - labelWidth - Utility.OneSpacing, Utility.OneHeight);
            Rect r3_2 = new Rect(r3_1.x, r3_1.y + Utility.GetHeightMiddle(1), r3_1.width, r3.height - Utility.GetHeightMiddle(1));

            //节点背景
            if (CurrentNode != null)
            {
                EditorGUI.DrawRect(r0, CurrentNode == node ? StoryWindowPreference.SelectNode : StoryWindowPreference.NormalNode);
            }
            else
            {
                EditorGUI.DrawRect(r0, StoryWindowPreference.NormalNode);
            }

            //链接按钮
            DrawUpButton(node);
            DrawDownButton(node);

            //选项描述
            EditorGUI.LabelField(r1_0, "选项描述");
            node.description = EditorGUI.TextField(r1_1, node.description);

            //通过条件
            //EditorGUI.DrawRect(r2, StoryWindowPreference.SelectNode);
            EditorGUI.LabelField(r2_0, "通过条件");
            if (GUI.Button(r2_1, "+"))
            {
                node.comparisons.Add(new ConditionComparison());
            }
            for (int j = 0; j < node.comparisons.Count; j++)
            {
                int width = 80;
                Rect re0 = new Rect(r2_2.x, r2_2.y + Utility.GetHeightMiddle(1) * j, r2_2.width, Utility.OneHeight);
                Rect re1 = new Rect(re0.x, re0.y, width, Utility.OneHeight);
                Rect re2 = new Rect(re1.x + width + Utility.OneSpacing, re1.y, width, Utility.OneHeight);
                Rect re3 = new Rect(re2.x + width + Utility.OneSpacing, re2.y, re0.width - 160 - Utility.OneSpacing * 3 - Utility.OneHeight, Utility.OneHeight);
                Rect re4 = new Rect(re3.x + re3.width + Utility.OneSpacing, re3.y, Utility.OneHeight, Utility.OneHeight);

                node.comparisons[j].keyIndex = EditorGUI.Popup(re1, node.comparisons[j].keyIndex, CurrentStory.ConditionKeys);
                node.comparisons[j].comparison = (Comparison)EditorGUI.EnumPopup(re2, GUIContent.none, node.comparisons[j].comparison);
                node.comparisons[j].value = EditorGUI.IntField(re3, GUIContent.none, node.comparisons[j].value);

                if (GUI.Button(re4, "X"))
                {
                    node.comparisons.RemoveAt(j);
                    j--;
                }
            }

            //数值变动
            //EditorGUI.DrawRect(r3, StoryWindowPreference.SelectNode);
            EditorGUI.LabelField(r3_0, "数值变动");
            if (GUI.Button(r3_1, "+"))
            {
                node.changes.Add(new ConditionChange());
            }
            for (int j = 0; j < node.changes.Count; j++)
            {
                int width = 80;
                Rect re0 = new Rect(r3_2.x, r3_2.y + Utility.GetHeightMiddle(1) * j, r3_2.width, Utility.OneHeight);
                Rect re1 = new Rect(re0.x, re0.y, width, Utility.OneHeight);
                Rect re2 = new Rect(re1.x + width + Utility.OneSpacing, re1.y, width, Utility.OneHeight);
                Rect re3 = new Rect(re2.x + width + Utility.OneSpacing, re2.y, re0.width - 160 - Utility.OneSpacing * 3 - Utility.OneHeight, Utility.OneHeight);
                Rect re4 = new Rect(re3.x + re3.width + Utility.OneSpacing, re3.y, Utility.OneHeight, Utility.OneHeight);

                node.changes[j].keyIndex = EditorGUI.Popup(re1, node.changes[j].keyIndex, CurrentStory.ConditionKeys);
                node.changes[j].change = (Change)EditorGUI.EnumPopup(re2, GUIContent.none, node.changes[j].change);
                node.changes[j].value = EditorGUI.IntField(re3, GUIContent.none, node.changes[j].value);

                if (GUI.Button(re4, "X"))
                {
                    node.changes.RemoveAt(j);
                    j--;
                }
            }

            //连接线
            if (!node.nextPlotNode.Equals(new PlotID()))
            {
                PlotNode pn = CurrentStory.GetNode(node.nextPlotNode);
                if (pn == null) return;
                Vector2 start = new Vector2(r0.x + r0.width + Utility.OneHeight, r0.y + r0.height / 2);
                Vector2 end = new Vector2(pn.layout.x - Utility.OneHeight, pn.layout.y + pn.layout.height / 2);

                bool isLastMainPlot = false;
                foreach (PlotNode item in CurrentStory.plotNodes)
                {
                    if (item.nextOptionNodes.Contains(node.id))
                    {
                        if (item.isMainPlot)
                        {
                            isLastMainPlot = true;
                            break;
                        }
                    }
                }
                bool isMainPlot = pn.isMainPlot && isLastMainPlot;
                DrawCurve(start, end, isMainPlot, Vector3.right, Vector3.left);
            }
        }
        /// <summary>
        /// 绘制节点上按钮
        /// </summary>
        /// <param name="node"></param>
        private static void DrawUpButton(Node node)
        {
            Rect rect = new Rect(node.layout.x - Utility.OneHeight, node.layout.y, Utility.OneHeight, node.layout.height);
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
        private static void DrawDownButton(Node node)
        {
            Rect rect = new Rect(node.layout.x + node.layout.width, node.layout.y, Utility.OneHeight, node.layout.height);
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
                Vector2 start = new Vector2(upNode.layout.x + upNode.layout.width + Utility.OneHeight, upNode.layout.y + upNode.layout.height / 2);
                Vector2 end = mousePos;
                DrawCurve(start, end, true, Vector3.right, Vector3.left);
            }
            if (downNode != null)
            {
                Vector2 start = mousePos;
                Vector2 end = new Vector2(downNode.layout.x - Utility.OneHeight, downNode.layout.y + downNode.layout.height / 2);
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
    }
}
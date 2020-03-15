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
    public class StoryWindow : MyEditorWindow<StoryWindow>
    {
        private Rect Top { get => new Rect(0, 0, position.width, Utility.GetHeightLong(1)); }
        private Rect Center { get => new Rect(0, Top.height, position.width, position.height - Top.height - Buttom.height); }
        private Rect Buttom { get => new Rect(0, position.height, position.width, 0); }
        private Rect ScrollView { get => new Rect(0, 0, StoryWindowPreference.ViewSize.x, StoryWindowPreference.ViewSize.y); }
       
        private Vector2 TopLineLeft { get => new Vector2(Top.x, Top.height); }
        private Vector2 TopLineRight { get => new Vector2(Top.width, Top.height); }

        private int GridInterval { get => 100; }
        private float BendingFactor { get => IsZoomOut ? 50 * Zoom : 50; }
        private float Zoom { get => 0.5f; }

        private Vector2Int MousePos { get; set; }
        private Vector2 ScrollPos { get; set; }
        public bool IsZoomOut
        {
            get
            {
                if (Story)
                {
                    return Story.isZoomOut;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (Story)
                {
                    Story.isZoomOut = value;
                    foreach (PlotNode item in Story.plotNodes)
                    {
                        //缩小
                        if (value)
                        {
                            item.layout.x *= Zoom;
                            item.layout.y *= Zoom;
                            item.layout.width = StoryWindowPreference.NodeWidth * Zoom;
                        }
                        //默认
                        else
                        {
                            item.layout.x /= Zoom;
                            item.layout.y /= Zoom;
                            item.layout.width = StoryWindowPreference.NodeWidth;
                        }
                    }
                    foreach (OptionNode item in Story.optionNodes)
                    {
                        //缩小
                        if (value)
                        {
                            item.layout.x *= Zoom;
                            item.layout.y *= Zoom;
                            item.layout.width = StoryWindowPreference.NodeWidth * Zoom;
                        }
                        //默认
                        else
                        {
                            item.layout.x /= Zoom;
                            item.layout.y /= Zoom;
                            item.layout.width = StoryWindowPreference.NodeWidth;
                        }
                    }
                }
            }
        }
        public Story Story { get; set; }
        public Node Node { get; set; }
        public Node UpNode { get; set; }
        public Node DownNode { get; set; }

        private void Update()
        {
            Repaint();
        }
        private void OnGUI()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    CheckNode();
                    break;
                case EventType.MouseUp:
                    break;
                case EventType.MouseMove:
                    CheckMouseMove();
                    break;
                case EventType.MouseDrag:
                    break;
                case EventType.KeyDown:
                    break;
                case EventType.KeyUp:
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
                    DrawMenu();
                    break;
                case EventType.MouseEnterWindow:
                    break;
                case EventType.MouseLeaveWindow:
                    break;
                default:
                    break;
            }

            ScrollPos = GUI.BeginScrollView(Center, ScrollPos, ScrollView);
            DrawBG();
            DrawTempCurve();
            BeginWindows();
            DrawCenter();
            EndWindows();
            GUI.EndScrollView();

            DrawTop();
            DrawButtom();
            DrawMenu();
        }
        private void OnFocus()
        {
            wantsMouseMove = true;
        }

        //打开
        /// <summary>
        /// 打开窗口
        /// </summary>
        [MenuItem("Window/E Tool/打开故事编辑器 E Story %#q")]
        public static void Open()
        {
            Instance.Refresh();
        }
        /// <summary>
        /// 打开上次的故事
        /// </summary>
        [MenuItem("Window/E Tool/打开最近编辑的故事 %#w")]
        public static void OpenLastStory()
        {
            Open();
            Story data = StoryWindowPreference.LastOpendStory;
            if (data)
            {
                Instance.OpenStory(data);
            }
            else
            {
                Debug.LogError("没有记录最近编辑的故事");
            }
        }
        /// <summary>
        /// 打开故事
        /// </summary>
        /// <param name="data"></param>
        public void OpenStory(Story data)
        {
            if (data != null)
            {
                if (data != Story)
                {
                    data.CheckNull();
                    Story = data;
                    Selection.activeObject = Story;
                    StoryWindowPreference.LastOpendStory = Story;
                    ShowNotification(new GUIContent("已打开 " + Story.name));
                    Debug.Log("打开故事 " + AssetDatabase.GetAssetPath(Story));
                }
            }
        }

        //关闭
        /// <summary>
        /// 关闭故事
        /// </summary>
        private void CloseStory()
        {
            if (Story == null)
            {
                ShowNotification(new GUIContent("当前未打开故事"));
            }
            else
            {
                ShowNotification(new GUIContent("已关闭 " + Story.name));
                Story = null;
            }
        }

        //创建
        /// <summary>
        /// 创建故事
        /// </summary>
        private void CreateStory()
        {
            string path = EditorUtility.SaveFilePanelInProject("创建故事", "新故事", "Asset", "保存故事", Application.dataPath);
            if (path == "") return;
            Story data = CreateInstance<Story>();
            AssetDatabase.CreateAsset(data, path);
            Story = data;
            OpenStory(data);
            //AssetDatabase.Refresh();
        }
        /// <summary>
        /// 创建故事节点
        /// </summary>
        private void CreatePlotNode()
        {
            if (Story != null)
            {
                Story.CreatePlotNode(new Rect(MousePos.x, MousePos.y, StoryWindowPreference.NodeWidth, 80));
            }
            else
            {
                ShowNotification(new GUIContent("未指定故事"));
                if (EditorUtility.DisplayDialog("E Writer", "你需要先打开一个故事才能创建节点", "好的", "关闭"))
                {
                }
            }
        }
        /// <summary>
        /// 创建剧情节点
        /// </summary>
        private void CreateOptionNode()
        {
            if (Story != null)
            {
                Story.CreateOptionNode(new Rect(MousePos.x, MousePos.y, StoryWindowPreference.NodeWidth, 80));
            }
            else
            {
                ShowNotification(new GUIContent("未指定故事"));
                if (EditorUtility.DisplayDialog("E Writer", "你需要先打开一个故事才能创建节点", "好的", "关闭"))
                {
                }
            }
        }
        /// <summary>
        /// 创建内容
        /// </summary>
        private void CreatePlot()
        {
            if (Node != null)
            {
                string path = EditorUtility.SaveFilePanelInProject("创建剧情片段", "新剧情片段", "Asset", "保存剧情片段", Application.dataPath);
                if (path == "") return;
                Plot plot = CreateInstance<Plot>();
                AssetDatabase.CreateAsset(plot, path);
                (Node as PlotNode).plot = plot;
                //AssetDatabase.Refresh();
            }
        }

        //保存
        /// <summary>
        /// 删除故事
        /// </summary>
        private void SaveStory()
        {
            if (Story != null)
            {
                EditorUtility.SetDirty(Story);
                AssetDatabase.SaveAssets();
                ShowNotification(new GUIContent("已保存 " + Story.name));
            }
            else
            {
                ShowNotification(new GUIContent("当前未打开故事"));
            }
        }

        //删除
        /// <summary>
        /// 删除故事
        /// </summary>
        private void DeleteStory()
        {
            if (Story != null)
            {
                string str = "确认要删除故事文件 {" + Story.name + "} 吗？这将无法恢复。";
                if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
                {
                    string path = AssetDatabase.GetAssetPath(Story);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();
                    Debug.Log("已删除故事 {" + path + "}");
                    Story = null;
                }
            }
            else
            {
                ShowNotification(new GUIContent("当前未打开故事"));
            }
        }

        //清除
        /// <summary>
        /// 清除节点临时连接
        /// </summary>
        private void ClearTempConnect()
        {
            UpNode = null;
            DownNode = null;
        }

        //刷新
        /// <summary>
        /// 刷新窗口
        /// </summary>
        protected override void Refresh()
        {
            titleContent = new GUIContent("E Story", EditorGUIUtility.IconContent("EditCollider").image);
            minSize = new Vector2(450, 400);

            Menu = new GenericMenu();
            Menu.AddItem(new GUIContent("创建剧情节点"), false, CheckMenuClick, 2);
            Menu.AddItem(new GUIContent("创建选项节点"), false, CheckMenuClick, 3);
            Menu.AddSeparator("");
            Menu.AddItem(new GUIContent("清除所有连接"), false, CheckMenuClick, 7);
            Menu.AddSeparator("");
            Menu.AddItem(new GUIContent("删除所有剧情节点"), false, CheckMenuClick, 12);
            Menu.AddItem(new GUIContent("删除所有选项节点"), false, CheckMenuClick, 13);
            Menu.AddItem(new GUIContent("删除所有节点"), false, CheckMenuClick, 14);

            ClearTempConnect();
        }

        ///获取
        
        ///设置
        
        ///重置

        //检查
        /// <summary>
        /// 检查菜单点击
        /// </summary>
        /// <param name="obj"></param>
        protected override void CheckMenuClick(object obj)
        {
            switch (obj)
            {
                case 1:
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
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    Story.RemoveNode(Node);
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    Story.RemovePlotNodes();
                    break;
                case 13:
                    Story.RemoveOptionNodes();
                    break;
                case 14:
                    Story.RemoveAllNodes();
                    break;

                default:
                    Debug.LogError("此右键菜单无实现方法");
                    break;
            }
        }
        /// <summary>
        /// 检查节点连接
        /// </summary>
        private void CheckNodeConnect()
        {
            Story.CheckNull();
            if (UpNode != null && DownNode != null)
            {
                if (UpNode.GetType() == typeof(PlotNode))
                {
                    PlotNode pnu = UpNode as PlotNode;
                    if (DownNode.GetType() == typeof(PlotNode))
                    {
                        PlotNode pnd = DownNode as PlotNode;

                        if (pnu == pnd)
                        {
                            ShowNotification(new GUIContent("不可连接自己"));
                        }
                        else
                        {
                            pnu.CheckNull();
                            pnu.nextPlotNode = pnd.id;
                        }
                    }
                    else
                    {
                        OptionNode ond = DownNode as OptionNode;

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
                            ShowNotification(new GUIContent("已存在连接"));
                        }
                        else
                        {
                            pnu.nextOptionNodes.Add(ond.id);
                        }
                    }
                }
                else
                {
                    OptionNode onu = UpNode as OptionNode;
                    if (DownNode.GetType() == typeof(PlotNode))
                    {
                        PlotNode pnd = DownNode as PlotNode;

                        onu.nextPlotNode = pnd.id;
                    }
                    else
                    {
                        OptionNode ond = DownNode as OptionNode;

                        if (onu == ond)
                        {
                            ShowNotification(new GUIContent("不可连接自己"));
                        }
                        else
                        {
                            ShowNotification(new GUIContent("选项节点不能链接选项节点"));
                        }
                    }
                }

                ClearTempConnect();
            }
        }
        /// <summary>
        /// 检查当前节点
        /// </summary>
        private void CheckNode()
        {
            if (Story != null && Story.plotNodes != null)
            {
                //获取当前选中节点
                bool isInNode = false;
                foreach (PlotNode item in Story.plotNodes)
                {
                    if (item.layout.Contains(MousePos))
                    {
                        isInNode = true;
                        //选中此节点
                        Node = item;
                        break;
                    }
                }
                foreach (OptionNode item in Story.optionNodes)
                {
                    if (item.layout.Contains(MousePos))
                    {
                        isInNode = true;
                        //选中此节点
                        Node = item;
                        break;
                    }
                }

                if (isInNode)
                {
                }
                else
                {
                    Node = null;
                }
            }
        }
        /// <summary>
        /// 检查鼠标移动
        /// </summary>
        private void CheckMouseMove()
        {
            MousePos = new Vector2Int((int)((Event.current.mousePosition.x) + ScrollPos.x),
                                                 (int)((Event.current.mousePosition.y) + ScrollPos.y - Utility.GetHeightLong(1)));

        }

        //绘制
        /// <summary>
        /// 绘制右键菜单
        /// </summary>
        protected override void DrawMenu()
        {
            if (Event.current.type == EventType.ContextClick)
            {
                if (UpNode == null && DownNode == null)
                {
                    if (Story != null)
                    {
                        if (Menu == null)
                        {
                            Refresh();
                        }
                        Menu.ShowAsContext();
                        Event.current.Use();
                    }
                }
                else
                {
                    ClearTempConnect();
                }
            }
        }
        /// <summary>
        /// 绘制窗口背景
        /// </summary>
        protected override void DrawBG()
        {
            int xMin = (int)(ScrollPos.x);
            int xMax = (int)(ScrollPos.x + position.width);
            int yMin = (int)(ScrollPos.y);
            int yMax = (int)(ScrollPos.y + position.height);
            int xStart = xMin - xMin % GridInterval;
            int yStart = yMin - yMin % GridInterval;
            //画垂线
            for (int i = xStart; i <= xMax; i += GridInterval)
            {
                Vector2 start = new Vector2(i, ScrollPos.y);
                Vector2 end = new Vector2(i, ScrollPos.y + position.height);
                DrawLine(start, end);
            }
            //画平线
            for (int i = yStart; i <= yMax; i += GridInterval)
            {
                Vector2 start = new Vector2(ScrollPos.x, i);
                Vector2 end = new Vector2(ScrollPos.x + position.width, i);
                DrawLine(start, end);
            }
        }
        /// <summary>
        /// 绘制窗口顶部
        /// </summary>
        private void DrawTop()
        {
            //顶部背景
            //EditorGUI.DrawRect(Top, StoryWindowPreference.Background);

            //分割线
            DrawLine(TopLineLeft, TopLineRight, Color.black);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button( "创建", GUILayout.ExpandWidth(false), GUILayout.Height(18)))
            {
                CreateStory();
            }

            if (Story)
            {
                if (GUILayout.Button("删除", GUILayout.ExpandWidth(false), GUILayout.Height(18)))
                {
                    DeleteStory();
                }
                if (GUILayout.Button("关闭", GUILayout.ExpandWidth(false), GUILayout.Height(18)))
                {
                    CloseStory();
                }
                if (GUILayout.Button("保存", GUILayout.ExpandWidth(false), GUILayout.Height(18)))
                {
                    SaveStory();
                }
                if (GUILayout.Button(IsZoomOut ? "详情" : "缩略", GUILayout.ExpandWidth(false), GUILayout.Height(18)))
                {
                    IsZoomOut = !IsZoomOut;
                }
            }
            
            Story data = (Story)EditorGUILayout.ObjectField(Story, typeof(Story));
            OpenStory(data);

            if (GUILayout.Button( "刷新", GUILayout.ExpandWidth(false), GUILayout.Height(18)))
            {
                Refresh();
            }
            if (GUILayout.Button( "设置", GUILayout.ExpandWidth(false), GUILayout.Height(18)))
            {
                ShowNotification(new GUIContent("请前往 \"Edit -> Preferences -> E Story\" 进行编辑"));
            }
            EditorGUILayout.EndHorizontal();
        }
        /// <summary>
        /// 绘制窗口底部
        /// </summary>
        private void DrawButtom()
        {
            //EditorGUI.DrawRect(Buttom, StoryWindowPreference.NormalNode);
            string mousePos = "X: " + MousePos.x + "  Y: " + MousePos.y;
            Rect r1 = new Rect(Utility.OneSpacing, Center.y + Center.height - Utility.GetHeightMiddle(2) + 10, 120, Utility.OneHeight);
            EditorGUI.LabelField(r1, mousePos);
        }
        /// <summary>
        /// 绘制窗口中心
        /// </summary>
        private void DrawCenter()
        {
            Color c = GUI.color;
            if (Story != null)
            {
                Story.CheckNull();
                if (Story.plotNodes != null)
                {
                    for (int i = 0; i < Story.plotNodes.Count; i++)
                    {
                        PlotNode node = Story.plotNodes[i];
                        GUI.color = Node == node ? Color.yellow : c;

                        string id = string.Format("{0}-{1}-{2}-{3}", node.id.chapter, node.id.scene, node.id.part, node.id.branch);
                        node.layout = GUI.Window(i, node.layout, DrawPlotNode, id);

                        if (node.branchType != PlotBranchType.独立)
                        {
                            //节点类型链接按钮
                            switch (node.timeType)
                            {
                                case PlotTimeType.过渡:
                                    DrawUpButton(node);
                                    DrawDownButton(node);
                                    break;
                                case PlotTimeType.开局:
                                    DrawDownButton(node);
                                    break;
                                case PlotTimeType.结局:
                                    DrawUpButton(node);
                                    break;
                                default:
                                    break;
                            }
                        }
                        //连接线
                        DrawNodeCurve(node);
                    }
                    GUI.color = c;
                }
                if (Story.optionNodes != null)
                {
                    for (int i = 0; i < Story.optionNodes.Count; i++)
                    {
                        OptionNode node = Story.optionNodes[i];
                        GUI.color = Node == node ? Color.yellow : c;

                        node.layout = GUI.Window(i + Story.plotNodes.Count, node.layout, DrawOptionNode, node.id.ToString());

                        //链接按钮
                        DrawUpButton(node);
                        DrawDownButton(node);
                        DrawNodeCurve(node);
                    }
                    GUI.color = c;
                }
            }
            else
            {

            }
        }
        /// <summary>
        /// 绘制剧情节点
        /// </summary>
        private void DrawPlotNode(int index)
        {
            PlotNode node = Story.plotNodes[index];
            if (node != null)
            {
                if (IsZoomOut)
                {
                    node.layout.height = Utility.GetHeightLong(2) + Utility.OneSpacing;
                }
                else
                {
                    string[] yan = node.description.Split((char)10);
                    node.layout.height = Utility.GetHeightLong(3) + 15 * yan.Length + Utility.OneSpacing * 4;
                }

                Rect r1 = new Rect(Utility.OneSpacing * 2, Utility.OneSpacing, 50, Utility.OneHeight);
                Rect r2 = new Rect(r1.x + r1.width + Utility.OneSpacing, Utility.OneSpacing, 50, Utility.OneHeight);
                Rect r3 = new Rect(node.layout.width - Utility.GetHeightLong(1) - Utility.OneSpacing, Utility.OneSpacing, Utility.GetHeightMiddle(1), Utility.OneHeight);
                Rect r4 = new Rect(r3.x - r3.width - Utility.OneSpacing, r3.y, Utility.GetHeightMiddle(1), Utility.OneHeight);

                Utility.SetLabelWidth(50);

                //删除
                if (GUI.Button(r3, new GUIContent("X", "删除节点")))
                {
                    Story.RemoveNode(node);
                    return;
                }

                if (!IsZoomOut)
                {
                    //节点分支类型
                    PlotBranchType tp = (PlotBranchType)EditorGUI.EnumPopup(r1, node.branchType);
                    if (tp != node.branchType)
                    {
                        Story.SetNodeBranchType(node, tp);
                    }
                    if (tp != PlotBranchType.独立)
                    {
                        //节点时间类型
                        PlotTimeType tp2 = (PlotTimeType)EditorGUI.EnumPopup(r2, node.timeType);
                        if (tp2 != node.timeType)
                        {
                            Story.SetNodeTimeType(node, tp2);
                        }
                        //清空
                        if (GUI.Button(r4, new GUIContent("O", "清空链接")))
                        {
                            Story.ClearNodeUpChoices(Node);
                            Story.ClearNodeDownChoices(Node);
                        }
                    }

                    //编号
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("剧情编号", GUILayout.Width(50));
                    int chapter = EditorGUILayout.IntField(node.id.chapter);
                    int scene = EditorGUILayout.IntField(node.id.scene);
                    int part = EditorGUILayout.IntField(node.id.part);
                    int branch = EditorGUILayout.IntField(node.id.branch);
                    PlotID id = new PlotID(chapter, scene, part, branch);
                    if (!id.Equals(node.id)) Story.SetPlotID(node, id);
                    EditorGUILayout.EndHorizontal();

                }

                //内容
                EditorGUILayout.BeginHorizontal();
                if (!IsZoomOut)
                {
                    EditorGUILayout.LabelField("剧情片段", GUILayout.Width(50));
                }
                node.plot = (Plot)EditorGUILayout.ObjectField(node.plot, typeof(Plot));
                if (!IsZoomOut)
                {
                    if (GUILayout.Button(new GUIContent("+", "创建剧情片段"), GUILayout.ExpandWidth(false)))
                    {
                        CreatePlot();
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (!IsZoomOut)
                {
                    //简介
                    node.description = EditorGUILayout.TextArea(node.description);
                }
            }

            GUI.DragWindow(new Rect(0, 0, StoryWindowPreference.ViewSize.x, StoryWindowPreference.ViewSize.y));
        }
        /// <summary>
        /// 绘制选项节点
        /// </summary>
        private void DrawOptionNode(int index)
        {
            OptionNode node = Story.optionNodes[index - Story.plotNodes.Count];

            if (IsZoomOut)
            {
                node.layout.height = Utility.GetHeightLong(2) + Utility.OneSpacing;
            }
            else
            {
                int cp = node.comparisons.Count;
                int ch = node.changes.Count;
                int lines = cp + ch;
                node.layout.height = Utility.GetHeightLong(lines + 4) + Utility.OneSpacing * 2;
            }
            Rect r1 = new Rect(node.layout.width - Utility.GetHeightLong(1) - Utility.OneSpacing, Utility.OneSpacing, Utility.GetHeightMiddle(1), Utility.OneHeight);
            Rect r4 = new Rect(r1.x - r1.width - Utility.OneSpacing, r1.y, Utility.GetHeightMiddle(1), Utility.OneHeight);

            //删除
            if (GUI.Button(r1, new GUIContent("X", "删除节点")))
            {
                Story.RemoveNode(node);
                return;
            }

            if (!IsZoomOut)
            {
                //清空
                if (GUI.Button(r4, new GUIContent("O", "清空链接")))
                {
                    Story.ClearNodeUpChoices(Node);
                    Story.ClearNodeDownChoices(Node);
                }
            }
            //选项描述
            node.description = EditorGUILayout.TextField(node.description);

            if (!IsZoomOut)
            {
                //通过条件
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("添加通过条件"))
                {
                    if (Story.conditions.Count > 0)
                    {
                        node.comparisons.Add(new ConditionComparison());
                    }
                    else
                    {
                        ShowNotification(new GUIContent("未设置条件"));
                    }
                }
                EditorGUILayout.EndHorizontal();

                for (int j = 0; j < node.comparisons.Count; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    node.comparisons[j].keyIndex = EditorGUILayout.Popup(node.comparisons[j].keyIndex, Story.ConditionKeys);
                    node.comparisons[j].comparison = (Comparison)EditorGUILayout.EnumPopup(node.comparisons[j].comparison);
                    node.comparisons[j].value = EditorGUILayout.IntField(node.comparisons[j].value);
                    if (GUILayout.Button(new GUIContent("X", "移除通过条件"), GUILayout.ExpandWidth(false)))
                    {
                        node.comparisons.RemoveAt(j);
                        j--;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                //数值变动
                if (GUILayout.Button("添加数值变动"))
                {
                    if (Story.conditions.Count > 0)
                    {
                        node.changes.Add(new ConditionChange());
                    }
                    else
                    {
                        ShowNotification(new GUIContent("未设置条件"));
                    }
                }
                for (int j = 0; j < node.changes.Count; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    node.changes[j].keyIndex = EditorGUILayout.Popup(node.changes[j].keyIndex, Story.ConditionKeys);
                    node.changes[j].change = (Change)EditorGUILayout.EnumPopup(node.changes[j].change);
                    node.changes[j].value = EditorGUILayout.IntField(node.changes[j].value);
                    if (GUILayout.Button(new GUIContent("X", "移除数值变动"), GUILayout.ExpandWidth(false)))
                    {
                        node.changes.RemoveAt(j);
                        j--;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            GUI.DragWindow(new Rect(0, 0, StoryWindowPreference.ViewSize.x, StoryWindowPreference.ViewSize.y));
        }
        /// <summary>
        /// 绘制节点上按钮
        /// </summary>
        /// <param name="node"></param>
        private void DrawUpButton(Node node)
        {
            Rect rect = new Rect(node.layout.x + (node.layout.width - 25) / 2, node.layout.y - 25, 25, 25);
            if (GUI.Button(rect, new GUIContent("O", "链接上一个节点")))
            {
                DownNode = node;
                CheckNodeConnect();
            }
        }
        /// <summary>
        /// 绘制节点下按钮
        /// </summary>
        /// <param name="node"></param>
        private void DrawDownButton(Node node)
        {
            Rect rect = new Rect(node.layout.x + (node.layout.width - 25) / 2, node.layout.y + node.layout.height, 25, 25);
            if (GUI.Button(rect, new GUIContent("O", "链接下一个节点")))
            {
                UpNode = node;
                CheckNodeConnect();
            }
        }
        /// <summary>
        /// 绘制连接线
        /// </summary>
        private void DrawNodeCurve(Node node)
        {
            if (node.GetType() == typeof(PlotNode))
            {
                PlotNode pn = node as PlotNode;

                if (pn.nextOptionNodes != null)
                {
                    foreach (int item in pn.nextOptionNodes)
                    {
                        OptionNode on = Story.GetNode(item);
                        if (on == null) continue;
                        Vector2 start = new Vector2(pn.layout.x + pn.layout.width / 2, pn.layout.y + pn.layout.height + 25);
                        Vector2 end = new Vector2(on.layout.x + on.layout.width / 2, on.layout.y - 25);

                        PlotNode next = Story.GetNode(on.nextPlotNode);
                        bool isMainPlot = false;
                        if (next != null)
                        {
                            isMainPlot = pn.branchType == PlotBranchType.主线 && next.branchType == PlotBranchType.主线;
                        }
                        DrawCurve(start, end, isMainPlot, Vector3.up, Vector3.down);
                    }
                }
                if (!pn.nextPlotNode.Equals(new PlotID()))
                {
                    PlotNode next = Story.GetNode(pn.nextPlotNode);
                    if (next == null) return;
                    Vector2 start = new Vector2(pn.layout.x + pn.layout.width / 2, pn.layout.y + pn.layout.height + 25);
                    Vector2 end = new Vector2(next.layout.x + next.layout.width / 2, next.layout.y - 25);

                    bool isMainPlot = pn.branchType == PlotBranchType.主线 && next.branchType == PlotBranchType.主线;
                    DrawCurve(start, end, isMainPlot, Vector3.up, Vector3.down);
                }
            }
            else
            {
                OptionNode on = node as OptionNode;

                //连接线
                if (!on.nextPlotNode.Equals(new PlotID()))
                {
                    PlotNode next = Story.GetNode(on.nextPlotNode);
                    if (next == null) return;
                    Vector2 start = new Vector2(on.layout.x + on.layout.width / 2, on.layout.y + on.layout.height + 25);
                    Vector2 end = new Vector2(next.layout.x + next.layout.width / 2, next.layout.y - 25);

                    bool isLastMainPlot = false;
                    foreach (PlotNode item in Story.plotNodes)
                    {
                        if (item.nextOptionNodes.Contains(on.id))
                        {
                            if (item.branchType == PlotBranchType.主线)
                            {
                                isLastMainPlot = true;
                                break;
                            }
                        }
                    }
                    bool isMainPlot = next.branchType == PlotBranchType.主线 && isLastMainPlot;
                    DrawCurve(start, end, isMainPlot, Vector3.up, Vector3.down);
                }
            }
        }
        /// <summary>
        /// 绘制节点预览连接线
        /// </summary>
        private void DrawTempCurve()
        {
            if (UpNode != null)
            {
                Vector2 start = new Vector2(UpNode.layout.x + UpNode.layout.width / 2, UpNode.layout.y + UpNode.layout.height + 25);
                Vector2 end = MousePos;
                DrawCurve(start, end, true, Vector3.up, Vector3.down);
            }
            if (DownNode != null)
            {
                Vector2 start = MousePos;
                Vector2 end = new Vector2(DownNode.layout.x + DownNode.layout.width / 2, DownNode.layout.y - 25);
                DrawCurve(start, end, true, Vector3.up, Vector3.down);
            }
        }
        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="start">起始节点窗口</param>
        /// <param name="end">结束节点窗口</param>
        /// <param name="isMainLine">是否绘制成主线高亮颜色</param>
        private void DrawCurve(Vector2 start, Vector2 end, bool isMainLine, Vector3 startDirc, Vector3 endDirc)
        {
            Vector3 startPos = new Vector3(start.x, start.y, 0);
            Vector3 endPos = new Vector3(end.x, end.y, 0);
            Vector3 startTan = startPos + startDirc * BendingFactor;
            Vector3 endTan = endPos + endDirc * BendingFactor;
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
    }
}
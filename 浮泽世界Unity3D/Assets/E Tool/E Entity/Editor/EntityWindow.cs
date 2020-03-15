using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using E.Tool;
using System.Linq;
using System.Reflection;
using System.Collections;
using UnityEditorInternal;
using System.IO;
using Object = UnityEngine.Object;

namespace E.Tool
{
    public class EntityWindow : MyEditorWindow<EntityWindow>
    {
        private Rect LeftTop { get => new Rect(0, 0, position.width / 4, Utility.GetHeightLong(4)); }
        private Rect LeftTopContent { get => new Rect(2, 2, LeftTop.width -4, LeftTop.height -4); }

        private float DatasHeight { get => Utility.GetHeightLong(Entitys.Count + 1) + 2; }
        private float DatasWeight { get => LeftDown.height < DatasHeight ? LeftDown.width - 16 : LeftDown.width - 4; }
        private Rect LeftDown { get => new Rect(0, Utility.GetHeightLong(4), position.width / 4, position.height - Utility.GetHeightLong(4)); }
        private Rect LeftScrollPosition { get => new Rect(0, 0, LeftDown.width, LeftDown.height); }
        private Rect LeftScrollView { get => new Rect(0, 0, DatasWeight, DatasHeight); }
        private Rect LeftScrollContent { get => new Rect(2, 2, DatasWeight, DatasHeight); }

        private int itemStack;
        private int roleStack;
        private int skillStack;
        private float InfoHeight 
        {
            get 
            {
                float f = 0;
                f += Utility.GetHeightMiddle(folder1 ? 6 : 1);
                f += Utility.GetHeightMiddle(folder2 ? 34 : 1);
                f += Utility.GetHeightMiddle(itemStack + 1);
                f += Utility.GetHeightMiddle(roleStack + 1);
                f += Utility.GetHeightMiddle(skillStack + 1);
                f += Utility.OneSpacing * 2;
                return f;
            }
        }
        private float InfoWeight { get => Right.height < InfoHeight ? Right.width - 16 : Right.width - 4; }
        private Rect Right { get => new Rect(position.width / 4, 0, position.width - position.width / 4 , position.height); }
        private Rect RightScrollPosition { get => new Rect(0, 0, Right.width, Right.height); }
        private Rect RightScrollView { get => new Rect(0, 0, InfoWeight, InfoHeight); }
        private Rect RightScrollContent { get => new Rect(3, 2, InfoWeight, InfoHeight); }

        private Vector2 TopLineLeft { get => new Vector2(LeftTop.x, LeftTop.height); }
        private Vector2 TopLineRight { get => new Vector2(LeftTop.width, LeftTop.height); }
        private Vector2 LeftLineTop { get => new Vector2(LeftTop.width, 0); }
        private Vector2 LeftLineButtom { get => new Vector2(LeftTop.width, position.height); }

        private string DefaultFolder 
        { 
            get => "Assets/E Tool/E Entity/Entitys/"; 
        }
        private string RoleFolder
        {
            get
            {
                return EntityFolder;
            }
            set
            {
                if (EntityFolder != value)
                {
                    EntityFolder = value;
                    EditorPrefs.SetString("RoleFolder", value);
                    LoadRoles();
                }
            }
        }
        private string ItemFolder
        {
            get
            {
                return EntityFolder;
            }
            set
            {
                if (EntityFolder != value)
                {
                    EntityFolder = value;
                    EditorPrefs.SetString("ItemFolder", value);
                    LoadItems();
                }
            }
        }
        private string BuildingFolder
        {
            get
            {
                return EntityFolder;
            }
            set
            {
                if (EntityFolder != value)
                {
                    EntityFolder = value;
                    EditorPrefs.SetString("BuildingFolder", value);
                    LoadBuildings();
                }
            }
        }
        private string[] TypeNames = new string[] { "角色", "物品", "建筑" };
        private string[] HeightNames = new string[] { "身高", "高度", "高度" };

        private Vector2 LeftScrollPos { get; set; }
        private Vector2 RightScrollPos { get; set; }
        private int EntityType { get; set; } = 0;
        private int[] Indexs { get; set; } = new int[] { 0, 0, 0 };
        private string EntityFolder { get; set; }
        private string[] EntityNames { get; set; }
        private List<EntityStaticData> Entitys { get; set; } = new List<EntityStaticData>();
        private EntityStaticData Entity 
        { 
            get
            {
                if (Entitys.Count > 0)
                {
                    return Entitys[Indexs[EntityType]];
                }
                return null;
            } 
        }

        private bool folder1;
        private bool folder2;

        private void OnEnable()
        {
            LoadFolders();
            LoadAssets(EntityType);
        }
        private void Update()
        {
            Repaint();
        }
        private void OnGUI()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    break;
                case EventType.MouseUp:
                    break;
                case EventType.MouseMove:
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

            DrawBG();
            DrawLeft();
            DrawRight();
        }
        private void OnFocus()
        {
            Instance.wantsMouseMove = true;
        }
        private void OnProjectChange()
        {
            LoadAssets(EntityType);
        }

        //打开
        /// <summary>
        /// 打开窗口
        /// </summary>
        [MenuItem("Window/E Tool/打开实体编辑器 E Entity %#e")]
        public static void Open()
        {
            Instance.Refresh();
        }

        ///关闭

        //创建
        private void CreateEntity()
        {
            string path = EditorUtility.SaveFilePanelInProject("创建"+ TypeNames[EntityType], "新"+ TypeNames[EntityType], "Asset", "保存"+ TypeNames[EntityType], DefaultFolder);
            if (path == "") return;
            EntityStaticData data;
            switch (EntityType)
            {
                case 0:
                    data = CreateInstance<RoleStaticData>();
                    AssetDatabase.CreateAsset(data, path);
                    break;
                case 1:
                    data = CreateInstance<ItemStaticData>();
                    AssetDatabase.CreateAsset(data, path);
                    break;
                case 2:
                    data = CreateInstance<BuildingStaticData>();
                    AssetDatabase.CreateAsset(data, path);
                    break;
                default:
                    break;
            }
            //AssetDatabase.Refresh();
        }

        //载入
        private void LoadFolders()
        {
            RoleFolder = DefaultFolder;
            ItemFolder = DefaultFolder;
            BuildingFolder = DefaultFolder;

            if (EditorPrefs.HasKey("RoleFolder"))
            {
                string path = EditorPrefs.GetString("RoleFolder");
                if (path.Contains("Application.dataPath") && Directory.Exists(path))
                {
                    RoleFolder = path;
                }
            }
            if (EditorPrefs.HasKey("ItemFolder"))
            {
                string path = EditorPrefs.GetString("ItemFolder");
                if (path.Contains("Application.dataPath") && Directory.Exists(path))
                {
                    ItemFolder = path;
                }
            }
            if (EditorPrefs.HasKey("BuildingFolder"))
            {
                string path = EditorPrefs.GetString("BuildingFolder");
                if (path.Contains("Application.dataPath") && Directory.Exists(path))
                {
                    BuildingFolder = path;
                }
            }
        }
        private void LoadAssets(int type)
        {
            switch (type)
            {
                case 0:
                    LoadRoles();
                    break;
                case 1:
                    LoadItems();
                    break;
                case 2:
                    LoadBuildings();
                    break;
                default:
                    break;
            }
        }
        private void LoadRoles()
        {
            string path = RoleFolder;
            Entitys.Clear();

            if (Directory.Exists(path))
            {
                string[] objects = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                foreach (string item in objects)
                {
                    string strTempPath = item.Replace(@"\", "/");
                    strTempPath = strTempPath.Substring(strTempPath.IndexOf("Assets"));
                    RoleStaticData r = (RoleStaticData)AssetDatabase.LoadAssetAtPath(@strTempPath, typeof(RoleStaticData));
                    if (r) { Entitys.Add(r); }
                }

                List<string> str = new List<string>();
                for (int i = 0; i < Entitys.Count; i++)
                {
                    str.Add(i + ":  " + Entitys[i].name);
                }
                EntityNames = str.ToArray();
            }

            //Debug.Log(string.Format("已载入 {0} 个角色数据", Roles.Count));
        }
        private void LoadItems()
        {
            string path = ItemFolder;
            Entitys.Clear();
            if (Directory.Exists(path))
            {
                string[] objects = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                foreach (string item in objects)
                {
                    string strTempPath = item.Replace(@"\", "/");
                    strTempPath = strTempPath.Substring(strTempPath.IndexOf("Assets"));
                    ItemStaticData i = (ItemStaticData)AssetDatabase.LoadAssetAtPath(@strTempPath, typeof(ItemStaticData));
                    if (i) { Entitys.Add(i); }
                }

                List<string> str = new List<string>();
                for (int i = 0; i < Entitys.Count; i++)
                {
                    str.Add(i + ":  " + Entitys[i].name);
                }
                EntityNames = str.ToArray();
            }

            //Debug.Log(string.Format("已载入 {0} 个物品数据", Items.Count));
        }
        private void LoadBuildings()
        {
            string path = BuildingFolder;
            Entitys.Clear();
            if (Directory.Exists(path))
            {
                string[] objects = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                foreach (string item in objects)
                {
                    string strTempPath = item.Replace(@"\", "/");
                    strTempPath = strTempPath.Substring(strTempPath.IndexOf("Assets"));
                    BuildingStaticData b = (BuildingStaticData)AssetDatabase.LoadAssetAtPath(@strTempPath, typeof(BuildingStaticData));
                    if (b) { Entitys.Add(b); }
                }

                List<string> str = new List<string>();
                for (int i = 0; i < Entitys.Count; i++)
                {
                    str.Add(i + ":  " + Entitys[i].name);
                }
                EntityNames = str.ToArray();
            }

            //Debug.Log(string.Format("已载入 {0} 个建筑数据", Buildings.Count));
        }

        //保存
        private void SaveEntity()
        {
            AssetDatabase.SaveAssets();
            ShowNotification(new GUIContent("已保存"));
        }

        //删除
        private void DeleteEntity()
        {
            if (Entity)
            {
                string str = string.Format("确认要删除 {0} 吗？这将无法恢复。", Entity.name);
                if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
                {
                    string path = AssetDatabase.GetAssetPath(Entity);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();
                    Debug.Log("已删除 {" + path + "}");
                }
            }
            else
            {
                ShowNotification(new GUIContent("当前选择实体"));
            }
        }

        ///清除

        //刷新
        /// <summary>
        /// 刷新窗口
        /// </summary>
        protected override void Refresh()
        {
            Instance.titleContent = new GUIContent("E Entity", EditorGUIUtility.IconContent("EditCollider").image);
            Instance.minSize = new Vector2(560, 200);

            Menu = new GenericMenu();
            Menu.AddItem(new GUIContent("创建剧情节点"), false, CheckMenuClick, 2);
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
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
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
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;

                default:
                    Debug.LogError("此右键菜单无实现方法");
                    break;
            }
        }

        //绘制
        /// <summary>
        /// 绘制右键菜单
        /// </summary>
        protected override void DrawMenu()
        {
            if (Event.current.type == EventType.ContextClick)
            {
                if (Menu == null)
                {
                    Refresh();
                }
                Menu.ShowAsContext();
                Event.current.Use();
            }
        }
        /// <summary>
        /// 绘制窗口背景
        /// </summary>
        protected override void DrawBG()
        {
        }
        /// <summary>
        /// 绘制窗口左部
        /// </summary>
        private void DrawLeft()
        {
            Utility.SetLabelWidth(80);
            GUILayout.BeginArea(LeftTopContent);
            int type = GUILayout.Toolbar(EntityType, TypeNames, GUILayout.Height(18));
            if (type != EntityType)
            {
                EntityType = type;
                LoadAssets(EntityType);
            }
            switch (EntityType)
            {
                case 0:
                    RoleFolder = EditorGUILayout.DelayedTextField(RoleFolder);
                    break;
                case 1:
                    ItemFolder = EditorGUILayout.DelayedTextField(ItemFolder);
                    break;
                case 2:
                    BuildingFolder = EditorGUILayout.DelayedTextField(BuildingFolder);
                    break;
                default:
                    break;
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("浏览", GUILayout.Height(18)))
            {
            }
            if (GUILayout.Button("刷新", GUILayout.Height(18)))
            {
                Refresh();
            }
            if (GUILayout.Button("设置", GUILayout.Height(18)))
            {
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("创建", GUILayout.Height(18)))
            {
                CreateEntity();
            }
            if (GUILayout.Button("删除", GUILayout.Height(18)))
            {
                DeleteEntity();
            }
            if (GUILayout.Button("保存", GUILayout.Height(18)))
            {
                SaveEntity();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();

            GUILayout.BeginArea(LeftDown);
            LeftScrollPos = GUI.BeginScrollView(LeftScrollPosition, LeftScrollPos, LeftScrollView);
            GUILayout.BeginArea(LeftScrollContent);
            if (Entitys.Count > 0)
            {
                Indexs[EntityType] = GUILayout.SelectionGrid(Indexs[EntityType], EntityNames, 1);
                if (Indexs[EntityType] >= Entitys.Count)
                {
                    Indexs[EntityType] = Entitys.Count - 1;
                }
            }
            GUILayout.EndArea();
            GUI.EndScrollView();
            GUILayout.EndArea();

            //分割线
            DrawLine(TopLineLeft, TopLineRight, Color.black);
            DrawLine(LeftLineTop, LeftLineButtom);
        }
        /// <summary>
        /// 绘制窗口右部
        /// </summary>
        private void DrawRight()
        {
            Utility.SetFieldWidth(30);
            GUILayout.BeginArea(Right);
            RightScrollPos = GUI.BeginScrollView(RightScrollPosition, RightScrollPos, RightScrollView);
            GUILayout.BeginArea(RightScrollContent);
            if (Entity)
            {
                EditorUtility.SetDirty(Entity);

                folder1 = EditorGUILayout.BeginFoldoutHeaderGroup(folder1, "实体信息");
                if (folder1)
                {
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.BeginHorizontal();
                    Entity.icon = (Sprite)EditorGUILayout.ObjectField(Entity.icon, typeof(Sprite), GUILayout.Height(100), GUILayout.Width(115));
                    
                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.BeginVertical();
                    Entity.name = EditorGUILayout.DelayedTextField(Entity.name);
                    Entity.description = EditorGUILayout.TextArea(Entity.description, GUILayout.Height(80));
                    EditorGUILayout.EndVertical();

                    Utility.SetLabelWidth(39);
                    EditorGUILayout.BeginVertical();
                    Entity.prefab = (GameObject)EditorGUILayout.ObjectField("预制体", Entity.prefab, typeof(GameObject));
                    Entity.volume = EditorGUILayout.IntField("体积", Entity.volume);
                    Entity.weight = EditorGUILayout.IntField("质量", Entity.weight);
                    Entity.height = EditorGUILayout.IntField(HeightNames[EntityType], Entity.height);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("生日");
                    Utility.SetLabelWidth(13);
                    int year = EditorGUILayout.IntField("年", Entity.birthday.x);
                    int month = EditorGUILayout.IntField("月", Entity.birthday.y);
                    int day = EditorGUILayout.IntField("日", Entity.birthday.z);
                    Utility.SetLabelWidth(80);
                    Entity.birthday = new Vector3Int(year, month, day);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.EndFoldoutHeaderGroup();

                folder2 = EditorGUILayout.BeginFoldoutHeaderGroup(folder2, TypeNames[EntityType] + "信息");
                if (folder2)
                {
                    EditorGUI.indentLevel = 1;
                    switch (EntityType)
                    {
                        case 0:
                            RoleStaticData rsd = Entity as RoleStaticData;
                            rsd.race = (Race)EditorGUILayout.EnumPopup("种族", rsd.race);
                            rsd.gender = (Gender)EditorGUILayout.EnumPopup("性别", rsd.gender);

                            Utility.SetLabelWidth(60);
                            Utility.SetFieldWidth(50);

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("社会信息", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel = 0;
                            rsd.social.id = EditorGUILayout.TextField("身份证号", rsd.social.id);
                            rsd.social.fz = EditorGUILayout.TextField("浮泽编号", rsd.social.fz);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("学历信息", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel = 0;
                            rsd.educational.degree = (Degree)EditorGUILayout.EnumPopup("学位", rsd.educational.degree);
                            rsd.educational.startYear = EditorGUILayout.IntField("学届", rsd.educational.startYear);
                            rsd.educational.university = EditorGUILayout.TextField("学校", rsd.educational.university);
                            rsd.educational.college = EditorGUILayout.TextField("学院", rsd.educational.college);
                            rsd.educational.profession = EditorGUILayout.TextField("专业", rsd.educational.profession);
                            rsd.educational.grade = EditorGUILayout.IntField("年级", rsd.educational.grade);
                            rsd.educational.@class = EditorGUILayout.TextField("班级", rsd.educational.@class);
                            rsd.educational.studentID = EditorGUILayout.TextField("学号", rsd.educational.studentID);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("性格信息", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel = 0;
                            rsd.personality.evilOrGood = EditorGUILayout.IntSlider("善良邪恶值", rsd.personality.evilOrGood, -150, 150);
                            rsd.personality.chaosOrLaw = EditorGUILayout.IntSlider("混乱守序值", rsd.personality.chaosOrLaw, -150, 150);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("生理素质", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel = 0;
                            rsd.physique.force = EditorGUILayout.IntField("力量上限", rsd.physique.force);
                            rsd.physique.speed = EditorGUILayout.IntField("速度上限", rsd.physique.speed);
                            rsd.physique.defense = EditorGUILayout.IntField("防御上限", rsd.physique.defense);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("心理素质", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel = 0;
                            rsd.mentality.memory = EditorGUILayout.IntField("记忆能力", rsd.mentality.memory);
                            rsd.mentality.logical = EditorGUILayout.IntField("逻辑能力", rsd.mentality.logical);
                            rsd.mentality.imagination = EditorGUILayout.IntField("想象能力", rsd.mentality.imagination);
                            rsd.mentality.expression = EditorGUILayout.IntField("表达能力", rsd.mentality.expression);
                            rsd.mentality.reaction = EditorGUILayout.IntField("应激能力", rsd.mentality.reaction);
                            rsd.mentality.courage = EditorGUILayout.IntField("胆魄能力", rsd.mentality.courage);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("感官素质", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel = 0;
                            rsd.sense.see = EditorGUILayout.IntField("视觉", rsd.sense.see);
                            rsd.sense.hear = EditorGUILayout.IntField("听觉", rsd.sense.hear);
                            rsd.sense.smell = EditorGUILayout.IntField("嗅觉", rsd.sense.smell);
                            rsd.sense.taste = EditorGUILayout.IntField("味觉", rsd.sense.taste);
                            rsd.sense.touch = EditorGUILayout.IntField("触觉", rsd.sense.touch);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("身体状态", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUI.indentLevel = 0;
                            rsd.bodyState.health.Max = EditorGUILayout.FloatField("生机", rsd.bodyState.health.Max);
                            rsd.bodyState.power.Max = EditorGUILayout.FloatField("体力", rsd.bodyState.power.Max);
                            rsd.bodyState.mind.Max = EditorGUILayout.FloatField("脑力", rsd.bodyState.mind.Max);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("拥有技能", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel = 0;
                            if (GUILayout.Button("+", GUILayout.Width(29), GUILayout.Height(18)))
                            {
                                rsd.skills.Add(new Skill());
                            }
                            if (GUILayout.Button("-", GUILayout.Width(28), GUILayout.Height(18)))
                            {
                                if (rsd.skills.Count > 0)
                                {
                                    rsd.skills.RemoveAt(rsd.skills.Count - 1);
                                }
                            }
                            int count3 = EditorGUILayout.IntField(rsd.skills.Count);
                            skillStack = count3;
                            if (count3 < rsd.skills.Count)
                            {
                                int a = rsd.skills.Count - count3;
                                rsd.skills.RemoveRange(rsd.skills.Count - a, a);
                            }
                            else if (count3 > rsd.skills.Count)
                            {
                                int a = count3 - rsd.skills.Count;
                                for (int i = 0; i < a; i++)
                                {
                                    rsd.skills.Add(new Skill());
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            Utility.SetLabelWidth(26);
                            for (int i = 0; i < rsd.skills.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Button("-", GUILayout.Width(60), GUILayout.Height(18)))
                                {
                                    rsd.skills.RemoveAt(i);
                                    i--;
                                    continue;
                                }
                                rsd.skills[i].skillTpye = (SkillTpye)EditorGUILayout.EnumPopup("名称", rsd.skills[i].skillTpye);
                                rsd.skills[i].level = (SkillLevel)EditorGUILayout.EnumPopup("等级", rsd.skills[i].level);
                                rsd.skills[i].levelEx = EditorGUILayout.Slider("经验", rsd.skills[i].levelEx, 0, 1);
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("人际关系", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel = 0;
                            if (GUILayout.Button("+", GUILayout.Width(29), GUILayout.Height(18)))
                            {
                                rsd.relationships.Add(new Relationship());
                            }
                            if (GUILayout.Button("-", GUILayout.Width(28), GUILayout.Height(18)))
                            {
                                if (rsd.relationships.Count > 0)
                                {
                                    rsd.relationships.RemoveAt(rsd.relationships.Count - 1);
                                }
                            }
                            int count2 = EditorGUILayout.IntField(rsd.relationships.Count);
                            roleStack = count2;
                            if (count2 < rsd.relationships.Count)
                            {
                                int a = rsd.relationships.Count - count2;
                                rsd.relationships.RemoveRange(rsd.relationships.Count - a, a);
                            }
                            else if (count2 > rsd.relationships.Count)
                            {
                                int a = count2 - rsd.relationships.Count;
                                for (int i = 0; i < a; i++)
                                {
                                    rsd.relationships.Add(new Relationship());
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            Utility.SetLabelWidth(39);
                            for (int i = 0; i < rsd.relationships.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Button("-", GUILayout.Width(60), GUILayout.Height(18)))
                                {
                                    rsd.relationships.RemoveAt(i);
                                    i--;
                                    continue;
                                }
                                rsd.relationships[i].role = (RoleStaticData)EditorGUILayout.ObjectField(rsd.relationships[i].role, typeof(RoleStaticData));
                                rsd.relationships[i].favorability = EditorGUILayout.IntField("好感度", rsd.relationships[i].favorability);
                                rsd.relationships[i].familiarity = EditorGUILayout.IntField("熟悉度", rsd.relationships[i].familiarity);
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("携带物品", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel = 0;
                            if (GUILayout.Button("+", GUILayout.Width(29), GUILayout.Height(18)))
                            {
                                rsd.items.Add(new ItemStack());
                            }
                            if (GUILayout.Button("-", GUILayout.Width(28), GUILayout.Height(18)))
                            {
                                if (rsd.items.Count > 0)
                                {
                                    rsd.items.RemoveAt(rsd.items.Count - 1);
                                }
                            }
                            int count = EditorGUILayout.IntField(rsd.items.Count);
                            itemStack = count;
                            if (count < rsd.items.Count)
                            {
                                int a = rsd.items.Count - count;
                                rsd.items.RemoveRange(rsd.items.Count - a, a);
                            }
                            else if (count > rsd.items.Count)
                            {
                                int a = count - rsd.items.Count;
                                for (int i = 0; i < a; i++)
                                {
                                    rsd.items.Add(new ItemStack());
                                }
                            }
                            EditorGUILayout.EndHorizontal(); 
                            Utility.SetLabelWidth(26);
                            for (int i = 0; i < rsd.items.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Button("-", GUILayout.Width(60), GUILayout.Height(18)))
                                {
                                    rsd.items.RemoveAt(i);
                                    i--;
                                    continue;
                                }
                                rsd.items[i].item = (ItemStaticData)EditorGUILayout.ObjectField(rsd.items[i].item, typeof(ItemStaticData));
                                rsd.items[i].stack = EditorGUILayout.IntField("数量", rsd.items[i].stack);
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            Utility.SetLabelWidth(80);
                            rsd.rmb = EditorGUILayout.IntField("拥有人民币", rsd.rmb);
                            rsd.fzb = EditorGUILayout.IntField("拥有浮泽币", rsd.fzb);

                            break;
                        case 1:
                            ItemStaticData isd = Entity as ItemStaticData;
                            isd.type = (ItemType)EditorGUILayout.EnumPopup("类型", isd.type);
                            isd.health.Max = EditorGUILayout.FloatField("耐久", isd.health.Max);
                            isd.power.Max = EditorGUILayout.FloatField("能量", isd.power.Max);
                            isd.rmbPrice = EditorGUILayout.IntField("价格", isd.rmbPrice);
                            isd.capacity = EditorGUILayout.IntField("容量", isd.capacity);
                            isd.capacity = EditorGUILayout.IntField("容量", isd.capacity);

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("容纳物品", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel = 0;
                            if (GUILayout.Button("+", GUILayout.Width(29), GUILayout.Height(18)))
                            {
                                isd.items.Add(new ItemStack());
                            }
                            if (GUILayout.Button("-", GUILayout.Width(28), GUILayout.Height(18)))
                            {
                                if (isd.items.Count > 0)
                                {
                                    isd.items.RemoveAt(isd.items.Count - 1);
                                }
                            }
                            int count4 = EditorGUILayout.IntField(isd.items.Count);
                            itemStack = count4;
                            if (count4 < isd.items.Count)
                            {
                                int a = isd.items.Count - count4;
                                isd.items.RemoveRange(isd.items.Count - a, a);
                            }
                            else if (count4 > isd.items.Count)
                            {
                                int a = count4 - isd.items.Count;
                                for (int i = 0; i < a; i++)
                                {
                                    isd.items.Add(new ItemStack());
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            Utility.SetLabelWidth(26);
                            for (int i = 0; i < isd.items.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Button("-", GUILayout.Width(60), GUILayout.Height(18)))
                                {
                                    isd.items.RemoveAt(i);
                                    i--;
                                    continue;
                                }
                                isd.items[i].item = (ItemStaticData)EditorGUILayout.ObjectField(isd.items[i].item, typeof(ItemStaticData));
                                isd.items[i].stack = EditorGUILayout.IntField("数量", isd.items[i].stack);
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            EditorGUI.indentLevel = 1;
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("组成部件", GUILayout.Width(80));
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel = 0;
                            if (GUILayout.Button("+", GUILayout.Width(29), GUILayout.Height(18)))
                            {
                                isd.components.Add(new ItemStack());
                            }
                            if (GUILayout.Button("-", GUILayout.Width(28), GUILayout.Height(18)))
                            {
                                if (isd.components.Count > 0)
                                {
                                    isd.components.RemoveAt(isd.components.Count - 1);
                                }
                            }
                            int count5 = EditorGUILayout.IntField(isd.components.Count);
                            itemStack = count5;
                            if (count5 < isd.components.Count)
                            {
                                int a = isd.components.Count - count5;
                                isd.components.RemoveRange(isd.components.Count - a, a);
                            }
                            else if (count5 > isd.components.Count)
                            {
                                int a = count5 - isd.components.Count;
                                for (int i = 0; i < a; i++)
                                {
                                    isd.components.Add(new ItemStack());
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            Utility.SetLabelWidth(26);
                            for (int i = 0; i < isd.components.Count; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Button("-", GUILayout.Width(60), GUILayout.Height(18)))
                                {
                                    isd.components.RemoveAt(i);
                                    i--;
                                    continue;
                                }
                                isd.components[i].item = (ItemStaticData)EditorGUILayout.ObjectField(isd.components[i].item, typeof(ItemStaticData));
                                isd.components[i].stack = EditorGUILayout.IntField("数量", isd.components[i].stack);
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            break;
                        case 2:
                            BuildingStaticData bsd = Entity as BuildingStaticData;
                            bsd.health.Max = EditorGUILayout.FloatField("耐久", bsd.health.Max);
                            bsd.rmbPrice = EditorGUILayout.IntField("价格", bsd.rmbPrice);

                            break;
                        default:
                            break;
                    }
                }
            }
            GUILayout.EndArea();
            GUI.EndScrollView();
            GUILayout.EndArea();
        }

        protected FloatProperty DrawFloatProperty(FloatProperty property, string label)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel(label);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            Utility.SetLabelWidth(39);
            property.Now = EditorGUILayout.FloatField("当前值", property.Now);
            property.Max = EditorGUILayout.FloatField("最小值", property.Max);
            property.Min = EditorGUILayout.FloatField("最大值", property.Min);
            Utility.SetLabelWidth(52);
            property.autoAdd = EditorGUILayout.FloatField("每秒增加", property.autoAdd);
            Utility.SetLabelWidth(80);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            return property;
        }
    }
}
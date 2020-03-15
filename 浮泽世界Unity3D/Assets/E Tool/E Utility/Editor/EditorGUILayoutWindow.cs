using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class EditorGUILayoutWindow : EditorWindow
{
    //运行数据
    /// <summary>
    /// 编辑器自动布局组件列表
    /// </summary>
    private static EditorGUILayoutWindow instance;
    /// <summary>
    /// 窗口实例
    /// </summary>
    public static EditorGUILayoutWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GetWindow<EditorGUILayoutWindow>();
            }
            return instance;
        }
    }
    
    bool foldoutGroup = true;
    bool foldoutGroup2 = true;
    bool foldout = true;
    bool foldout2 = true;
    bool toggleGroup = true;
    bool toggle = true;
    bool toggle2 = true;

    double delayedDouble = 0;
    float delayedFloat = 0;
    int delayedInt = 0;
    string delayedString = "";

    double rDouble = 0;
    float rFloat = 0;
    int rInt = 0;
    string rString = "";
    string password = "";

    float knob = 0;
    float min = 25;
    float max = 75;
    float slider = 0;

    int layer = 0;
    int mask = 0;
    int popup = 0;
    int intPopup = 0;

    Vector2 vector2;
    Vector3 vector3;
    Vector4 vector4;

    Bounds bounds = new Bounds();
    Color color = Color.white;
    Gradient gradient = new Gradient();
    AnimationCurve animationCurve = new AnimationCurve();
    Sprite Sprite;


    int toolbar;
    int selectionGrid = 3;

    private void OnGUI()
    {
        //PlatformTool editorTool = (PlatformTool)CreateInstance("PlatformTool"); 
        //PlatformTool editorTool2 = (PlatformTool)CreateInstance("PlatformTool");
        //EditorGUILayout.EditorToolbar(editorTool, editorTool2);

        foldoutGroup = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutGroup, "折叠组：延迟属性");
        if (foldoutGroup)
        {
            EditorGUI.indentLevel = 1;

            delayedDouble = EditorGUILayout.DelayedDoubleField("延迟双精度浮点数", delayedDouble);
            delayedFloat = EditorGUILayout.DelayedFloatField("延迟单精度浮点数", delayedFloat);
            delayedInt = EditorGUILayout.DelayedIntField("延迟整数", delayedInt);
            delayedString = EditorGUILayout.DelayedTextField("延迟字符串", delayedString);

            EditorGUI.indentLevel = 0;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        foldoutGroup2 = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutGroup2, "折叠组：实时属性");
        if (foldoutGroup2)
        {
            EditorGUI.indentLevel = 1;

            rDouble = EditorGUILayout.DoubleField("双精度浮点数", rDouble);
            rFloat = EditorGUILayout.FloatField("单精度浮点数", rFloat);
            rInt = EditorGUILayout.IntField("整数", rInt);
            rString = EditorGUILayout.TextField("字符串", rString);
            rString = EditorGUILayout.TextArea(rString);

            EditorGUI.indentLevel = 0;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        foldout = EditorGUILayout.Foldout(foldout, "折叠");
        if (foldout)
        {
            EditorGUI.indentLevel = 1;

            EditorGUILayout.PrefixLabel("前置标签");
            color = EditorGUILayout.ColorField("颜色", color);
            gradient = EditorGUILayout.GradientField("渐变色", gradient);
            animationCurve = EditorGUILayout.CurveField("动画曲线", animationCurve);
            vector2 = EditorGUILayout.Vector2Field("二维向量", vector2);
            vector3 = EditorGUILayout.Vector3Field("二维向量", vector3);
            vector4 = EditorGUILayout.Vector4Field("二维向量", vector4);
            bounds = EditorGUILayout.BoundsField("范围", bounds);
            EditorGUILayout.SelectableLabel("可选标签");
            EditorGUILayout.DropdownButton(new GUIContent("下拉按钮"), FocusType.Keyboard);

            EditorGUI.indentLevel = 0;
        }

        toggleGroup = EditorGUILayout.BeginToggleGroup("切换组", toggleGroup);
        if (toggleGroup)
        {
            EditorGUI.indentLevel = 1;

            EditorGUILayout.HelpBox("消息类型：信息", MessageType.Info);
            EditorGUILayout.HelpBox("消息类型：警告", MessageType.Warning);
            EditorGUILayout.HelpBox("消息类型：错误", MessageType.Error);
            EditorGUILayout.HelpBox("消息类型：无", MessageType.None);
            EditorGUILayout.HelpBox("消息类型：无，覆盖", MessageType.None, true);

            EditorGUI.indentLevel = 0;
        }
        EditorGUILayout.EndToggleGroup();

        foldout2 = EditorGUILayout.InspectorTitlebar(foldout2, this);
        if (foldout2)
        {
            slider = EditorGUILayout.Slider("滑块", slider, 0, 100);
            EditorGUILayout.MinMaxSlider("范围值", ref min, ref max, 0, 100);
            Sprite = (Sprite)EditorGUILayout.ObjectField("对象", Sprite, typeof(Sprite));
            password = EditorGUILayout.PasswordField("密码", password);
            toggle = EditorGUILayout.Toggle("切换", toggle);
            toggle2 = EditorGUILayout.ToggleLeft("切换", toggle2);

            string[] strs = new string[] { "选项一", "选项二", "选项三" };
            int[] ints = new int[] { 0, 1, 2 };
            layer = EditorGUILayout.LayerField("层级", layer);
            mask = EditorGUILayout.MaskField("蒙版", mask, strs);
            popup = EditorGUILayout.Popup("选项框", popup, strs);
            intPopup = EditorGUILayout.IntPopup("整数选项框", intPopup, strs, ints);
            knob = EditorGUILayout.Knob(new Vector2(40, 40), knob, 0, 100, "%", Color.white, Color.yellow, true);

            EditorGUILayout.LabelField("标签");
            EditorGUILayout.LabelField("标签1", "标签2");
        }


        toolbar = GUILayout.Toolbar(toolbar, new string[] { "1", "2", "3" });
        selectionGrid = GUILayout.SelectionGrid(selectionGrid, new string[] { "1", "2", "3" }, 3);
        GUILayout.Box(EditorGUIUtility.IconContent("EditCollider").image);
        GUILayout.Box("Box");
    }

    [MenuItem("Window/E Tool/编辑器自动布局组件列表 %#t")]
    public static void Open()
    {
        Initialize();
        Debug.Log("已打开 编辑器自动布局组件列表");
    }
    //初始化
    /// <summary>
    /// 重置窗口
    /// </summary>
    private static void Initialize()
    {
        Instance.titleContent = new GUIContent("编辑器自动布局组件列表");
        Instance.minSize = new Vector2(450, 400);
    }


    // Tagging a class with the EditorTool attribute and no target type registers a global tool. Global tools are valid for any selection, and are accessible through the top left toolbar in the editor.
    [EditorTool("平台工具")]
    public class PlatformTool : EditorTool
    {
        // Serialize this value to set a default value in the Inspector.
        [SerializeField] private Texture2D toolIcon;
        private GUIContent iconContent;

        public PlatformTool(GUIContent content)
        {
            iconContent = content;
        }

        private void OnEnable()
        {
        }
        public override GUIContent toolbarIcon
        {
            get { return iconContent; }
        }

        // This is called for each window that your tool is active in. Put the functionality of your tool here.
        public override void OnToolGUI(EditorWindow window)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 position = Tools.handlePosition;

            using (new Handles.DrawingScope(Color.green))
            {
                position = Handles.Slider(position, Vector3.right);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Vector3 delta = position - Tools.handlePosition;
                Undo.RecordObjects(Selection.transforms, "Move Platform");

                foreach (var transform in Selection.transforms)
                    transform.position += delta;
            }
        }
    }
}
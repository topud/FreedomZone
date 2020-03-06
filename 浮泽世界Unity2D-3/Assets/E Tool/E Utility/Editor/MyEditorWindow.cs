using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace E.Tool
{
    public abstract class MyEditorWindow<T> : EditorWindow where T : EditorWindow
    {
        /// <summary>
        /// 窗口实例
        /// </summary>
        private static T instance;

        /// <summary>
        /// 窗口实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GetWindow<T>();

                }
                return instance;
            }
        }
        /// <summary>
        /// 上下文菜单
        /// </summary>
        protected GenericMenu Menu { get; set; }
        /// <summary>
        /// 顶部面板
        /// </summary>
        protected virtual Rect Top { get => new Rect(0, 0, position.width, Utility.GetHeightLong(1)); }
        /// <summary>
        /// 中部面板
        /// </summary>
        protected virtual Rect Center { get => new Rect(0, Top.height, position.width, position.height - Top.height); }
        /// <summary>
        /// 底部面板
        /// </summary>
        protected virtual Rect Buttom { get => new Rect(0, position.height - Utility.GetHeightLong(1), position.width, Utility.GetHeightLong(1)); }

        protected abstract void Refresh();
        protected abstract void CheckMenuClick(object obj);
        protected abstract void DrawBG();
        protected abstract void DrawTop();
        protected abstract void DrawCenter();
        protected abstract void DrawButtom();
        protected abstract void DrawMenu();
        protected virtual void DrawLine(Vector2 start, Vector2 end)
        {
            Handles.DrawBezier(start, end, start, end, Color.gray, null, 1);
        }
        protected virtual void DrawLine(Vector2 start, Vector2 end, Color color, float width = 1)
        {
            Handles.DrawBezier(start, end, start, end, color, null, width);
        }
    }
}
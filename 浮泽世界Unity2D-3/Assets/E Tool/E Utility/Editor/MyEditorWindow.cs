using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace E.Tool
{
    public abstract class MyEditorWindow<T> : EditorWindow where T : EditorWindow
    {
        private static T instance;

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
        protected GenericMenu Menu { get; set; }

        protected abstract void Refresh();
        protected abstract void CheckMenuClick(object obj);
        protected abstract void DrawMenu();
        protected abstract void DrawBG();
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
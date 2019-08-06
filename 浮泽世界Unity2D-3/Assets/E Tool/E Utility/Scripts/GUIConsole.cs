using UnityEngine;
using System.Collections.Generic;

namespace E.Tool
{
    class LogEntry
    {
        public string message;
        public LogType type;
        public LogEntry(string message, LogType type)
        {
            this.message = message;
            this.type = type;
        }
    }

    public class GUIConsole : MonoBehaviour
    {
        public int height = 25;
        List<LogEntry> log = new List<LogEntry>();
        Vector2 scroll = Vector2.zero;

#if !UNITY_EDITOR
    void Awake()
    {
        Application.logMessageReceived += OnLog;
    }
#endif

        void OnLog(string message, string stackTrace, LogType type)
        {
            // show everything in debug builds and only errors/exceptions in release
            if (Debug.isDebugBuild || type == LogType.Error || type == LogType.Exception)
            {
                log.Add(new LogEntry(message, type));
                scroll.y = 99999f; // autoscroll
            }
        }

        void OnGUI()
        {
            if (log.Count == 0) return;

            scroll = GUILayout.BeginScrollView(scroll, "Box", GUILayout.Width(Screen.width), GUILayout.Height(height));
            foreach (LogEntry entry in log)
            {
                if (entry.type == LogType.Error || entry.type == LogType.Exception)
                    GUI.color = Color.red;
                else if (entry.type == LogType.Warning)
                    GUI.color = Color.yellow;
                GUILayout.Label(entry.message);
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();
        }
    }
}
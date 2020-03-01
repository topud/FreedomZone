using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Story PlotItem", order = 1)]
    public class Plot : ScriptableObject
    {
        [Tooltip("对话")] public List<PlotItem> sentences;
    }

    [Serializable]
    public class PlotItem
    {
        [Tooltip("角色名称")] public string role;
        [Tooltip("对话内容")] public string words;
        [Tooltip("角色表情")] public Sprite avatar;

        public PlotItem(string role, string words)
        {
            this.role = role;
            this.words = words;
            avatar = null;
        }
    }
}
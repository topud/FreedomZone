using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class StoryNodeOption
    {
        [Tooltip("节点的编号")] public NodeID id;
        [Tooltip("节点的选项描述")] public string description;
        [Tooltip("节点的选项显示条件")] public List<ConditionComparison> conditions = new List<ConditionComparison>();

        public StoryNodeOption(NodeID id)
        {
            this.id = id;
        }
    }
}
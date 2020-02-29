using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class StoryNodeOption
    {
        [Tooltip("此分支节点的编号")] public NodeID id;
        [Tooltip("此分支节点的描述")] public string description;
        [Tooltip("选择此分支节点的条件数值要求")] public List<ConditionComparison> conditions = new List<ConditionComparison>();
        [Tooltip("选择此分支节点后的条件数值变动")] public List<ConditionChange> conditionChanges = new List<ConditionChange>();

        public StoryNodeOption(NodeID id)
        {
            this.id = id;
        }
    }
}
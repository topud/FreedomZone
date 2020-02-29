using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class OptionNode : Node
    {
        [Tooltip("选项编号")] [ReadOnly] public int id;
        [Tooltip("选择此选项的条件数值要求")] public List<ConditionComparison> comparisons = new List<ConditionComparison>();
        [Tooltip("选择此选项后的条件数值变动")] public List<ConditionChange> changes = new List<ConditionChange>();

        public OptionNode(RectInt rect, int id)
        {
            layout = rect;
            description = "选项简介";
            nextPlotNode = new PlotID();

            this.id = id;
            comparisons = new List<ConditionComparison>();
            changes = new List<ConditionChange>();
        }
    }
}
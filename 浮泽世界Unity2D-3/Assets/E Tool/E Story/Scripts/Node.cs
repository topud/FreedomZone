using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public abstract class Node
    {
        [Tooltip("节点布局")] public RectInt layout;
        [Tooltip("节点描述")] public string description;
        [Tooltip("后续剧情节点")] public PlotID nextPlotNode;
    }
}
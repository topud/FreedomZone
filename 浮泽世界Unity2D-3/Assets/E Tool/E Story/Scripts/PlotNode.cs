using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class PlotNode : Node
    {
        [Tooltip("剧情编号")] public PlotID id;
        [Tooltip("剧情分支类型")] public PlotBranchType branchType;
        [Tooltip("剧情时间类型")] public PlotTimeType timeType;
        [Tooltip("剧情内容")] public Plot plot;
        [Tooltip("后续选项节点")] public List<int> nextOptionNodes = new List<int>();

        public PlotNode(Rect rect, PlotID id)
        {
            layout = rect;
            description = "剧情简介";
            nextPlotNode = new PlotID();

            this.id = id;
            timeType = PlotTimeType.过渡;
            branchType = PlotBranchType.主线;
            plot = null;
            nextOptionNodes = new List<int>();
        }

        public bool ContainsOptionNode(OptionNode node)
        {
            foreach (int item in nextOptionNodes)
            {
                if (item == node.id)
                {
                    return true;
                }
            }
            return false;
        }

        public void CheckNull()
        {
            if (nextOptionNodes == null)
            {
                nextOptionNodes = new List<int>();
            }
            else
            {
                nextOptionNodes.RemoveAll(x => x == -1);
            }
        }
    }

    [Serializable]
    public struct PlotID
    {
        [Tooltip("章节号")] public int chapter;
        [Tooltip("场景号")] public int scene;
        [Tooltip("片段号")] public int part;
        [Tooltip("分支号")] public int branch;

        public bool IsRightFormat
        {
            get
            {
                if (chapter <= 0)
                {
                    return false;
                }
                if (scene <= 0)
                {
                    return false;
                }
                if (part <= 0)
                {
                    return false;
                }
                if (branch <= 0)
                {
                    return false;
                }
                return true;
            }
        }

        public PlotID(int c, int s, int p, int b)
        {
            chapter = c;
            scene = s;
            part = p;
            branch = b;
        }

        public override bool Equals(object obj)
        {
            PlotID other = (PlotID)obj;
            if (chapter == other.chapter && scene == other.scene && part == other.part && branch == other.branch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public enum PlotBranchType
    {
        主线,
        支线,
        独立
    }
    public enum PlotTimeType
    {
        过渡,
        开局,
        结局
    }
}
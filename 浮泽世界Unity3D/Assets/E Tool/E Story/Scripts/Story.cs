using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Story", order = 0)]
    public class Story : ScriptableObject
    {
        public string description;
        public List<PlotNode> plotNodes = new List<PlotNode>();
        public List<OptionNode> optionNodes = new List<OptionNode>();
        public List<Condition> conditions = new List<Condition>();
        public bool isZoomOut = false;

        /// <summary>
        /// 条件集合
        /// </summary>
        public string[] ConditionKeys
        {
            get
            {
                int count = conditions.Count;
                string[] keys;
                if (count > 0)
                {
                    keys = new string[count];
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        keys[i] = conditions[i].key;
                    }
                }
                else
                {
                    keys = new string[0];
                }
                return keys;
            }
        }
        /// <summary>
        /// 结局数量
        /// </summary>
        public int EndingCount
        {
            get
            {
                int count = 0;
                foreach (PlotNode item in plotNodes)
                {
                    if (item.timeType == PlotTimeType.结局)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// 创建剧情节点
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public PlotNode CreatePlotNode(Rect rect)
        {
            if (plotNodes == null)
            {
                plotNodes = new List<PlotNode>();
            }

            PlotID id = new PlotID(1,1,1,1);
            while (IsContainID(id))
            {
                id.branch++;
            }
            PlotNode node = new PlotNode(rect, id);
            plotNodes.Add(node);
            return node;
        }
        /// <summary>
        /// 创建选项节点
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public OptionNode CreateOptionNode(Rect rect)
        {
            if (optionNodes == null)
            {
                optionNodes = new List<OptionNode>();
            }

            int id = 0;
            while (ContainsID(id))
            {
                id++;
            }
            OptionNode node = new OptionNode(rect, id);
            optionNodes.Add(node);
            return node;
        }
        /// <summary>
        /// 检查节点是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsContainID(PlotID id)
        {
            foreach (PlotNode item in plotNodes)
            {
                if (item.id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查节点是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsID(int id)
        {
            foreach (OptionNode item in optionNodes)
            {
                if (item.id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        //获取
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PlotNode GetNode(PlotID id)
        {
            foreach (PlotNode item in plotNodes)
            {
                if (item.id.Equals(id))
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OptionNode GetNode(int id)
        {
            foreach (OptionNode item in optionNodes)
            {
                if (item.id.Equals(id))
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取开局节点
        /// </summary>
        public PlotNode GetStartNode()
        {
            foreach (PlotNode item in plotNodes)
            {
                if (item.timeType == PlotTimeType.开局)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取结局节点
        /// </summary>
        public List<PlotNode> GetEndingNodes()
        {
            List<PlotNode> nodes = new List<PlotNode>();
            foreach (PlotNode item in plotNodes)
            {
                if (item.timeType == PlotTimeType.结局)
                {
                    nodes.Add(item);
                }
            }
            return nodes;
        }

        //设置
        /// <summary>
        /// 设置剧情节点编号
        /// </summary>
        /// <param name="newID"></param>
        public void SetPlotID(PlotNode node, PlotID newID)
        {
            if (!newID.IsRightFormat)
            {
                Debug.LogError("剧情节点编号不能是0或负数");
                return;
            }

            if (IsContainID(newID))
            {
                Debug.LogError("此编号的节点已存在 {" + newID.chapter + "-" + newID.scene + "-" + newID.part + "-" + newID.branch + "}");
                return;
            }

            //同步上行节点连接
            foreach (PlotNode item in plotNodes)
            {
                if (item.nextPlotNode.Equals(node.id))
                {
                    item.nextPlotNode = newID;
                }
            }
            foreach (OptionNode item in optionNodes)
            {
                if (item.nextPlotNode.Equals(node.id))
                {
                    item.nextPlotNode = newID;
                }
            }
            node.id = newID;
        }
        /// <summary>
        /// 设置剧情分支类型
        /// </summary>
        public void SetNodeBranchType(PlotNode node, PlotBranchType type)
        {
            if (IsContainID(node.id))
            {
                switch (type)
                {
                    case PlotBranchType.主线:
                        break;
                    case PlotBranchType.支线:
                        break;
                    case PlotBranchType.独立:
                        ClearNodeDownChoices(node);
                        ClearNodeUpChoices(node);
                        break;
                    default:
                        break;
                }
                node.branchType = type;
            }
        }
        /// <summary>
        /// 设置剧情时间类型
        /// </summary>
        public void SetNodeTimeType(PlotNode node, PlotTimeType type)
        {
            if (IsContainID(node.id))
            {
                switch (type)
                {
                    case PlotTimeType.过渡:
                        break;
                    case PlotTimeType.开局:
                        foreach (PlotNode item in plotNodes)
                        {
                            if (item.timeType == PlotTimeType.开局)
                            {
                                item.timeType = PlotTimeType.过渡;
                            }
                        }
                        ClearNodeUpChoices(node);
                        break;
                    case PlotTimeType.结局:
                        ClearNodeDownChoices(node);
                        break;
                    default:
                        break;
                }
                node.timeType = type;
            }
        }

        //移除
        /// <summary>
        /// 移除节点
        /// </summary>
        public void RemoveNode(Node node)
        {
            string str = "确认要删除此节点吗？";
            if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
            {
                if (plotNodes.Contains(node as PlotNode))
                {
                    plotNodes.Remove(node as PlotNode);
                }
                if (optionNodes.Contains(node as OptionNode))
                {
                    optionNodes.Remove(node as OptionNode);
                }
            }
        }
        /// <summary>
        /// 移除剧情节点
        /// </summary>
        public void RemovePlotNodes()
        {
            string str = "确认要清空所有剧情节点吗？";
            if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
            {
                plotNodes.Clear();
            }
        }
        /// <summary>
        /// 移除选项节点
        /// </summary>
        public void RemoveOptionNodes()
        {
            string str = "确认要清空所有选项节点吗？";
            if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
            {
                optionNodes.Clear();
            }
        }
        /// <summary>
        /// 移除所有节点
        /// </summary>
        public void RemoveAllNodes()
        {
            string str = "确认要清空所有节点吗？";
            if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
            {
                plotNodes.Clear();
                optionNodes.Clear();
            }
        }

        //清除
        /// <summary>
        /// 清除节点下行连接
        /// </summary>
        /// <param name="node"></param>
        public void ClearNodeDownChoices(Node node)
        {
            if (node.GetType() == typeof(PlotNode))
            {
                PlotNode pn = node as PlotNode;
                pn.nextOptionNodes.Clear();
                pn.nextPlotNode = new PlotID();
            }
            else
            {
                OptionNode on = node as OptionNode;
                on.nextPlotNode = new PlotID();
            }
        }
        /// <summary>
        /// 清除节点上行连接
        /// </summary>
        /// <param name="node"></param>
        public void ClearNodeUpChoices(Node node)
        {
            if (node.GetType() == typeof(PlotNode))
            {
                PlotNode pn = node as PlotNode;
                foreach (PlotNode item in plotNodes)
                {
                    if (item.nextPlotNode.Equals(pn.id))
                    {
                        item.nextPlotNode = new PlotID();
                    }
                }
                foreach (OptionNode item in optionNodes)
                {
                    if (item.nextPlotNode.Equals(pn.id))
                    {
                        item.nextPlotNode = new PlotID();
                    }
                }
            }
            else
            {
                OptionNode on = node as OptionNode;
                foreach (PlotNode item in plotNodes)
                {
                    if (item.nextOptionNodes != null)
                    {
                        item.nextOptionNodes.RemoveAll(x => x == on.id);
                    }
                }
            }
        }

        //检查
        public void CheckNull()
        {
            if (plotNodes == null)
            {
                plotNodes = new List<PlotNode>();
            }
            else
            {
                plotNodes.RemoveAll(x => x == null);
            }
            if (optionNodes == null)
            {
                optionNodes = new List<OptionNode>();
            }
            else
            {
                optionNodes.RemoveAll(x => x == null);
            }
        }
    }
}
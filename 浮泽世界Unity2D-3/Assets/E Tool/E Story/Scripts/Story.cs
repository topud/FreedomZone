// ========================================================
// 作者：E Star
// 创建时间：2019-02-27 01:18:23
// 当前版本：1.0
// 作用描述：可序列化故事类
// 挂载目标：无
// ========================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Story", order = 0)]
    public class Story : StaticDataDictionary<Story>
    {
        [Tooltip("故事描述"), TextArea(1, 10)] public string Describe;
        public List<StoryNode> Nodes = new List<StoryNode>();
        public List<Condition> Conditions = new List<Condition>();

        public string[] ConditionKeys
        {
            get
            {
                int count = Conditions.Count;
                string[] keys;
                if (count > 0)
                {
                    keys = new string[count];
                    for (int i = 0; i < Conditions.Count; i++)
                    {
                        keys[i] = Conditions[i].Key;
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
        /// 创建节点
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public StoryNode CreateNode(RectInt rect)
        {
            if (Nodes == null)
            {
                Nodes = new List<StoryNode>();
            }

            NodeID id = new NodeID(1,1,1,1);
            while (ContainsID(id))
            {
                id.branch++;
            }
            StoryNode node = new StoryNode(id, rect);
            Nodes.Add(node);
            return node;
        }
        /// <summary>
        /// 检查节点是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsID(NodeID id)
        {
            foreach (StoryNode item in Nodes)
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
        public StoryNode GetNode(NodeID id)
        {
            foreach (StoryNode item in Nodes)
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
        public StoryNode GetStartNode()
        {
            foreach (StoryNode item in Nodes)
            {
                if (item.Type == NodeType.起始节点)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取结局节点
        /// </summary>
        public List<StoryNode> GetEndingNodes()
        {
            List<StoryNode> nodes = new List<StoryNode>();
            foreach (StoryNode item in Nodes)
            {
                if (item.Type == NodeType.结局节点)
                {
                    nodes.Add(item);
                }
            }
            return nodes;
        }

        //设置
        /// <summary>
        /// 设置节点编号
        /// </summary>
        /// <param name="id"></param>
        public void SetNodeID(StoryNode node, NodeID id)
        {
            if (!ContainsID(id))
            {
                //同步上行节点连接
                foreach (StoryNode item in Nodes)
                {
                    if (item.nodeOptions != null)
                    {
                        for (int i = 0; i < item.nodeOptions.Count; i++)
                        {
                            if (item.nodeOptions[i].id.Equals(node.id))
                            {
                                item.nodeOptions[i].id = id;
                            }
                        }
                    }
                }
                node.id = id;
            }
            else
            {
                Debug.LogError("此编号的节点已存在 {" + id.chapter + "-" + id.scene + "-" + id.part + "-" + id.branch + "}");
            }
        }
        /// <summary>
        /// 设置节点类型
        /// </summary>
        public void SetNodeType(StoryNode node, NodeType nodeType)
        {
            if (ContainsID(node.id))
            {
                switch (nodeType)
                {
                    case NodeType.中间节点:
                        break;
                    case NodeType.起始节点:
                        foreach (StoryNode item in Nodes)
                        {
                            if (item.Type == NodeType.起始节点)
                            {
                                item.Type = NodeType.中间节点;
                            }
                        }
                        ClearNodeUpChoices(node);
                        break;
                    case NodeType.结局节点:
                        ClearNodeDownChoices(node);
                        break;
                    default:
                        break;
                }
                node.Type = nodeType;
            }
        }

        //移除
        /// <summary>
        /// 移除节点
        /// </summary>
        public void RemoveNode(StoryNode node)
        {
            if (Nodes.Contains(node))
            {
                string str = "确认要删除此节点吗？";
                if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
                {
                    Nodes.Remove(node);
                }
            }
        }

        //清除
        /// <summary>
        /// 清除节点下行连接
        /// </summary>
        /// <param name="node"></param>
        public void ClearNodeDownChoices(StoryNode node)
        {
            if (node.nodeOptions != null)
            {
                node.nodeOptions.Clear();
            }
        }
        /// <summary>
        /// 清除节点上行连接
        /// </summary>
        /// <param name="node"></param>
        public void ClearNodeUpChoices(StoryNode node)
        {
            foreach (StoryNode item in Nodes)
            {
                if (item.nodeOptions != null)
                {
                    for (int i = 0; i < item.nodeOptions.Count; i++)
                    {
                        if (item.nodeOptions[i].id.Equals(node.id))
                        {
                            item.nodeOptions.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 清空节点
        /// </summary>
        public void ClearNodes()
        {
            string str = "确认要清空所有节点吗？";
            if (EditorUtility.DisplayDialog("警告", str, "确认", "取消"))
            {
                Nodes.Clear();
            }
        }
    }
}
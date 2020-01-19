using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;
using UnityEngine.AddressableAssets;

public class StoryManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private Story currentStory;

    public List<NodeID> CurrentStoryPassdNodeIDs;
    public NodeID CurrentNodeID;
    public StoryNode CurrentNode;

    public Story CurrentStory 
    {
        get 
        {
            return currentStory; 
        }
        set => currentStory = value;
    }
    public List<Condition> Conditions 
    {
        get=> CurrentStory.conditions;
        set => CurrentStory.conditions = value;
    }
    
    /// <summary>
    /// 获取通过的节点
    /// </summary>
    public List<StoryNode> GetPassedNodes()
    {
        List<StoryNode> nodes = new List<StoryNode>();
        foreach (StoryNode item in CurrentStory.nodes)
        {
            foreach (NodeID id in CurrentStoryPassdNodeIDs)
            {
                if (id.Equals(item.id))
                {
                    nodes.Add(item);
                }
            }
        }
        return nodes;
    }
    /// <summary>
    /// 获取通过的结局节点
    /// </summary>
    public List<StoryNode> GetPassedEndingNodes()
    {
        List<StoryNode> nodes = CurrentStory.GetEndingNodes();
        List<StoryNode> nodesP = new List<StoryNode>();
        if (nodes.Count > 0)
        {
            foreach (StoryNode item in nodes)
            {
                foreach (NodeID id in CurrentStoryPassdNodeIDs)
                {
                    if (id.Equals(item.id))
                    {
                        nodesP.Add(item);
                    }
                }
            }
        }
        return nodesP;
    }
    /// <summary>
    /// 获取全节点通过百分比（分数格式）
    /// </summary>
    public string GetAllNodesPassFraction()
    {
        return GetPassedNodes().Count + "/" + CurrentStory.nodes.Count;
    }
    /// <summary>
    /// 获取全结局解锁百分比（分数格式）
    /// </summary>
    public string GetAllEndingNodesPassFraction()
    {
        return GetPassedEndingNodes().Count + "/" + CurrentStory.GetEndingNodes().Count;
    }
    /// <summary>
    /// 获取全节点通过百分比（小数格式）
    /// </summary>
    public float GetAllNodesPassPercentage()
    {
        if (GetPassedNodes().Count == 0)
        {
            return 0;
        }
        return (float)GetPassedNodes().Count / CurrentStory.nodes.Count;
    }
    /// <summary>
    /// 获取全结局解锁百分比（小数格式）
    /// </summary>
    public float GetAllEndingNodesPassPercentage()
    {
        if (CurrentStory.GetEndingNodes().Count == 0)
        {
            return 0;
        }
        return (float)GetPassedEndingNodes().Count / CurrentStory.GetEndingNodes().Count;
    }
}
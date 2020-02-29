using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;
using UnityEngine.AddressableAssets;

public class StoryManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private Story currentStory;

    public List<PlotID> CurrentStoryPassdNodeIDs;
    public PlotID CurrentNodeID;
    public PlotNode CurrentNode;

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
    public List<PlotNode> GetPassedNodes()
    {
        List<PlotNode> nodes = new List<PlotNode>();
        foreach (PlotNode item in CurrentStory.plotNodes)
        {
            foreach (PlotID id in CurrentStoryPassdNodeIDs)
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
    public List<PlotNode> GetPassedEndingNodes()
    {
        List<PlotNode> nodes = CurrentStory.GetEndingNodes();
        List<PlotNode> nodesP = new List<PlotNode>();
        if (nodes.Count > 0)
        {
            foreach (PlotNode item in nodes)
            {
                foreach (PlotID id in CurrentStoryPassdNodeIDs)
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
        return GetPassedNodes().Count + "/" + CurrentStory.plotNodes.Count;
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
        return (float)GetPassedNodes().Count / CurrentStory.plotNodes.Count;
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
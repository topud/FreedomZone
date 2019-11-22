using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class StoryNode
    {
        [ReadOnly] public NodeID ID;
        [ReadOnly] public NodeType Type;
        public bool IsMainNode;
        //[Tooltip("节点内容")] public StoryContent Content;

        [Tooltip("发生时间")] public DateTime Time;
        [Tooltip("发生地点")] public string Position;
        [Tooltip("摘要"), TextArea] public string Summary;
        [Tooltip("内容形式")] public ContentType ContentType;
        [Tooltip("剧情对话")] public List<Sentence> Sentences = new List<Sentence>();
        [Tooltip("过场动画")] public List<Animation> Animations = new List<Animation>();

        [Tooltip("节点分支选项")] public List<NextNode> NextNodes = new List<NextNode>();

        [Tooltip("节点布局")] public RectInt Rect;
        [Tooltip("节点展开")] public bool IsFold;

        public StoryNode(NodeID id, RectInt rect)
        {
            ID = id;
            Type = NodeType.中间节点;
            IsMainNode = true;
            //Content = null;
            NextNodes = new List<NextNode>();
            Rect = rect;
            IsFold = false;


            Time = new DateTime(2000, 1, 1, 0, 0, 0);
            Position = "";
            Summary = "";
            ContentType = ContentType.剧情对话;
            Sentences = new List<Sentence>();
            Animations = new List<Animation>();
        }
        public bool ContainsNextNode(NodeID id)
        {
            foreach (NextNode item in NextNodes)
            {
                if (item.ID.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class NextNode
    {
        [Tooltip("节点的编号")] public NodeID ID;
        [Tooltip("节点的选项描述")] public string Describe;
        [Tooltip("节点的选项显示条件")] public List<ConditionComparison> Conditions = new List<ConditionComparison>();

        public NextNode(string text, NodeID id)
        {
            Describe = text;
            ID = id;
        }
    }

    [Serializable]
    public struct NodeID
    {
        [Tooltip("章节号")] public int Chapter;
        [Tooltip("场景号")] public int Scene;
        [Tooltip("片段号")] public int Part;
        [Tooltip("分支号")] public int Branch;

        public NodeID(int c, int s, int p, int b)
        {
            Chapter = c;
            Scene = s;
            Part = p;
            Branch = b;
        }

        public override bool Equals(object obj)
        {
            NodeID other = (NodeID)obj;
            if (Chapter == other.Chapter && Scene == other.Scene && Part == other.Part && Branch == other.Branch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Serializable]
    public struct Condition
    {
        [Tooltip("条件名")] public string Key;
        [Tooltip("默认值"), Range(-100, 100)] public int DefaultValue;
    }
    [Serializable]
    public struct ConditionComparison
    {
        [Tooltip("条件名")] public int KeyIndex;
        [Tooltip("对比方式")] public Comparison Comparison;
        [Tooltip("目标值"), Range(-100, 100)] public int Value;

        public void SetIndex(int index)
        {
            KeyIndex = index;
        }
        public void SetComparison(Comparison comparison)
        {
            Comparison = comparison;
        }
        public void SetValue(int value)
        {
            Value = value;
        }
    }

    public enum Comparison
    {
        大于,
        大于等于,
        小于,
        小于等于,
        等于,
        不等于
    }

    [Serializable]
    public struct Sentence
    {
        [Tooltip("角色名称")] public string Speaker;
        [Tooltip("角色表情")] public Sprite Expression;
        [Tooltip("角色说话内容"), TextArea(1, 10)] public string Words;

        [Tooltip("节点展开")] public bool IsFold;

        public Sentence(string speaker, string words)
        {
            Speaker = speaker;
            Expression = null;
            Words = words;
            IsFold = true;
        }
    }

    public enum NodeType
    {
        中间节点,
        起始节点,
        结局节点
    }
    public enum ContentType
    {
        剧情对话,
        过场动画
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class StoryNode
    {
        [ReadOnly] public NodeID id;
        [ReadOnly] public NodeType Type;
        public bool isMainNode;

        [Tooltip("简介"), TextArea] public string description;
        [Tooltip("对话")] public List<Sentence> sentences = new List<Sentence>();
        [Tooltip("选项")] public List<StoryNodeOption> nodeOptions = new List<StoryNodeOption>();

        [Tooltip("布局")] public RectInt layout;

        public StoryNode(NodeID id, RectInt rect)
        {
            this.id = id;
            Type = NodeType.中间节点;
            isMainNode = true;
            nodeOptions = new List<StoryNodeOption>();
            layout = rect;

            description = "";
            sentences = new List<Sentence>();
        }

        public bool ContainsNextNode(NodeID id)
        {
            foreach (StoryNodeOption item in nodeOptions)
            {
                if (item.id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public struct NodeID
    {
        [Tooltip("章节号")] public int chapter;
        [Tooltip("场景号")] public int scene;
        [Tooltip("片段号")] public int part;
        [Tooltip("分支号")] public int branch;

        public NodeID(int c, int s, int p, int b)
        {
            chapter = c;
            scene = s;
            part = p;
            branch = b;
        }

        public override bool Equals(object obj)
        {
            NodeID other = (NodeID)obj;
            if (chapter == other.chapter && scene == other.scene && part == other.part && branch == other.branch)
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
}
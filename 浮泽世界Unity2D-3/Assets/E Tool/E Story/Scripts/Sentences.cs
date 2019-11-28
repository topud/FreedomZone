using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Story Sentences", order = 1)]
    public class Sentences : StaticDataDictionary<Sentences>
    {
        [Tooltip("对话")] public Sentence[] sentences;
    }

    [Serializable]
    public class Sentence
    {
        [Tooltip("角色名称")] public string character;
        [Tooltip("角色表情")] public Sprite avatar;
        [Tooltip("对话内容"), TextArea(1, 10)] public string words;

        public Sentence(string speaker, string words)
        {
            character = speaker;
            avatar = null;
            this.words = words;
        }
    }
}
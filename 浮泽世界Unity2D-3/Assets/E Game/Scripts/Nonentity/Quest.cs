using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public partial struct Quest
    {
        public int Hash;
        public int progress;
        public bool completed;

        // constructors
        public Quest(QuestStaticData data)
        {
            Hash = data.name.GetStableHashCode();
            progress = 0;
            completed = false;
        }

        // wrappers for easier access
        public QuestStaticData data
        {
            get
            {
                if (!QuestStaticData.Dictionary.ContainsKey(Hash))
                    throw new KeyNotFoundException("哈希值{" + Hash + "}找不到对应ScriptableQuest，确保所有ScriptableQuest都在Resources文件夹中，以便正确加载它们。");
                return (QuestStaticData)QuestStaticData.Dictionary[Hash];
            }
        }
        public string name => data.name;
        public int requiredLevel => data.requiredLevel;
        public string predecessor => data.predecessor != null ? data.predecessor.name : "";
        public long rewardGold => data.rewardGold;
        public long rewardExperience => data.rewardExperience;
        public InteractorStaticData rewardItem => data.rewardItem;

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Quest")]
    public class Quest : StaticData
    {
        [Tooltip("任务类型")] public QuestType type;

        [Header("目标（根据任务类型三选一）")]
        [Tooltip("目标实体")] public EntityStaticData tartgetEntity;
        [Tooltip("目标技能")] public Skill tartgetSkill;
        [Tooltip("目标数量"), Range(0, 100)] public int tartgetCount = 1;

        [Tooltip("奖励软妹币")] public int rewardRMB = 0;
        [Tooltip("奖励浮泽币")] public int rewardFZB = 0;
        [Tooltip("奖励技能")] public Skill rewardSkill;
        [Tooltip("奖励物品")] public List<ItemStaticData> rewardItems = new List<ItemStaticData>();

        [Tooltip("要求完成的前一项任务")] public Quest requireQuest;
        [Tooltip("要求掌握的技能")] public Skill requireSkill;

        [Tooltip("完成数量"), ReadOnly] public int finishCount = 0;
        
        /// <summary>
        /// 任务进度
        /// </summary>
        public float Progress
        {
            get
            {
                return (float)finishCount / tartgetCount;
            }
        }
        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool IsFinished
        {
            get => Progress >= 0 ? true : false;
            set
            {
                if (value == true) finishCount = tartgetCount;
                else finishCount = 0;
            }
        }
    }

    public enum QuestType
    {
        调查对象,

        收集物品,
        制作物品,
        使用物品,
        破坏物品,
        
        对话角色,
        击杀角色,

        前往地点,

        学习技能,
    }
}
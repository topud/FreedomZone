using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace E.Tool
{
    [CreateAssetMenu(menuName = "E Quest")]
    public class Quest : StaticDataDictionary<Quest>
    {
        [Header("任务信息")]
        [SerializeField, Tooltip("任务标题")] private string title;
        [SerializeField, Tooltip("任务细节")] private string detail;
        [SerializeField, Tooltip("任务类型")] private QuestType questType;

        [Header("任务目标（根据任务类型三选一）")]
        [SerializeField, Tooltip("目标角色")] private CharacterStaticData tartgetCharacter;
        [SerializeField, Tooltip("目标物品")] private InteractorStaticData tartgetInteractor;
        [SerializeField, Tooltip("目标技能")] private Skill tartgetSkill;

        [Header("任务完成情况")]
        [SerializeField, Tooltip("目标数量"), Range(0, 100)] private int tartgetCount = 1;
        [SerializeField, Tooltip("完成数量"), ReadOnly] private int finishCount = 0;

        [Header("任务奖励")]
        [SerializeField, Tooltip("奖励软妹币")] private int rewardRMB = 0;
        [SerializeField, Tooltip("奖励浮泽币")] private int rewardFZB = 0;
        [SerializeField, Tooltip("奖励技能")] private Skill rewardSkill;
        [SerializeField, Tooltip("奖励物品")] private List<InteractorStaticData> rewardItems = new List<InteractorStaticData>();

        [Header("前提要求")]
        [SerializeField, Tooltip("要求完成的前一项任务")] private Quest requireQuest;
        [SerializeField, Tooltip("要求掌握的技能")] private Skill requireSkill;

        /// <summary>
        /// 任务标题
        /// </summary>
        public string Title { get => title; }
        /// <summary>
        /// 任务细节
        /// </summary>
        public string Detail { get => detail; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public QuestType QuestType { get => questType; }

        /// <summary>
        /// 目标角色
        /// </summary>
        public CharacterStaticData TartgetCharacter { get => tartgetCharacter; set => tartgetCharacter = value; }
        /// <summary>
        /// 目标物品
        /// </summary>
        public InteractorStaticData TartgetInteractor { get => tartgetInteractor; set => tartgetInteractor = value; }
        /// <summary>
        /// 目标技能
        /// </summary>
        public Skill TartgetSkill { get => tartgetSkill; set => tartgetSkill = value; }

        /// <summary>
        /// 目标数量
        /// </summary>
        public int TartgetCount
        {
            get => tartgetCount;
            private set => tartgetCount = Utility.ClampMin(value, 1);
        }
        /// <summary>
        /// 完成数量
        /// </summary>
        public int FinishCount
        {
            get => finishCount;
            set => finishCount = Utility.Clamp(value, 0, tartgetCount);
        }
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

        /// <summary>
        /// 奖励软妹币
        /// </summary>
        public int RewardRMB { get => rewardRMB; }
        /// <summary>
        /// 奖励浮泽币
        /// </summary>
        public int RewardFZB { get => rewardFZB; }
        /// <summary>
        /// 奖励技能
        /// </summary>
        public Skill RewardSkill { get => rewardSkill; set => rewardSkill = value; }
        /// <summary>
        /// 奖励物品
        /// </summary>
        public List<InteractorStaticData> RewardItems { get => rewardItems; }

        /// <summary>
        /// 要求完成的前一项任务
        /// </summary>
        public Quest RequireQuest { get => requireQuest; }
        /// <summary>
        /// 要求掌握的技能
        /// </summary>
        public Skill RequireSkill { get => requireSkill; }
    }

    public enum QuestType
    {
        调查物品,
        收集物品,
        制作物品,
        使用物品,
        破坏物品,

        调查角色,
        对话角色,
        击杀角色,

        前往地点,

        学习技能,
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using E.Utility;

public abstract class ScriptableQuest : StaticData
{
    [Header("通用")]
    [SerializeField, TextArea(1, 30)] protected string toolTip; // not public, use ToolTip()

    [Header("要求")]
    public int requiredLevel; // player.level
    public ScriptableQuest predecessor; // this quest has to be completed first

    [Header("奖励")]
    public long rewardGold;
    public long rewardExperience;
    public InteractorStaticData rewardItem;
}

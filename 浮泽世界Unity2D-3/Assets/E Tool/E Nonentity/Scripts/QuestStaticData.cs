using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

[CreateAssetMenu(menuName = "E Quest")]
public class QuestStaticData : StaticData
{
    [Header("通用")]
    [SerializeField, TextArea(1, 30)] protected string toolTip; // not public, use ToolTip()

    [Header("要求")]
    public int requiredLevel; // player.level
    public QuestStaticData predecessor; // this quest has to be completed first

    [Header("奖励")]
    public long rewardGold;
    public long rewardExperience;
    public InteractorStaticData rewardItem;
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class EntityManager : SingletonClass<EntityManager>
{
    [Header("组件")]
    public GameObject HumanPrefab;
    public Transform CharacterCollection { get => GameObject.Find("Entities").transform.GetChild(1); }
    public Transform ItemCollection { get => GameObject.Find("Entities").transform.GetChild(0); }

    [Header("数据")]
    [ReadOnly] public List<Character> Characters = new List<Character>();
    [ReadOnly] public List<Item> Items = new List<Item>();

    /// <summary>
    /// 检查场景内的实体
    /// </summary>
    public void CheckSceneEntity()
    {
        Characters.Clear();
        Character[] characters = CharacterCollection.GetComponentsInChildren<Character>();
        foreach (Character item in characters)
        {
            Characters.Add(item);
        }
        Debug.Log("场景内NPC数量 " + Characters.Count);
        
        Items.Clear();
        Item[] interactors = ItemCollection.GetComponentsInChildren<Item>();
        foreach (Item item in interactors)
        {
            Items.Add(item);
        }
        Debug.Log("场景内物品数量 " + Items.Count);
    }

    /// <summary>
    /// 生成角色，从静态数据
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Character SpawnCharacter(CharacterStaticData sData, Vector2 position, bool isPlayer = false)
    {
        GameObject go;
        Character character;
        if (sData.Prefab)
        {
            go = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), CharacterCollection);
            character = go.GetComponent<Character>();
        }
        else
        {
            go = Instantiate(HumanPrefab, position, new Quaternion(0, 0, 0, 0), CharacterCollection);
            character = go.GetComponent<Character>();
            character.StaticData = sData;
            character.ResetComponents();
            character.ResetDynamicData();
        }
        character.IsPlayer = isPlayer;
        Characters.Add(character);
        return character;
    }
    /// <summary>
    /// 生成角色，从动态数据（如存档）
    /// </summary>
    public Character SpawnCharacter(CharacterDynamicData dData, bool isPlayer = false)
    {
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(dData.Name);
        Character character = SpawnCharacter(sData, dData.Position, isPlayer);
        return character;
    }
    /// <summary>
    /// 生成Interactor
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Item SpawnInteractor(ItemStaticData sData, Vector2 position)
    {
        GameObject target;
        Item inte;
        target = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), ItemCollection);
        inte = target.GetComponent<Item>();
        Items.Add(inte);
        return inte;
    }
    /// <summary>
    /// 生成Interactor
    /// </summary>
    public Item SpawnInteractor(ItemDynamicData dData)
    {
        ItemStaticData sData = (ItemStaticData)ItemStaticData.GetValue(dData.Name);
        return SpawnInteractor(sData, dData.Position);
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <returns></returns>
    public Character GetCharacter(string name)
    {
        foreach (Character item in Characters)
        {
            if (item.StaticData.Name == name)
            {
                return item;
            }
        }
        return null;
    }
    /// <summary>
    /// 获取物品
    /// </summary>
    /// <param name="name">物品名</param>
    /// <returns></returns>
    public Item GetInteractor(string name)
    {
        foreach (Item item in Items)
        {
            if (item.StaticData.Name == name)
            {
                return item;
            }
        }
        return null;
    }
}

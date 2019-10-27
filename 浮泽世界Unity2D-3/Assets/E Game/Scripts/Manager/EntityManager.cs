using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class EntityManager : SingletonClass<EntityManager>
{
    [Header("组件")]
    public GameObject HumanPrefab;
    public Transform CharacterCollection { get => GameObject.Find("Characters").transform; }
    public Transform ItemCollection { get => GameObject.Find("Items").transform; }

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
        Debug.Log("场景内角色数量 " + Characters.Count);
        
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
    public Character SpawnCharacter(string name, Vector2 position, bool isPlayer = false)
    {
        GameObject go;
        Character character;
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(name);
        if (sData)
        {
            if (sData.Prefab)
            {
                go = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), CharacterCollection);
                character = go.GetComponent<Character>();
                character.ResetDynamicData();
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
            Debug.Log("角色生成成功：" + name);
            return character;
        }
        else
        {
            Debug.LogError("静态数据不存在：" + name);
            return null;
        }
    }
    /// <summary>
    /// 生成角色，从动态数据（如存档）
    /// </summary>
    public Character SpawnCharacter(CharacterDynamicData dData, bool isPlayer = false)
    {
        Character character = SpawnCharacter(dData.Name, dData.Position, isPlayer);
        return character;
    }
    /// <summary>
    /// 生成物品
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Item SpawnItem(string name, Vector2 position)
    {
        GameObject go;
        Item item;
        ItemStaticData sData = (ItemStaticData)ItemStaticData.GetValue(name);
        if (sData)
        {
            go = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), ItemCollection);
            item = go.GetComponent<Item>();
            item.ResetDynamicData();
            Items.Add(item);
            Debug.Log("物品生成成功：" + name);
            return item;
        }
        else
        {
            Debug.LogError("静态数据不存在：" + name);
            return null;
        }
    }
    /// <summary>
    /// 生成物品
    /// </summary>
    public Item SpawnItem(ItemDynamicData dData)
    {
        return SpawnItem(dData.Name, dData.Position);
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
    public Item GetItem(string name)
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

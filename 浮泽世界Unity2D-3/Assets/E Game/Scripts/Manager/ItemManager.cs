using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class ItemManager : SingletonClass<ItemManager>
{
    [Header("组件")]
    public GameObject ItemPrefab;

    [Header("数据")]
    [ReadOnly] public List<Item> Items = new List<Item>();

    /// <summary>
    /// 检查场景内的实体
    /// </summary>
    public void CheckSceneItems()
    {
        
        Items.Clear();
        Item[] interactors = transform.GetComponentsInChildren<Item>();
        foreach (Item item in interactors)
        {
            Items.Add(item);
        }
        Debug.Log("场景内物品数量 " + Items.Count);
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
            go = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), transform);
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

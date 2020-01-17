﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class ItemManager : MonoBehaviour
{
    [Header("组件")]
    public GameObject ItemPrefab;

    [Header("数据")]
    [SerializeField, ReadOnly] private List<Item> items = new List<Item>();
    public List<Item> Items
    {
        get
        {
            items.Clear();
            Item[] interactors = transform.GetComponentsInChildren<Item>();
            foreach (Item item in interactors)
            {
                items.Add(item);
            }
            return items;
        }
    }

    private void OnEnable()
    {
        items = Items;
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
        GameObject go;
        Item item;
        ItemStaticData sData = (ItemStaticData)ItemStaticData.GetValue(dData.Name);
        if (sData)
        {
            go = Instantiate(sData.Prefab, dData.position, new Quaternion(0, 0, 0, 0), transform);
            item = go.GetComponent<Item>();
            item.SetDynamicData(dData);
            Debug.Log("物品生成成功：" + dData.Name);
            return item;
        }
        else
        {
            Debug.LogError("静态数据不存在：" + dData.Name);
            return null;
        }
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
    public Item GetItem(int id)
    {
        foreach (Item item in Items)
        {
            if (item.gameObject.GetInstanceID() == id)
            {
                return item;
            }
        }
        return null;
    }
}

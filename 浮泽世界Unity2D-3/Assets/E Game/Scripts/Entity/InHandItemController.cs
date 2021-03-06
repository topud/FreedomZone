﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

[RequireComponent(typeof(CopyPosition))]
[RequireComponent(typeof(CopyRendererOrder))]
[RequireComponent(typeof(LookAt))]
public class InHandItemController : MonoBehaviour
{
    private Item item;
    private CopyPosition copyPosition;
    private CopyRendererOrder copyRendererOrder;
    private LookAt lookAt;

    public Item Item { get => item; private set => item = value; }

    private void Awake()
    {
        copyPosition = GetComponent<CopyPosition>();
        copyRendererOrder = GetComponent<CopyRendererOrder>();
        lookAt = GetComponent<LookAt>();
    }

    public void SetIsLookAtCursor(bool isLookAtCursor)
    {
        lookAt.enabled = isLookAtCursor;
    }
    public void SetItem(Item item, bool isLookAtCursor)
    {
        Item = item;
        Item.SetCollider(true);
        Item.gameObject.SetActive(true);

        copyPosition.self = Item.transform;
        copyRendererOrder.self = Item.SpriteSorter.Renderers[0];
        lookAt.self = Item.transform;
        lookAt.enabled = isLookAtCursor;

        copyPosition.Update();
        lookAt.Update();
    }
    public void RemoveItem(bool active)
    {
        if (Item)
        {
            Item.gameObject.SetActive(active);
            Item.SetCollider(false);

            copyPosition.self = null;
            copyRendererOrder.self = null;
            lookAt.self = null;
            lookAt.enabled = false;

            Item = null;
        }
    }
}

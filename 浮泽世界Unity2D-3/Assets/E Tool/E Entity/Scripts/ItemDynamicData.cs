﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    [Serializable]
    public class ItemDynamicData : EntityDynamicData
    {
        [Tooltip("是否正在使用")] public bool isUsing = false;
    }
}
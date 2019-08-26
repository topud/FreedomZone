using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public abstract class UISlotBase<T> : MonoBehaviour
    {
        [Header("数据")]
        [SerializeField] private T data;
        public T Data { get => data; protected set => data = value; }

        public virtual void SetData(T data)
        {
            Data = data;
            UpdateData();
        }
        public abstract void UpdateData();
    }
}

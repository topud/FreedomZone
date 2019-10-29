using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public abstract class UISlotBase<T> : MonoBehaviour
    {
        [Header("数据")]
        [SerializeField, ReadOnly] private T data;
        public T Data { get => data; protected set => data = value; }

        public virtual void SetData(T data)
        {
            Data = data;
            UpdateData();
        }
        public abstract void UpdateData();

        public virtual void OnBeginDrag()
        {

        }
        public virtual void OnDrag()
        {

        }
        public virtual void OnEndDrag()
        {

        }
        public virtual void OnDrop()
        {

        }
    }
}

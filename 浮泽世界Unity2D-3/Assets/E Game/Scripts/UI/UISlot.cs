using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace E.Tool
{
    public abstract class UISlot<D> : MonoBehaviour
    {
        [Header("槽 数据")]
        [SerializeField, ReadOnly] private D data;
        public D Data { get => data; protected set => data = value; }

        public virtual void SetData(D data)
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

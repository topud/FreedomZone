using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class UIListBase<D, S> : UIBase where S : UISlotBase<D>
    {
        [Header("组件")]
        [SerializeField] protected Transform tsfParent;
        [SerializeField] protected S pfbSlot;

        [Header("数据")]
        public List<D> Datas = new List<D>();

        private void Start()
        {
            pfbSlot.gameObject.SetActive(false);
            Refresh();
        }

        public virtual void Clear()
        {
            Datas.Clear();
            for (int i = 0; i < tsfParent.childCount; i++)
            {
                Transform transform = tsfParent.GetChild(i);
                if (transform == pfbSlot.transform)
                {
                    continue;
                }
                else
                {
                    Destroy(transform);
                }
            }
        }
        public abstract void LoadData();
        public virtual void SetPanel()
        {
            if (Datas == null) return;
            foreach (D item in Datas)
            {
                GameObject go = Instantiate(pfbSlot.gameObject, tsfParent);
                S slot = go.GetComponent<S>();
                slot.SetData(item);
            }
        }

        public virtual void Refresh()
        {
            Clear();
            LoadData();
            SetPanel();
        }
    }
}
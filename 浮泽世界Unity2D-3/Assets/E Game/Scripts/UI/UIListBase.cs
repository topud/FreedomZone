using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public abstract class UIListBase<D, S> : UIBase where S : UISlotBase<D>
    {
        [SerializeField] protected Transform content;
        [SerializeField] protected S slotPrefab;
        public List<D> Datas;

        private void Start()
        {
            slotPrefab.gameObject.SetActive(false);
            Refresh();
        }
        public virtual void Clear()
        {
            Datas.Clear();
            for (int i = 0; i < content.childCount; i++)
            {
                Transform transform = content.GetChild(i);
                if (transform == slotPrefab.transform)
                {
                    continue;
                }
                else
                {
                    Destroy(transform);
                }
            }
        }
        public virtual void LoadData()
        {
            Datas = new List<D>();
        }
        public virtual void SetPanel()
        {
            if (Datas == null) return;
            foreach (D item in Datas)
            {
                GameObject go = Instantiate(slotPrefab.gameObject, content);
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
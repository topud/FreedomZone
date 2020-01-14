using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace E.Tool
{
    public abstract class UIList<D, S> : UIBase where S : UISlot<D>
    {
        [Header("组件")]
        [SerializeField] protected Transform tsfParent;
        [SerializeField] protected S pfbSlot;

        [Header("数据")]
        public S selectedSlot;
        public List<D> datas = new List<D>();
        public S[] Slots { get => GetComponentsInChildren<S>(); }

        protected virtual void Start()
        {
            pfbSlot.gameObject.SetActive(false);
        }
        protected virtual void OnEnable()
        {
            Refresh();
        }
        protected virtual void OnDisable()
        {
        }

        public virtual void Clear()
        {
            datas.Clear();
            for (int i = 0; i < tsfParent.childCount; i++)
            {
                tsfParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        public abstract void LoadData();
        public virtual void SetPanel()
        {
            if (datas == null) return;
            foreach (D item in datas)
            {
                S slot = GetAvailableSlot();
                slot.SetData(item);
            }
        }
        public S GetAvailableSlot()
        {
            for (int i = 0; i < tsfParent.childCount; i++)
            {
                if (tsfParent.GetChild(i).gameObject.activeInHierarchy) continue;
                S slot = tsfParent.GetChild(i).GetComponent<S>();
                if (slot)
                {
                    slot.gameObject.SetActive(true);
                    return slot;
                }
                else
                {
                    continue;
                }
            }
            GameObject go = Instantiate(pfbSlot.gameObject, tsfParent);
            go.SetActive(true);
            return go.GetComponent<S>();
        }
        public virtual void Refresh()
        {
            Clear();
            LoadData();
            SetPanel();
        }

        public override void Show()
        {
            base.Show();
            Refresh();
        }
    }
}
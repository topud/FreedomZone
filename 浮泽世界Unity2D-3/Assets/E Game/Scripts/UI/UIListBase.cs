using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace E.Tool
{
    public abstract class UIListBase<D, S> : UIBase where S : UISlotBase<D>
    {
        [Header("组件")]
        [SerializeField] protected Transform tsfParent;
        [SerializeField] protected S pfbSlot;

        [Header("数据")]
        [SerializeField] protected S selectedSlot;
        public List<D> Datas = new List<D>();

        public S SelectedSlot
        {
            get => selectedSlot;
            set
            {
                selectedSlot = value;
                Refresh();
            }
        }
        public int SelectedSlotID
        {
            get
            {
                if (!SelectedSlot) return -1;
                for (int i = 0; i < Slots.Count; i++)
                {
                    if (SelectedSlot == Slots[i]) return i;
                }
                return -1;
            }
        }
        public List<S> Slots
        {
            get
            {
                List<S> slots = new List<S>();
                for (int i = 0; i < tsfParent.childCount; i++)
                {
                    if (!tsfParent.GetChild(i).gameObject.activeInHierarchy) continue;
                    S slot = tsfParent.GetChild(i).GetComponent<S>();
                    if (slot) slots.Add(slot);
                }
                return slots;
            }
        }


        protected virtual void Start()
        {
            pfbSlot.gameObject.SetActive(false);
        }
        protected virtual void OnEnable()
        {
            Refresh();

            Character.OnPlayerItemChange.AddListener(Refresh);
        }
        protected virtual void OnDisable()
        {
            Character.OnPlayerItemChange.RemoveListener(Refresh);
        }

        public virtual void Clear()
        {
            Datas.Clear();
            for (int i = 0; i < tsfParent.childCount; i++)
            {
                tsfParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        public abstract void LoadData();
        public virtual void SetPanel()
        {
            if (Datas == null) return;
            foreach (D item in Datas)
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

        public void SelectedLastSlot()
        {
            if (Slots.Count > 0)
            {
                if (SelectedSlot)
                {
                    int id = SelectedSlotID - 1;
                    if (id < 0) id = Slots.Count - 1;
                    SelectedSlot = Slots[id];
                }
                else
                {
                    SelectedSlot = Slots[Slots.Count - 1];
                }
            }
            else
            {
                Debug.Log("没有槽了");
            }
        }
        public void SelectedNextSlot()
        {
            if (Slots.Count > 0)
            {
                if (SelectedSlot)
                {
                    int id = SelectedSlotID + 1;
                    if (id >= Slots.Count) id = 0;
                    SelectedSlot = Slots[id];
                }
                else
                {
                    SelectedSlot = Slots[0];
                }
            }
            else
            {
                Debug.Log("没有槽了");
            }
        }
    }
}
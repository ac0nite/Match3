using Common;
using Match3.General;
using UnityEngine;

namespace Match3.Pool
{
    public class SlotPool : BasePool<Slot>
    {
        private readonly Slot _prefab;
        private readonly Transform _parent;
        private Slot _cashSlot;

        public SlotPool(Slot prefab, int capacity) : base(capacity)
        {
            _prefab = prefab;
            _parent = new GameObject($"{prefab.name}Pool").transform;
            
            Initialise();
        }

        public override Slot Create()
        {
            _cashSlot = UnityEngine.GameObject.Instantiate(_prefab, _parent);
            _cashSlot.SetActive(false);
            return _cashSlot;
        }

        public override Slot Configure(Slot item)
        {
            return item;
        }

        public override void Put(Slot item)
        {
            item.SetActive(false);
            base.Put(item);
        }
    }
}
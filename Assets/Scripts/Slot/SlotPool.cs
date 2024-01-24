using Common;
using Match3.Context;
using Match3.General;
using UnityEngine;

namespace Match3.Pool
{
    public class SlotPool : BasePool<Slot>
    {
        private readonly Slot _prefab;
        private readonly Transform _parent;
        private Slot _cashSlot;

        public SlotPool(ApplicationContext _, Slot prefab, int capacity) : base(capacity)
        {
            _prefab = prefab;
            _parent = new GameObject($"{prefab.name}Pool").transform;

            Initialise();
        }

        public override Slot Create()
        {
            _cashSlot = UnityEngine.Object.Instantiate(_prefab, _parent);
            return _cashSlot;
        }

        public override Slot Configure(Slot slot)
        {
            return slot;
        }

        public override void Put(Slot item)
        {
            item.Clear();
            base.Put(item);
        }
    }
}
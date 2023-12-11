using UnityEngine;

namespace Common
{
    public interface ISlotGenerator
    {
        public IGridSlot[] Allocate(int capacity);
    }
    
    public class SlotGenerator : BasePool<IGridSlot>, ISlotGenerator
    {
        private readonly GridSlot _prefab;
        private readonly GameObject _parent;

        public SlotGenerator(GridSlot prefab, int capacity) : base(capacity)
        {
            _prefab = prefab;
            _parent = new GameObject("GridSlotPool");
        }

        public override IGridSlot Create()
        {
            return Object.Instantiate(_prefab, _parent.transform).SetActive(false);
        }

        public override IGridSlot Configure(IGridSlot item)
        {
            return item;
        }

        public IGridSlot[] Allocate(int capacity)
        {
            IGridSlot[] slots = new IGridSlot[capacity];
            for (int i = 0; i < capacity; i++)
                slots[i] = Get();

            return slots;
        }
    }
}
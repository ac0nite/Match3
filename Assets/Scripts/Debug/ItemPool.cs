using UnityEngine;

namespace Common.Debug
{
    public class ItemPool : BasePool<Item>
    {
        private readonly Item _prefab;
        private readonly Transform _parent;
        private Item _cashItem;

        public ItemPool(Item prefab, int capacity) : base(capacity)
        {
            _prefab = prefab;
            _parent = new GameObject($"{prefab.name}Pool").transform;
            
            Initialise();
        }

        public override Item Create()
        {
            _cashItem = UnityEngine.GameObject.Instantiate(_prefab, _parent);
            _cashItem.SetActive(false);
            return _cashItem;
        }

        public override Item Configure(Item item)
        {
            return item;
        }
    }
}
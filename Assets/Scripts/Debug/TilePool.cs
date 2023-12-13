using UnityEngine;

namespace Common.Debug
{
    public class TilePool : BasePool<Tile>
    {
        private readonly Tile _prefab;
        private readonly Transform _parent;
        private Tile _cashTile;

        public TilePool(Tile prefab, int capacity) : base(capacity)
        {
            _prefab = prefab;
            _parent = new GameObject($"{prefab.name}Pool").transform;
            
            Initialise();
        }

        public override Tile Create()
        {
            _cashTile = UnityEngine.GameObject.Instantiate(_prefab, _parent);
            _cashTile.SetActive(false);
            return _cashTile;
        }

        public override Tile Configure(Tile tile)
        {
            return tile;
        }
    }
}
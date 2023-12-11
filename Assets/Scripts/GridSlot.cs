using UnityEngine;

namespace Common
{
    public interface IGridSlot
    {
        IGridSlot SetPosition(GridPosition position);
        IGridSlot SetActive(bool active);
        IGridSlot Initialise(IconSpriteModel model);
    }
    public class GridSlot : MonoBehaviour, IGridSlot
    {
        [SerializeField] private SpriteRenderer _renderer;

        public IconSpriteModel Model { get; private set; } = null;

        public GridPosition Position { get; private set; }
        public IGridSlot SetActive(bool active)
        {
            _renderer.enabled = active;
            return this;
        }

        public IGridSlot SetPosition(GridPosition position)
        {
            Position = position;
            return this;
        }

        public IGridSlot Initialise(IconSpriteModel model)
        {
            Model = model;
            _renderer.sprite = Model.Icon;
            return this;
        }
    }
}
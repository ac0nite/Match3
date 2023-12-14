using UnityEngine;

namespace Common.Debug
{
    public class Slot : MonoBehaviour
    {
        public GridPosition Position { get; private set; }
        public Tile Tile { get; private set; }

        public bool IsMatch { get; set; }

        public Slot SetItem(Tile tile)
        {
            Tile = tile; 
            tile.transform.SetParent(transform, false);
            IsMatch = false;
            return this;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            Tile?.SetActive(active);
        }

        public Slot Initialise(Vector3 position, GridPosition gridPosition)
        {
            transform.position = position;
            Position = gridPosition;
            return this;
        }

        public void ChangePosition(GridPosition position)
        {
            Position = position;
        }

        public bool Match(Slot slot)
        {
            if (IsEmpty || slot.IsEmpty) return false;
            return Tile.ID == slot.Tile.ID;
        }

        public bool IsEmpty => Tile == null;
        public override string ToString() => IsEmpty ? $"[null] {Position}" : $"[{Tile.ID}] {Position}";

        public void Clean()
        {
            Tile = null;
            IsMatch = false;
        }
    }
}
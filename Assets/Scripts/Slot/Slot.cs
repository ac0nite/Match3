using Common;
using UnityEngine;

namespace Match3.General
{
    public class Slot : MonoBehaviour
    {
        public GridPosition Position { get; private set; }
        public Tile Tile { get; private set; }
        public bool IsMatch { get; set; }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            Tile?.SetActive(active);
        }
        
        public void SetGridPosition(GridPosition position)
        {
            Position = position;
            Tile?.UpdateOrder(Position.OrderIndex);
        }
        
        public void SetForceWorldPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }
        
        public void SetTile(Tile tile)
        {
            Clear();
            
            Tile = tile;
            Tile.Bind(transform, Position.OrderIndex);
        }

        public bool Match(Slot slot)
        {
            if (IsEmpty || slot.IsEmpty) return false;
            return Tile.ID == slot.Tile.ID;
        }

        public bool IsEmpty => Tile == null;
        
        public void Clear()
        {
            Tile = null;
            IsMatch = false;
        }
        public override string ToString() => IsEmpty ? $"[null] {Position}" : $"[{Tile.ID}] {Position}";
    }
}
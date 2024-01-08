using Cysharp.Threading.Tasks;
using ID;
using Match3.Models;
using Match3.Slots;
using UnityEngine;

namespace Match3.General
{
    public class Slot : SlotBase
    {
        public override UniqueID ID { get; set; }
        public override GridPosition Position { get; set; }
        public override bool IsMatch { get; set; }

        public void SetGridPosition(GridPosition position)
        {
            Position = position;
            _animator.SortingOrder = Position.RowIndex + Position.ColumnIndex;
            Debug.Log($"{this} {_animator.SortingOrder}", transform);
        }
        
        public void SetForceWorldPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }
        
        public void Initialise(TileModel spriteModel)
        {
            ID = spriteModel.ID;
            _animator.Initialise(spriteModel.SpriteAnimation);
            _animator.PlayIdle(true);
        }

        public bool Match(Slot slot)
        {
            if (IsEmpty || slot.IsEmpty) return false;
            return ID == slot.ID;
        }

        public bool IsEmpty => _animator.IsEmpty;
        
        public void Clear()
        {
            _animator.Dispose();
            IsMatch = false;
        }
        public override string ToString() => IsEmpty ? $"[null] {Position}" : $"[{ID}] {Position}";

        public UniTask PlayDestroyAnimationAsync()
        {
            return _animator.PlayDestroyAsync();
        }
    }
}
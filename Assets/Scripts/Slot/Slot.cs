using Cysharp.Threading.Tasks;
using ID;
using Match3.Models;
using Match3.Services;
using Match3.Slots;

namespace Match3.General
{
    public class Slot : SlotBase
    {
        public override UniqueID ID { get; set; }
        public override GridPosition Position { get; set; }
        public override bool IsMatch { get; set; }

        public void ChangeGridPosition(GridPosition position)
        {
            Position = position;
            _animator.SortingOrder = Position.RowIndex + Position.ColumnIndex;
            // Debug.Log($"{this} {_animator.SortingOrder}", transform);
        }
        
        public Slot Configure(IBoardService service, int rowIndex, int columnIndex)
        {
            ChangeGridPosition(new GridPosition(rowIndex, columnIndex));
            transform.localScale = service.GetTileScale;
            transform.position = service.GetWorldPosition(Position);
            return this;
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
using Common;
using Debug;

namespace Match3.Board
{
    public interface IValidator
    {
        bool IsSlot(GridPosition position);
        bool IsEmpty(GridPosition position);
        bool IsEmptySlot(GridPosition position);

    }
    public class SlotValidator : IValidator
    {
        private readonly IBoardModel _board;

        public SlotValidator(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
        }

        public bool IsSlot(GridPosition position)
        {
            return position.ColumnIndex >= 0 &&
                   position.ColumnIndex < _board.Column &&
                   position.RowIndex >= 0 &&
                   position.RowIndex < _board.Row;
        }

        public bool IsEmpty(GridPosition position)
        {
            return _board.Slots[position.RowIndex, position.ColumnIndex].Tile == null;
        }

        public bool IsEmptySlot(GridPosition position)
        {
            if (!IsSlot(position))
                return true;

            return _board.Slots[position.RowIndex, position.ColumnIndex].IsEmpty;
        }
    }
}
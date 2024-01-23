using Common;
using Match3.Context;
using Match3.Models;

namespace Match3.General
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
                   position.ColumnIndex < _board.Size.Column &&
                   position.RowIndex >= 0 &&
                   position.RowIndex < _board.Size.Row;
        }

        public bool IsEmpty(GridPosition position)
        {
            return _board[position].IsEmpty;
        }

        public bool IsEmptySlot(GridPosition position)
        {
            if (position.IsEmpty)
                return false;
            
            if (!IsSlot(position))
                return true;

            return _board[position].IsEmpty;
        }
    }
}
using Debug;
using Match3.Board;

namespace Common.Debug
{
    public interface IMatching
    {
        bool Find();
    }
    public class FindMatching : IMatching
    {
        bool isMatch = false;
        private readonly IBoardModel _board;
        private readonly IValidator _validator;

        public FindMatching(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _validator = context.Resolve<IValidator>();
        }
        
        public bool Find()
        {
            isMatch = false;
            for (int i = 0; i < _board.Row; i++)
            {
                for (int j = 0; j < _board.Column; j++)
                {   
                    TryToFindAndMark(_board.Slots[i, j]);
                }
            }

            UnityEngine.Debug.Log($"Find: {isMatch}");
            return isMatch;
        }

        private void TryToFindAndMark(Slot slot)
        {
            if (IsMatch3(slot, slot.Position.Left, slot.Position.Right))
            {
                MarkAsMatch(slot.Position.Left);
                MarkAsMatch(slot.Position.Right);
                slot.IsMatch = isMatch = true;
            }
            
            if (IsMatch3(slot, slot.Position.Up, slot.Position.Down))
            {
                MarkAsMatch(slot.Position.Up);
                MarkAsMatch(slot.Position.Down);
                slot.IsMatch = isMatch = true;
            }
        }

        private void MarkAsMatch(GridPosition position)
        {
            _board.Slots[position.RowIndex, position.ColumnIndex].IsMatch = true;
        }

        private bool IsMatch3(Slot slot, GridPosition neighborOne, GridPosition neighborTwo)
        {
            if (_validator.IsEmptySlot(neighborOne) || _validator.IsEmptySlot(neighborTwo))
                return false;

            if (slot.Match(_board.Slots[neighborOne.RowIndex, neighborOne.ColumnIndex]) &&
                slot.Match(_board.Slots[neighborTwo.RowIndex, neighborTwo.ColumnIndex]))
                return true;

            return false;
        }
    }
}
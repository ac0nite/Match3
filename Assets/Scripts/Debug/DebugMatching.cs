namespace Common.Debug
{
    public class DebugMatching : IMatching
    {
        private readonly MatchGame _match;
        private readonly Slot[,] _slots;
        private readonly int _row;
        private readonly int _column;
        private readonly Checker _checker;
        bool isMatch = false;

        public DebugMatching(MatchGame matchGame, Checker checker)
        {
            _match = matchGame;
            _slots = _match.BoardSlot;
            _row = _match.BoardSlot.GetLength(0);
            _column = _match.BoardSlot.GetLength(1);
            _checker = checker;
        }
        
        public bool FindMatches()
        {
            isMatch = false;
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    TryToFindAndMark(_slots[i, j]);
                }
            }

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
            _slots[position.RowIndex, position.ColumnIndex].IsMatch = true;
        }

        private bool IsMatch3(Slot slot, GridPosition neighborOne, GridPosition neighborTwo)
        {
            if (_checker.IsEmptySlot(neighborOne) || _checker.IsEmptySlot(neighborTwo))
                return false;

            if (slot.Match(_slots[neighborOne.RowIndex, neighborOne.ColumnIndex]) &&
                slot.Match(_slots[neighborTwo.RowIndex, neighborTwo.ColumnIndex]))
                return true;

            return false;
        }
    }
}
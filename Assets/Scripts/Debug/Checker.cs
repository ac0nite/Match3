using System;

namespace Common.Debug
{
    public class Checker
    {
        private readonly int _columnWidth;
        private readonly int _rowHeight;
        private readonly Slot[,] _slots;

        public Checker(int rowHeight, int columnWidth)
        {
            _columnWidth = columnWidth;
            _rowHeight = rowHeight;
        }

        public Checker(Slot[,] slotGrid)
        {
            _slots = slotGrid;
            _rowHeight = _slots.GetLength(0);
            _columnWidth = _slots.GetLength(1);
        }

        public bool IsSlot(GridPosition position)
        {
            return position.ColumnIndex >= 0 &&
                   position.ColumnIndex < _columnWidth &&
                   position.RowIndex >= 0 &&
                   position.RowIndex < _rowHeight;
        }

        public bool IsEmpty(GridPosition position)
        {
            return _slots[position.RowIndex, position.ColumnIndex].Tile == null;
        }

        public bool IsEmptySlot(GridPosition position)
        {
            if (!IsSlot(position))
                return true;

            return _slots[position.RowIndex, position.ColumnIndex].IsEmpty;
        }
    }
}
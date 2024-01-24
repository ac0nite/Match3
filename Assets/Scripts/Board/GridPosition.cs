
using System;

namespace Match3.General
{
    [Serializable]
    public struct GridPosition
    {
        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }

        public GridPosition(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public bool IsUp(GridPosition position)
        {
            return RowIndex == position.RowIndex - 1 &&
                   ColumnIndex == position.ColumnIndex;
        }
        
        public bool IsDown(GridPosition position)
        {
            return RowIndex == position.RowIndex + 1 &&
                   ColumnIndex == position.ColumnIndex;
        }
        
        public bool IsLeft(GridPosition position)
        {
            return RowIndex == position.RowIndex &&
                   ColumnIndex == position.ColumnIndex + 1;
        }
        
        public bool IsRight(GridPosition position)
        {
            return RowIndex == position.RowIndex &&
                   ColumnIndex == position.ColumnIndex - 1;
        }

        public bool IsSides(GridPosition position)
        {
            return IsLeft(position) || IsRight(position);
        }

        public static GridPosition Empty => new GridPosition(-1, -1);
        public bool IsEmpty => RowIndex < 0 && ColumnIndex < 0;
        public GridPosition Up => new GridPosition(RowIndex + 1, ColumnIndex);
        public GridPosition Down => new GridPosition(RowIndex - 1, ColumnIndex);
        public GridPosition Left => new GridPosition(RowIndex, ColumnIndex - 1);
        public GridPosition Right => new GridPosition(RowIndex, ColumnIndex + 1);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var grid = (GridPosition) obj;
            return RowIndex == grid.RowIndex && ColumnIndex == grid.ColumnIndex;
        }
        public override string ToString()
        {
            return $"[{RowIndex}, {ColumnIndex}]";
        }
    }
}
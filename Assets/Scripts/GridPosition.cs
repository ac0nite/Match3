﻿namespace Common
{
    public struct GridPosition
    {
        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }
        public int RowRenderer { get; private set; }

        public GridPosition(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            RowRenderer = -1;
        }
        
        public GridPosition(int rowIndex, int columnIndex, int rowRenderer)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            RowRenderer = rowRenderer;
        }

        public void Change(GridPosition position)
        {
            ColumnIndex = position.ColumnIndex;
            RowIndex = position.RowIndex;
            RowRenderer = position.RowRenderer;
        }

        public bool IsUp(GridPosition position)
        {
            return ColumnIndex == position.ColumnIndex && 
                   RowIndex == position.RowIndex + 1;
        }
        
        public bool IsDown(GridPosition position)
        {
            return ColumnIndex == position.ColumnIndex && 
                   RowIndex == position.RowIndex - 1;
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
        public GridPosition Up => new GridPosition(RowIndex - 1, ColumnIndex);
        public GridPosition Down => new GridPosition(RowIndex + 1, ColumnIndex);
        public GridPosition Left => new GridPosition(RowIndex, ColumnIndex - 1);
        public GridPosition Right => new GridPosition(RowIndex, ColumnIndex + 1);


        public override string ToString()
        {
            return $"[{RowIndex}, {ColumnIndex}] [{RowRenderer}]";
        }
    }
}
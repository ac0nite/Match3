namespace Common
{
    public struct GridPosition
    {
        public int RowIndex { get; }
        public int ColumnIndex { get; }
        
        public GridPosition(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public bool IsUp(GridPosition position)
        {
            return ColumnIndex < position.ColumnIndex;
        }

        public static GridPosition Empty => new GridPosition(-1, -1);
        public bool IsEmpty => RowIndex > 0 && ColumnIndex > 0;
    }
}
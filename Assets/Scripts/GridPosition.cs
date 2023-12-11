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
    }
}
using System;
using Common;
using Common.Debug;
using UnityEngine;

namespace Debug
{
    public interface IBoardService
    {
        void Initialise(BoardParam boardParam);
        Vector3 GetWorldPosition(GridPosition begin);
        GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition);
        Vector3 GetWorldPosition(int rowIndex, int columnIndex);
        int OrderLayer(int rowIndex, int columnIndex);
        void CleanSlot(Slot slot);
    }
    
    public class BoardService : IBoardService
    {
        private readonly IBoardModel _boardModel;
        private BoardParam _boardParam;
        private Vector3 _originalPosition;
        private readonly IPool<Tile> _tilePool;

        public BoardService(ApplicationContext context)
        {
            _boardModel = context.Resolve<IBoardModel>();
            _tilePool = context.Resolve<IPool<Tile>>();
        }

        public void Initialise(BoardParam boardParam)
        {
            _boardParam = boardParam;
            _originalPosition = GetOriginPosition(_boardParam.Row, _boardParam.Column);
            _boardModel.Initialise(_boardParam.Row, _boardParam.Column);
        }

        public Vector3 GetWorldPosition(GridPosition position)
        {
            return new Vector3(position.ColumnIndex, -position.RowIndex) * _boardParam.TileSize + _originalPosition;
        }

        public int OrderLayer(int rowIndex, int columnIndex)
        {
            return _boardModel.Row * _boardModel.Column - _boardModel.Column * (rowIndex + 1) + 1 + columnIndex;
        }

        public void CleanSlot(Slot slot)
        {
            _tilePool.Put(slot.Tile);
            slot.Clear();
        }
        
        public GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition)
        {
            var rowIndex = (worldPointerPosition - _originalPosition).y / _boardParam.TileSize;
            var columnIndex = (worldPointerPosition - _originalPosition).x / _boardParam.TileSize;

            return new GridPosition(Convert.ToInt32(-rowIndex), Convert.ToInt32(columnIndex));
        }

        public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, -rowIndex) * _boardParam.TileSize + _originalPosition;
        }

        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            var offsetY = Mathf.Floor(rowCount / 2.0f) * _boardParam.TileSize;
            var offsetX = Mathf.Floor(columnCount / 2.0f) * _boardParam.TileSize;

            return new Vector3(-offsetX, offsetY);
        }
    }
}
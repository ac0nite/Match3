using System;
using Board;
using Common;
using Cysharp.Threading.Tasks;
using Match3.Context;
using Match3.General;
using Match3.Models;
using UnityEngine;

namespace Match3.Services
{
    public interface IBoardService
    {
        void Initialise(BoardSettings boardSettings);
        Vector3 GetWorldPosition(GridPosition begin);
        GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition);
        Vector3 GetWorldPosition(int rowIndex, int columnIndex);
        int OrderLayer(int rowIndex, int columnIndex);
        UniTask CleanSlot(Slot slot);
        Vector3 GetTileScale { get; }
    }
    
    public class BoardService : IBoardService
    {
        private readonly IBoardModel _boardModel;
        // private Vector3 _originalPosition;
        private readonly Camera _camera;
        private BoardBounds BoardBounds { get; set; }
        public Vector3 GetTileScale { get; private set; }

        public BoardService(ApplicationContext context, Camera camera)
        {
            _boardModel = context.Resolve<IBoardModel>();
            _camera = camera;
        }

        public void Initialise(BoardSettings boardSettings)
        {
            BoardBounds ??= new BoardBounds(boardSettings.Bounds, _camera);
            BoardBounds.Calculate(boardSettings.Size);
            GetTileScale = Vector3.one * BoardBounds.TileScale;
            
            // _originalPosition = GetOriginPosition(_boardSettings.Row, _boardSettings.Column);
            _boardModel.Initialise(boardSettings.Size.Row, boardSettings.Size.Column);
            
            //Debug.Log($"Original Position:{_originalPosition}");
        }

        public Vector3 GetWorldPosition(GridPosition position)
        {
            return BoardBounds.WorldPosition(position.RowIndex, position.ColumnIndex);
            //return new Vector3(position.ColumnIndex, -position.RowIndex) * BoardBounds.TileSize + BoardBounds.OriginalPosition;
        }
        
        public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            // return new Vector3(columnIndex, rowIndex) * BoardBounds.TileSize + BoardBounds.OriginalPosition;
            return BoardBounds.WorldPosition(rowIndex, columnIndex);
        }

        public int OrderLayer(int rowIndex, int columnIndex)
        {
            return _boardModel.Row * _boardModel.Column - _boardModel.Column * (rowIndex + 1) + 1 + columnIndex;
        }

        public async UniTask CleanSlot(Slot slot)
        {
            _boardModel.Counter[slot.ID]--;
            
            await slot.PlayDestroyAnimationAsync();
            
            slot.Clear();

            await UniTask.DelayFrame(1);
        }

        public GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition)
        {
            var rowIndex = (worldPointerPosition - BoardBounds.OriginalPosition).y / BoardBounds.TileSize;
            var columnIndex = (worldPointerPosition - BoardBounds.OriginalPosition).x / BoardBounds.TileSize;

            return new GridPosition(Convert.ToInt32(rowIndex), Convert.ToInt32(columnIndex));
        }

        // private Vector3 GetOriginPosition(int rowCount, int columnCount)
        // {
        //     // var offsetY = Mathf.Floor(rowCount / 2.0f) * _boardParam.TileSize;
        //     // var offsetX = Mathf.Floor(columnCount / 2.0f) * _boardParam.TileSize;
        //     
        //     
        //     
        //     var offsetY =  (rowCount / 2.0f);
        //     var offsetX =  (columnCount / 2.0f);
        //     
        //     return new Vector3(-offsetX, -offsetY);
        // }
    }
}
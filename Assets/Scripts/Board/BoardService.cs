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
        private BoundsParam Bounds { get; set; }
        public Vector3 GetTileScale { get; private set; }

        public BoardService(ApplicationContext context, Camera camera)
        {
            _boardModel = context.Resolve<IBoardModel>();
            _camera = camera;
        }

        public void Initialise(BoardSettings boardSettings)
        {
            Bounds ??= new BoundsParam(boardSettings.Bounds, _camera);
            Bounds.Calculate(new Vector2Int(boardSettings.Row, boardSettings.Column));
            GetTileScale = Vector3.one * Bounds.TileScale;
            
            // _originalPosition = GetOriginPosition(_boardSettings.Row, _boardSettings.Column);
            _boardModel.Initialise(boardSettings.Row, boardSettings.Column);
            
            //Debug.Log($"Original Position:{_originalPosition}");
        }

        public Vector3 GetWorldPosition(GridPosition position)
        {
            return new Vector3(position.ColumnIndex, -position.RowIndex) * Bounds.TileSize + Bounds.OriginalPosition;
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
            var rowIndex = (worldPointerPosition - Bounds.OriginalPosition).y / Bounds.TileSize;
            var columnIndex = (worldPointerPosition - Bounds.OriginalPosition).x / Bounds.TileSize;

            return new GridPosition(Convert.ToInt32(rowIndex), Convert.ToInt32(columnIndex));
        }

        public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, rowIndex) * Bounds.TileSize + Bounds.OriginalPosition;
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
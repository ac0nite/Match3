using System;
using Board;
using Cysharp.Threading.Tasks;
using Match3.Context;
using Match3.General;
using Match3.Models;
using UnityEngine;

namespace Match3.Services
{
    public interface IBoardService
    {
        void Initialise(BoardSettings boardSettings, BoardSize size);
        Vector3 GetWorldPosition(GridPosition begin);
        GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition);
        Vector3 GetWorldPosition(int rowIndex, int columnIndex);
        int OrderLayer(int rowIndex, int columnIndex);
        UniTask CleanSlot(Slot slot);
        BoardBounds BoardBounds { get; }
    }
    
    public class BoardService : IBoardService
    {
        private readonly IBoardModel _boardModel;
        private readonly Camera _camera;
        public BoardService(ApplicationContext context, Camera camera)
        {
            _boardModel = context.Resolve<IBoardModel>();
            _camera = camera;
        }
        
        public BoardBounds BoardBounds { get; private set; }

        public void Initialise(BoardSettings boardSettings, BoardSize size)
        {
            BoardBounds ??= new BoardBounds(boardSettings.Bounds, _camera);
            
            BoardBounds.Calculate(size);
            _boardModel.Initialise(size);
        }

        public Vector3 GetWorldPosition(GridPosition position)
        {
            return BoardBounds.WorldPosition(position.RowIndex, position.ColumnIndex);
        }
        
        public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return BoardBounds.WorldPosition(rowIndex, columnIndex);
        }

        public int OrderLayer(int rowIndex, int columnIndex)
        {
            return _boardModel.Size.Row * _boardModel.Size.Column - _boardModel.Size.Column * (rowIndex + 1) + 1 + columnIndex;
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
    }
}
using System;
using Debug;
using DG.Tweening;
using Match3.Board;

namespace Common.Debug
{
    public interface IShiftingSlots
    {
        void Shift(GridPosition begin, GridPosition end, Action callback);
        void AllShift();
    }
    public class ShiftingSlots : IShiftingSlots
    {
        private readonly IValidator _validator;
        private readonly IBoardModel _board;
        private readonly IBoardService _boardService;
        
        private Slot _cashedSlot;
        private readonly float _animationTime;

        public ShiftingSlots(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _boardService = context.Resolve<IBoardService>();
            _validator = context.Resolve<IValidator>();
            
            _animationTime = context.Settings.ShiftingAnimationTime;
        }
        public void Shift(GridPosition begin, GridPosition end, Action callback)
        {
            var from = _boardService.GetWorldPosition(begin);
            var to = _boardService.GetWorldPosition(end);

            var oneSlot = _board.Slots[begin.RowIndex, begin.ColumnIndex];
            var twoSlot = _board.Slots[end.RowIndex, end.ColumnIndex];

            var bPos = oneSlot.Position;
            var ePos= twoSlot.Position;

            _board.Slots[begin.RowIndex, begin.ColumnIndex].SetGridPosition(ePos);
            _board.Slots[end.RowIndex, end.ColumnIndex].SetGridPosition(bPos);
            
            
            (_board.Slots[begin.RowIndex, begin.ColumnIndex], _board.Slots[end.RowIndex, end.ColumnIndex]) = 
                (_board.Slots[end.RowIndex, end.ColumnIndex], _board.Slots[begin.RowIndex, begin.ColumnIndex]);
            
            oneSlot.transform.DOMove(to, _animationTime);
            twoSlot.transform.DOMove(from, _animationTime).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        public void AllShift()
        {
            GridPosition from, to;
            
            for (int i = _board.Row - 1; i >= 0; i--)
            {
                for (int j = _board.Column - 1; j >= 0; j--)
                {
                    if (_board.Slots[i, j].IsEmpty)
                    {
                        // UnityEngine.Debug.Log($"* {slots[i, j]}");
                        to = from = _board.Slots[i, j].Position;
                        while (_validator.IsEmptySlot(from = from.Up))
                        {
                            // UnityEngine.Debug.Log($"- UP {slots[i, j]}");
                            if (!_validator.IsSlot(from))
                                break;
                        }

                        if (_validator.IsSlot(from))
                        {
                            Shift(from, to, null);
                        }
                    }
                }
            }
        }
    }
}
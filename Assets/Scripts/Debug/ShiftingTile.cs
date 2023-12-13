using System;
using DG.Tweening;

namespace Common.Debug
{
    public class ShiftingTile
    {
        private readonly MatchGame _match;
        private Slot _cashedSlot;
        public ShiftingTile(MatchGame match)
        {
            _match = match;
        }
        public void Shift(GridPosition begin, GridPosition end, Action callback)
        {
            var from = _match.GetWorldPosition(begin);
            var to = _match.GetWorldPosition(end);

            var oneSlot = _match.BoardSlot[begin.RowIndex, begin.ColumnIndex];
            var twoSlot = _match.BoardSlot[end.RowIndex, end.ColumnIndex];
            
            _match.BoardSlot[begin.RowIndex, begin.ColumnIndex].ChangePosition(end);
            _match.BoardSlot[end.RowIndex, end.ColumnIndex].ChangePosition(begin);

            oneSlot.transform.DOMove(to, 0.3f);
            twoSlot.transform.DOMove(from, 0.3f).OnComplete(() =>
            {
                (_match.BoardSlot[begin.RowIndex, begin.ColumnIndex], _match.BoardSlot[end.RowIndex, end.ColumnIndex]) = (_match.BoardSlot[end.RowIndex, end.ColumnIndex], _match.BoardSlot[begin.RowIndex, begin.ColumnIndex]);
                callback?.Invoke();
            });
        }
    }
}
using System;
using DG.Tweening;

namespace Common.Debug
{
    public class ShiftingSlot
    {
        private readonly MatchGame _match;
        private Slot _cashedSlot;
        private readonly Checker _checker;

        public ShiftingSlot(MatchGame match, Checker checker)
        {
            _match = match;
            _checker = checker;
        }
        public void Shift(GridPosition begin, GridPosition end, Action callback)
        {
            var from = _match.GetWorldPosition(begin);
            var to = _match.GetWorldPosition(end);

            var oneSlot = _match.BoardSlot[begin.RowIndex, begin.ColumnIndex];
            var twoSlot = _match.BoardSlot[end.RowIndex, end.ColumnIndex];

            _match.BoardSlot[begin.RowIndex, begin.ColumnIndex].ChangePosition(end);
            _match.BoardSlot[end.RowIndex, end.ColumnIndex].ChangePosition(begin);
            
            (_match.BoardSlot[begin.RowIndex, begin.ColumnIndex], _match.BoardSlot[end.RowIndex, end.ColumnIndex]) = (_match.BoardSlot[end.RowIndex, end.ColumnIndex], _match.BoardSlot[begin.RowIndex, begin.ColumnIndex]);
            
            oneSlot.transform.DOMove(to, 0.3f);
            twoSlot.transform.DOMove(from, 0.3f).OnComplete(() =>
            {
                //(oneSlot, twoSlot) = (twoSlot, oneSlot);
                // (_match.BoardSlot[begin.RowIndex, begin.ColumnIndex], _match.BoardSlot[end.RowIndex, end.ColumnIndex]) = (_match.BoardSlot[end.RowIndex, end.ColumnIndex], _match.BoardSlot[begin.RowIndex, begin.ColumnIndex]);
                callback?.Invoke();
            });
        }

        public void AllShift()
        {
            var slots = _match.BoardSlot;
            var row = _match.BoardSlot.GetLength(0);
            var column = _match.BoardSlot.GetLength(1);
            GridPosition from, to;
            
            for (int i = row - 1; i >= 0; i--)
            {
                for (int j = column-1; j >= 0; j--)
                {
                    if (slots[i, j].IsEmpty)
                    {
                        // UnityEngine.Debug.Log($"* {slots[i, j]}");
                        to = from = slots[i, j].Position;
                        while (_checker.IsEmptySlot(from = from.Up))
                        {
                            // UnityEngine.Debug.Log($"- UP {slots[i, j]}");
                            if (!_checker.IsSlot(from))
                                break;
                        }

                        if (_checker.IsSlot(from))
                        {
                            Shift(from, to, null);
                        }
                    }
                }
            }
        }
    }
}
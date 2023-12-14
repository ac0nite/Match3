using System;
using UnityEngine;

namespace Common.Debug
{
    public interface IGameplay
    {
        void SetActive(bool active);
    }
    
    public class Gameplay : IGameplay
    {
        private readonly IInputSystem _input;
        private readonly MatchGame _match;
        private readonly ShiftingTile _shifting;
        
        private GridPosition _beginPositionGrid;
        private GridPosition _endPositionGrid;
        private readonly IMatching _findMatch;
        private readonly IClearingBoard _cleaner;

        public Gameplay(MatchGame match, IInputSystem input, ShiftingTile shifting, IMatching findMatch, IClearingBoard clearingBoard)
        {
            _match = match;
            _input = input;
            _shifting = shifting;
            _findMatch = findMatch;
            _cleaner = clearingBoard;
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                _input.PointerDown += TryToCheckSlot;
                _input.PointerDrag += TryToNextCheckSlot;
            }
            else
            {
                _input.PointerDown -= TryToCheckSlot;
                _input.PointerDrag -= TryToNextCheckSlot;
            }
        }

        private void TryToCheckSlot(Vector3 worldPosition)
        {
            _beginPositionGrid = _match.GetGridPositionByPointer(worldPosition);
            if (_match.Checker.IsEmptySlot(_beginPositionGrid))
                _beginPositionGrid = GridPosition.Empty;
            else
            {
                UnityEngine.Debug.Log($"Begin grid:{_beginPositionGrid} slot:{_match.BoardSlot[_beginPositionGrid.RowIndex, _beginPositionGrid.ColumnIndex].Position}");   
            }
            
            //UnityEngine.Debug.Log($"grid:{_beginPositionGrid} IsEmptySlot:{_match.Checker.IsEmptySlot(_beginPositionGrid)}");   

            //Debug_matchGame.Slots[grid.RowIndex, grid.ColumnIndex]
        }
        
        private void TryToNextCheckSlot(Vector3 worldPosition)
        {
            if (_beginPositionGrid.IsEmpty)
            {
                return;
            }
            
            _endPositionGrid = _match.GetGridPositionByPointer(worldPosition);

            if (_match.Checker.IsSlot(_endPositionGrid))
            {
                //UnityEngine.Debug.Log($"SIDES LEFT:{_beginPositionGrid.IsLeft(_endPositionGrid)} RIGHT:{_beginPositionGrid.IsRight(_endPositionGrid)} {_endPositionGrid.ToString()}");
                if (_beginPositionGrid.IsSides(_endPositionGrid) || _beginPositionGrid.IsDown(_endPositionGrid))
                {
                    UnityEngine.Debug.Log($"SIDES AND DOWN {_endPositionGrid.ToString()}");
                    Shift();
                }
                else if(_beginPositionGrid.IsUp(_endPositionGrid) && !_match.Checker.IsEmpty(_endPositionGrid))
                {
                    UnityEngine.Debug.Log($"UP {_endPositionGrid.ToString()}");
                    Shift();
                }
                else
                {
                    _endPositionGrid = GridPosition.Empty;
                    UnityEngine.Debug.Log($"-");
                }
            }
        }

        private void Shift()
        {
            _input.Lock = true;
            
            _shifting.Shift(_beginPositionGrid, _endPositionGrid, () =>
            {
                TryToMatchesAndShifting(() =>
                {
                    _beginPositionGrid = GridPosition.Empty;
                    _endPositionGrid = GridPosition.Empty;
                    _input.Lock = false;
                });
            });
        }

        private void TryToMatchesAndShifting(Action callback)
        {
            while (_findMatch.FindMatches())
            {
                _cleaner.Clean(null);
            }

            _shifting.AllShift();
            callback?.Invoke();
        }
    }
}
using System;
using System.Threading;
using Debug;
using Match3.Board;
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

        private readonly IMatching _matching;
        private readonly IBoardService _boardService;
        private readonly IValidator _validator;
        private readonly IClearingSlots _cleaning;
        private readonly IBoardModel _board;
        private readonly IShiftingSlots _shifting;
        
        private GridPosition _beginPositionGrid;
        private GridPosition _endPositionGrid;
        private readonly ICheckResult _checkingResult;

        public Gameplay(ApplicationContext context)
        {
            _input = context.Resolve<IInputSystem>();
            _board = context.Resolve<IBoardModel>();
            _boardService = context.Resolve<IBoardService>();
            _validator = context.Resolve<IValidator>();
            _cleaning = context.Resolve<IClearingSlots>();
            _shifting = context.Resolve<IShiftingSlots>();
            _matching = context.Resolve<IMatching>();
            _checkingResult = context.Resolve<ICheckResult>();
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                _input.PointerDown += TryToCheckSlot;
                _input.PointerDrag += TryToNextCheckSlot;
                _input.Lock = false;
            }
            else
            {
                _input.PointerDown -= TryToCheckSlot;
                _input.PointerDrag -= TryToNextCheckSlot;
                _input.Lock = true;
            }
        }

        private void TryToCheckSlot(Vector3 worldPosition)
        {
            _beginPositionGrid = _boardService.GetGridPositionByPointer(worldPosition);
            if (_validator.IsEmptySlot(_beginPositionGrid))
                _beginPositionGrid = GridPosition.Empty;
            else
            {
                UnityEngine.Debug.Log($"Begin grid:{_beginPositionGrid} slot:{_board.Slots[_beginPositionGrid.RowIndex, _beginPositionGrid.ColumnIndex].Position}");   
            }
        }
        
        private void TryToNextCheckSlot(Vector3 worldPosition)
        {
            if (_beginPositionGrid.IsEmpty)
            {
                return;
            }
            
            _endPositionGrid = _boardService.GetGridPositionByPointer(worldPosition);

            if (_validator.IsSlot(_endPositionGrid))
            {
                if (_beginPositionGrid.IsSides(_endPositionGrid) || _beginPositionGrid.IsDown(_endPositionGrid))
                {
                    UnityEngine.Debug.Log($"SIDES AND DOWN {_endPositionGrid.ToString()}");
                    UpdateBoardAsync();
                }
                else if(_beginPositionGrid.IsUp(_endPositionGrid) && !_validator.IsEmpty(_endPositionGrid))
                {
                    UnityEngine.Debug.Log($"UP {_endPositionGrid.ToString()}");
                    // Shift();
                    UpdateBoardAsync();
                }
                else
                {
                    _endPositionGrid = GridPosition.Empty;
                    UnityEngine.Debug.Log($"-");
                }
            }
        }

        private async void UpdateBoardAsync()
        {
            Log($"UpdateBoardAsync begin");
            _input.Lock = true;
            await _shifting.Shift(_beginPositionGrid, _endPositionGrid);
            Log($"after shift");

            _matching.Find();
            do
            {
                await _cleaning.MatchExecuteAsync();
                await _shifting.AllShiftAsync();
                // await UniTask.Delay(100);
            } 
            while (_matching.Find());
            
            _checkingResult.Check();
            
            
            _beginPositionGrid = GridPosition.Empty;
            _endPositionGrid = GridPosition.Empty;
            _input.Lock = false;
            
            Log($"UpdateBoardAsync end");
        }

        private void Log(string message)
        {
            UnityEngine.Debug.Log($"[{Thread.CurrentThread.ManagedThreadId}] {message}");
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
            var isMatch = _matching.Find();
            UnityEngine.Debug.Log(isMatch);
            do
            {
                _cleaning.MatchExecute(() => _shifting.AllShift());
            } 
            while (_matching.Find());
            
            // while (_findMatch.FindMatches())
            // {
            //     _findMatch.FindMatches();
            //     _cleaner.Clean(() => _shifting.AllShift());
            // }
            //
            // _shifting.AllShift();
            callback?.Invoke();
        }
    }
}
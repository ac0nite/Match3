using Common;
using Cysharp.Threading.Tasks;
using Match3.Board;
using Match3.Context;
using Match3.Models;
using Match3.Services;
using UnityEngine;

namespace Match3.General
{
    public interface IGameplay
    {
        void SetActive(bool active);
        UniTask UpdateBoardAsync();
    }
    
    public class Gameplay : IGameplay
    {
        private readonly IInputSystem _input;

        private readonly IMatching _matching;
        private readonly IBoardService _boardService;
        private readonly IValidator _validator;
        private readonly IClearing _cleaning;
        private readonly IShifting _shifting;
        
        private GridPosition _beginPositionGrid;
        private GridPosition _endPositionGrid;
        private readonly ICheckResult _checkingResult;
        private readonly IUpdateBoard _updateBoard;

        public Gameplay(ApplicationContext context)
        {
            _input = context.Resolve<IInputSystem>();
            _boardService = context.Resolve<IBoardService>();
            _validator = context.Resolve<IValidator>();
            _cleaning = context.Resolve<IClearing>();
            _shifting = context.Resolve<IShifting>();
            _matching = context.Resolve<IMatching>();
            _updateBoard = context.Resolve<IUpdateBoard>();
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
            //Debug.Log($"Begin:{_beginPositionGrid}");
            
            if (_validator.IsEmptySlot(_beginPositionGrid))
                _beginPositionGrid = GridPosition.Empty;
            else
            {
                Debug.Log($"Begin:{_beginPositionGrid}");   
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
                    UpdateBoardAsync();
                }
                else if(_beginPositionGrid.IsUp(_endPositionGrid) && !_validator.IsEmpty(_endPositionGrid))
                {
                    UpdateBoardAsync();
                }
                else
                {
                    _endPositionGrid = GridPosition.Empty;
                }
            }
        }

        public async UniTask UpdateBoardAsync()
        {
            _input.Lock = true;
            
            Debug.Log($"End:{_endPositionGrid}");   

            await _shifting.SwapAsync(_beginPositionGrid, _endPositionGrid);
            await _updateBoard.UpdateAsync();

            _beginPositionGrid = GridPosition.Empty;
            _endPositionGrid = GridPosition.Empty;
            
            _input.Lock = false;
        }
    }
}
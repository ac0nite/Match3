using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Context;
using Match3.Models;
using Match3.Services;

namespace Match3.General
{
    public interface IShifting
    {
        UniTask SwapAsync(GridPosition begin, GridPosition end);
        UniTask AllShiftAsync();
    }
    public class Shifting : IShifting
    {
        private readonly IValidator _validator;
        private readonly IBoardModel _board;
        private readonly IBoardService _boardService;
        
        private Slot _cashedSlot;
        private readonly float _animationTime;
        private readonly List<UniTask> _tasks;
        
        private GridPosition _fromGridPosition = GridPosition.Empty;
        private GridPosition _toGridPosition = GridPosition.Empty;

        public Shifting(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _boardService = context.Resolve<IBoardService>();
            _validator = context.Resolve<IValidator>();

            _animationTime = context.Settings.Animation.ShiftingTime;

            _tasks = new List<UniTask>();
        }

        public async UniTask SwapAsync(GridPosition begin, GridPosition end)
        {
            var from = _boardService.GetWorldPosition(begin);
            var to = _boardService.GetWorldPosition(end);

            var oneSlot = _board[begin];
            var twoSlot = _board[end];

            var bPos = oneSlot.Position;
            var ePos= twoSlot.Position;

            _board[begin].ChangeGridPosition(ePos);
            _board[end].ChangeGridPosition(bPos);
            
            (_board[begin], _board[end]) = (_board[end], _board[begin]);
            
            oneSlot.transform.DOMove(to, _animationTime);
            await twoSlot.transform.DOMove(from, _animationTime).AsyncWaitForCompletion().AsUniTask();
        }

        public async UniTask AllShiftAsync()
        {
            _tasks.Clear();

            while (Find(out _fromGridPosition, out _toGridPosition))
            {
                _tasks.Add(SwapAsync(_fromGridPosition, _toGridPosition));
                await UniTask.DelayFrame(2);
            }

            await UniTask.WhenAll(_tasks);

            bool Find(out GridPosition from, out GridPosition to)
            {
                from = GridPosition.Empty;
                to = GridPosition.Empty;
                
                foreach (Slot slot in _board.Slots)
                {
                    if (slot.IsEmpty)
                    {
                        to = from = slot.Position;
                        while (_validator.IsEmptySlot(from = from.Up))
                        {
                            if (!_validator.IsSlot(from))
                                break;
                        }
                        if (_validator.IsSlot(from))
                            return true;
                    }
                }
                return false;
            }
        }
    }
}
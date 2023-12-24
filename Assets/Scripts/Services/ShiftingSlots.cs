using System;
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
        void Shift(GridPosition begin, GridPosition end, Action callback);
        void AllShift();
        
        UniTask Shift(GridPosition begin, GridPosition end);
        UniTask AllShiftAsync();
    }
    public class Shifting : IShifting
    {
        private readonly IValidator _validator;
        private readonly IBoardModel _board;
        private readonly IBoardService _boardService;
        
        private Slot _cashedSlot;
        private readonly float _animationTime;

        public Shifting(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _boardService = context.Resolve<IBoardService>();
            _validator = context.Resolve<IValidator>();
            
            _animationTime = context.Settings.Animation.ShiftingTime;
        }
        public void Shift(GridPosition begin, GridPosition end, Action callback)
        {
            var from = _boardService.GetWorldPosition(begin);
            var to = _boardService.GetWorldPosition(end);

            var oneSlot = _board[begin];
            var twoSlot = _board[end];

            var bPos = oneSlot.Position;
            var ePos= twoSlot.Position;

            _board[begin].SetGridPosition(ePos);
            _board[end].SetGridPosition(bPos);
            
            
            (_board[begin], _board[end]) = (_board[end], _board[begin]);
            
            oneSlot.transform.DOMove(to, _animationTime);
            twoSlot.transform.DOMove(from, _animationTime).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
        
        public async UniTask Shift(GridPosition begin, GridPosition end)
        {
            var from = _boardService.GetWorldPosition(begin);
            var to = _boardService.GetWorldPosition(end);

            var oneSlot = _board[begin];
            var twoSlot = _board[end];

            var bPos = oneSlot.Position;
            var ePos= twoSlot.Position;

            _board[begin].SetGridPosition(ePos);
            _board[end].SetGridPosition(bPos);
            
            (_board[begin], _board[end]) = (_board[end], _board[begin]);
            
            oneSlot.transform.DOMove(to, _animationTime);
            await twoSlot.transform.DOMove(from, _animationTime).AsyncWaitForCompletion().AsUniTask();
        }

        public void AllShift()
        {
            GridPosition from, to;
            
            for (int i = _board.Row - 1; i >= 0; i--)
            {
                for (int j = _board.Column - 1; j >= 0; j--)
                {
                    if (_board[new GridPosition(i,j)].IsEmpty)
                    {
                        // UnityEngine.Debug.Log($"* {slots[i, j]}");
                        to = from = _board[new GridPosition(i,j)].Position;
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
        
        public async UniTask AllShiftAsync()
        {
            Slot lastSlot = null;
            UniTask lastTask = new UniTask();

            List<UniTask> _tasks = new List<UniTask>();

            var from = new GridPosition();
            var to = new GridPosition();
            UniTask _lastTask;
            while (Find(out from, out to))
            {
                //await Shift(from, to);
                //await UniTask.DelayFrame(1);
                _tasks.Add(Shift(from, to));
                await UniTask.DelayFrame(2);
            }

            await UniTask.WhenAll(_tasks);
            //await lastTask;

            bool Find(out GridPosition _from, out GridPosition _to)
            {
                _from = GridPosition.Empty;
                _to = GridPosition.Empty;
                
                for (int i = _board.Row - 1; i >= 0; i--)
                {
                    for (int j = _board.Column - 1; j >= 0; j--)
                    {
                        if (_board[new GridPosition(i,j)].IsEmpty)
                        {
                            to = from = _board[new GridPosition(i,j)].Position;
                            while (_validator.IsEmptySlot(from = from.Up))
                            {
                                if (!_validator.IsSlot(from))
                                    break;
                            }
                        
                            if (_validator.IsSlot(from))
                            {
                                //lastTask = Shift(from, to);
                                // _tasks.Add(Shift(from, to));
                                // await UniTask.DelayFrame(1);
                                _from = from;
                                _to = to;
                                return true;
                                // await Shift(from, to);
                                // i = _board.Row;
                                // j = _board.Column;
                            }
                        }
                    }
                }

                
                return false;
            }

            // await lastTask;
            // await UniTask.WhenAll(_tasks);
        }
    }
}
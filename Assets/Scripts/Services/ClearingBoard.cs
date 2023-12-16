﻿using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using Match3.Context;
using Match3.Models;
using Match3.Services;

namespace Match3.General
{
    public interface IClearingSlots
    {
        void MatchExecute(Action callback);
        UniTask MatchExecuteAsync();
        UniTask AllExecuteAsync();
    }
    
    public class ClearingBoard : IClearingSlots
    {
        private readonly IBoardModel _board;
        private readonly IBoardService _boardService;

        public ClearingBoard(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _boardService = context.Resolve<IBoardService>();
        }
        public void MatchExecute(Action callback)
        {
            foreach (var slot in _board.Slots)
            {
                if(slot.IsMatch) 
                    _boardService.CleanSlot(slot);
            }
            
            callback?.Invoke();
        }

        public async UniTask MatchExecuteAsync()
        {
            List<UniTask> _tasks = new List<UniTask>();
            foreach (var slot in _board.Slots)
            {
                if (slot.IsMatch)
                {
                    var task = _boardService.CleanSlot(slot);
                    _tasks.Add(task);
                }
            }
            await UniTask.WhenAll(_tasks);
        }

        public async UniTask AllExecuteAsync()
        {
            List<UniTask> _tasks = new List<UniTask>();
            foreach (var slot in _board.Slots)
            {
                if (!slot.IsEmpty)
                {
                    var task = _boardService.CleanSlot(slot);
                    _tasks.Add(task);
                }
            }

            await UniTask.WhenAll(_tasks);
        }
    }
}
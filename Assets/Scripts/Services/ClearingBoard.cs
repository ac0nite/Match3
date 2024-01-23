using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using Match3.Context;
using Match3.Models;
using Match3.Services;
using Unity.VisualScripting;
using UnityEngine;

namespace Match3.General
{
    public interface IClearing
    {
        void MatchExecute(Action callback);
        UniTask MatchExecuteAsync();
        UniTask AllExecuteAsync();
    }
    
    public class ClearingBoard : IClearing
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
            // Slot lastSlot = null;
            //
            // foreach (var slot in _board.Slots)
            // {
            //     if (lastSlot != null)
            //     {
            //         // UnityEngine.Debug.Log($"[X] run {lastSlot.Position}");
            //         // _boardService.CleanSlot(lastSlot);
            //         //lastSlot = null;
            //     }
            //     
            //     if (slot.IsMatch)
            //     {
            //         lastSlot = slot;
            //         
            //         // UnityEngine.Debug.Log($"[X] run {lastSlot.Position}");
            //         await _boardService.CleanSlot(slot);
            //         //await UniTask.Delay(1000);
            //     }
            // }
            //
            // UnityEngine.Debug.Log($"[X] last {lastSlot.Position}");
            
            // await _boardService.CleanSlot(lastSlot);
            
            // if (lastSlot != null)
            // {
            //     await _boardService.CleanSlot(lastSlot);
            // }



            List<UniTask> _tasks = new List<UniTask>();
            foreach (var slot in _board.Slots)
            {
                if (!slot.IsEmpty && slot.IsMatch)
                {
                    Debug.Log($"clean: {slot}");
                    _tasks.Add(_boardService.CleanSlot(slot));
                }
                
            }
            // await lastTask;
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
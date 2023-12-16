using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Debug;

namespace Common.Debug
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
            UnityEngine.Debug.Log($"[{Thread.CurrentThread.ManagedThreadId}] Clearing Execute begin");
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
            
            

            UniTask lastTask = new UniTask();
            foreach (var slot in _board.Slots)
            {
                if(slot.IsMatch) 
                    lastTask = _boardService.CleanSlot(slot);
                
            }

            await lastTask;
            
            UnityEngine.Debug.Log($"[{Thread.CurrentThread.ManagedThreadId}] ExecuteAsync end");
        }

        public async UniTask AllExecuteAsync()
        {
            foreach (var slot in _board.Slots)
            {
                if(!slot.IsEmpty)
                    await _boardService.CleanSlot(slot);
            }
        }
    }
}
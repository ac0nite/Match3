using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Debug;

namespace Common.Debug
{
    public interface ICleaningBoard
    {
        void Execute(Action callback);
        UniTask ExecuteAsync();
    }
    
    public class ClearingBoard : ICleaningBoard
    {
        private readonly IBoardModel _board;
        private readonly IBoardService _boardService;

        public ClearingBoard(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _boardService = context.Resolve<IBoardService>();
        }
        public void Execute(Action callback)
        {
            foreach (var slot in _board.Slots)
            {
                if(slot.IsMatch) 
                    _boardService.CleanSlot(slot);
            }
            
            callback?.Invoke();
        }

        public async UniTask ExecuteAsync()
        {
            UnityEngine.Debug.Log($"[{Thread.CurrentThread.ManagedThreadId}] Clearing Execute begin");
            Slot lastSlot = null;
            foreach (var slot in _board.Slots)
            {
                if (lastSlot != null)
                    _boardService.CleanSlot(lastSlot);
                
                if (slot.IsMatch)
                {
                    lastSlot = slot;
                    //await _boardService.CleanSlot(slot);
                    //await UniTask.Delay(1000);
                }
            }

            if (lastSlot != null)
            {
                await _boardService.CleanSlot(lastSlot);
            }
            
            UnityEngine.Debug.Log($"[{Thread.CurrentThread.ManagedThreadId}] ExecuteAsync end");
        }
    }
}
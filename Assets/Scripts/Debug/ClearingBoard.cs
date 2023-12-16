using System;
using Debug;

namespace Common.Debug
{
    public interface ICleaningBoard
    {
        void Execute(Action callback);
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
    }
}
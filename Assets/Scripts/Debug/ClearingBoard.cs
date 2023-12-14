using System;

namespace Common.Debug
{
    public interface IClearingBoard
    {
        void Clean(Action callback);
    }
    
    public class ClearingBoard : IClearingBoard
    {
        private readonly MatchGame _matchGame;

        public ClearingBoard(MatchGame matchGame)
        {
            _matchGame = matchGame;
        }
        public void Clean(Action callback)
        {
            foreach (var slot in _matchGame.BoardSlot)
            {
                if(slot.IsMatch) 
                    _matchGame.CleanSlot(slot);
            }
            
            callback?.Invoke();
        }
    }
}
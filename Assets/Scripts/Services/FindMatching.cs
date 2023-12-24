using System;
using System.Linq;
using Match3.Context;
using Match3.Models;

namespace Match3.General
{
    public interface IMatching
    {
        bool Find();
    }

    public interface ICheckResult
    {
        event Action RoundCompletedEvent;
        void Check();
    }
    public class FindMatching : IMatching, ICheckResult
    {
        private readonly IBoardModel _board;
        private readonly IValidator _validator;

        bool isMatch = false;

        public event Action RoundCompletedEvent;

        public FindMatching(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _validator = context.Resolve<IValidator>();
        }
        
        public bool Find()
        {
            isMatch = false;
            for (int i = 0; i < _board.Capacity; i++)
            {
                TryToFindAndMark(_board.Slots[i]);
            }
            return isMatch;
        }

        private void TryToFindAndMark(Slot slot)
        {
            if (IsMatch3(slot, slot.Position.Left, slot.Position.Right))
            {
                MarkAsMatch(slot.Position.Left);
                MarkAsMatch(slot.Position.Right);
                slot.IsMatch = isMatch = true;
            }
            
            if (IsMatch3(slot, slot.Position.Up, slot.Position.Down))
            {
                MarkAsMatch(slot.Position.Up);
                MarkAsMatch(slot.Position.Down);
                slot.IsMatch = isMatch = true;
            }
        }

        private void MarkAsMatch(GridPosition position)
        {
            _board[position].IsMatch = true;
        }

        private int Index(GridPosition position) => position.ColumnIndex * _board.Row + position.RowIndex;
        private bool IsMatch3(Slot slot, GridPosition neighborOne, GridPosition neighborTwo)
        {
            if (_validator.IsEmptySlot(neighborOne) || _validator.IsEmptySlot(neighborTwo))
                return false;
            
            if (slot.Match(_board[neighborOne]) && slot.Match(_board[neighborTwo]))
                return true;

            return false;
        }
        
        public void Check()
        {
            var sum = _board.Counter.Sum(c => c.Value);
            var isAllCounter = _board.Counter.All(c => c.Value < 3);
            UnityEngine.Debug.Log($"CheckResult! Sum:{sum} IsAllCounter:{isAllCounter}");
            
            if(sum < 3) RoundCompletedEvent?.Invoke();

            
            if(isAllCounter)
                RoundCompletedEvent?.Invoke();
        }
    }
}
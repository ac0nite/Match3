using System.Collections.Generic;
using System.Linq;

namespace Common.Debug
{
    public interface IMatchingStrategy
    {
        List<List<GridPosition>> FindMatches();
    }
    
    public class GeneralMatching : IMatchingStrategy
    {
        private readonly MatchGame _match;
        private readonly int _column;
        private readonly int _row;
        private readonly Slot[,] _slots;

        public GeneralMatching(MatchGame matchGame)
        {
            _match = matchGame;
            _slots = _match.BoardSlot;
            _row = _match.BoardSlot.GetLength(0);
            _column = _match.BoardSlot.GetLength(1);
        }

        public List<List<GridPosition>> FindMatches()
        {
            List<List<GridPosition>> matches = new List<List<GridPosition>>();

            for (int row = 0; row < _row; row++)
            {
                for (int col = 0; col < _column; col++)
                {
                    List<GridPosition> horizontalMatch = FindHorizontalMatch(row, col);
                    List<GridPosition> verticalMatch = FindVerticalMatch(row, col);

                    if (horizontalMatch.Count >= 3)
                        AddMatch(matches, horizontalMatch);

                    if (verticalMatch.Count >= 3)
                        AddMatch(matches, verticalMatch);
                }
            }
            
            foreach (var positions in matches)
            {
                UnityEngine.Debug.Log($"----------");
                foreach (var position in positions)
                {
                    UnityEngine.Debug.Log($"{position}", _slots[position.RowIndex, position.ColumnIndex]);
                    _slots[position.RowIndex, position.ColumnIndex].Tile.SetActive(false);
                }
            }

            return matches;
        }

        private List<GridPosition> FindHorizontalMatch(int row, int col)
        {
            List<GridPosition> match = new List<GridPosition>();
            match.Add(_slots[row, col].Position);

            if (col < _column - 1 && _slots[row, col].Match(_slots[row, col + 1]))
            {
                match.AddRange(FindHorizontalMatch(row, col + 1));
            }

            return match;
        }
        
        private List<GridPosition> FindVerticalMatch(int row, int col)
        {
            List<GridPosition> match = new List<GridPosition>();
            match.Add(_slots[row, col].Position);

            if (row < _row - 1 && _slots[row, col].Match(_slots[row + 1, col]))
            {
                match.AddRange(FindVerticalMatch(row + 1, col));
            }

            return match;
        }
        
        private void AddMatch(List<List<GridPosition>> matches, List<GridPosition> newMatch)
        {
            if (!matches.Any(match => match.Intersect(newMatch).Any()))
            {
                matches.Add(newMatch);
            }
        }
    }
}
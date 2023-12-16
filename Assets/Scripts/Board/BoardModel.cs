using System.Collections.Generic;
using Match3.General;

namespace Match3.Models
{
    public interface IBoardModel
    {
        void Initialise(int row, int column);
        Slot[,] Slots { get; }
        int Row { get; }
        int Column { get; }
        IDictionary<string, int> Counter { get; }
    }

    public class BoardModel : IBoardModel
    {
        public void Initialise(int row, int column)
        {
            Slots = new Slot[row, column];
            Row = row;
            Column = column;
            Counter = new Dictionary<string, int>();
        }

        public Slot[,] Slots { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public IDictionary<string, int> Counter { get; private set; }
    }
}
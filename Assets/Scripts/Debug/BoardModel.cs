using Common.Debug;

namespace Debug
{
    public interface IBoardModel
    {
        void Initialise(int row, int column);
        Slot[,] Slots { get; }
        int Row { get; }
        int Column { get; }
    }

    public class BoardModel : IBoardModel
    {
        public void Initialise(int row, int column)
        {
            Slots = new Slot[row, column];
            Row = row;
            Column = column;
        }

        public Slot[,] Slots { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
    }
}
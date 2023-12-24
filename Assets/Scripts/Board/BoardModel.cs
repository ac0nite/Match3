using System;
using System.Collections.Generic;
using ID;
using Match3.General;

namespace Match3.Models
{
    public interface IBoardModel
    {
        void Initialise(int row, int column);
        Slot[] Slots { get; }
        int Row { get; }
        int Column { get; }
        int Capacity { get; }
        IDictionary<UniqueID, int> Counter { get; }
        Slot this[GridPosition position] { get; set; }
    }

    public class BoardModel : IBoardModel
    {
        public void Initialise(int row, int column)
        {
            Slots = new Slot[row * column];
            Row = row;
            Column = column;
            Capacity = row * column;
            Counter = new Dictionary<UniqueID, int>();
        }

        public Slot[] Slots { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public int Capacity { get; private set; }
        private int GetIndex(GridPosition position) => position.RowIndex * Column + position.ColumnIndex;
        public IDictionary<UniqueID, int> Counter { get; private set; }

        public Slot this[GridPosition position]
        {
            get => Slots[GetIndex(position)];
            set => Slots[GetIndex(position)] = value;
        }
    }
}
using System.Collections.Generic;
using Board;
using ID;
using Match3.Context;
using Match3.General;

namespace Match3.Models
{
    public interface IBoardModel
    {
        void Initialise(BoardSize size);
        Slot[] Slots { get; }
        BoardSize Size { get; }
        IDictionary<UniqueID, int> Counter { get; }
        Slot this[GridPosition position] { get; set; }
    }

    public class BoardModel : IBoardModel
    {
        public BoardModel(ApplicationContext _)
        { }
        public void Initialise(BoardSize size)
        {
            Slots = new Slot[size.Capacity];
            Size = size;
            Counter ??= new Dictionary<UniqueID, int>();
            Counter.Clear();
        }

        public Slot[] Slots { get; private set; }

        public BoardSize Size { get; private set; }
        private int GetIndex(GridPosition position) => position.RowIndex * Size.Column + position.ColumnIndex;
        public IDictionary<UniqueID, int> Counter { get; private set; }

        public Slot this[GridPosition position]
        {
            get => Slots[GetIndex(position)];
            set => Slots[GetIndex(position)] = value;
        }
    }
}
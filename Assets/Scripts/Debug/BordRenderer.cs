using Common;
using Common.Debug;

namespace Debug
{
    public interface IBoardRenderer
    {
        void Create();
        void Random();
    }
    
    public class BordRenderer : IBoardRenderer
    {
        private readonly IPool<Slot> _slotPool;
        private readonly IPool<Tile> _tilePool;
        private readonly IBoardService _boardService;
        private readonly IBoardModel _board;
        private readonly Settings _settings;

        public BordRenderer(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _slotPool = context.Resolve<IPool<Slot>>();
            _tilePool = context.Resolve<IPool<Tile>>();
            _boardService = context.Resolve<IBoardService>();
            _settings = context.Settings;
        }

        public void Create()
        {
            _boardService.Initialise(_settings.BoardParam);
            int sortingOrder = _settings.BoardParam.Capacity;
            for (int i = 0; i < _board.Row; i++)
            {
                for (int j = 0; j < _board.Column; j++)
                {
                    var slot = _slotPool.Get();
                    slot.SetGridPosition(new GridPosition(i,j, _boardService.OrderLayer(i,j)));
                    slot.SetForceWorldPosition(_boardService.GetWorldPosition(i,j));
                    slot.SetActive(true);
                    _board.Slots[i, j] = slot;
                    //UnityEngine.Debug.Log($"{slot.Position} [{i},{j}]{order(i,j)}", slot);
                }
            }

            //int order(int i, int j) => _board.Row * _board.Column - _board.Column * (i + 1) + 1 + j;

            // int sortingOrder = 0;
            // for (int i = _board.Row - 1; i >= 0; i--)
            // {
            //     for (int j = _board.Column - 1; j >= 0; j--)
            //     {
            //         var slot = _slotPool.Get();
            //         slot.SetGridPosition(new GridPosition(i,j, ++sortingOrder));
            //         slot.SetForceWorldPosition(_boardService.GetWorldPosition(i,j));
            //         slot.SetActive(true);
            //         _board.Slots[i, j] = slot;
            //         
            //         UnityEngine.Debug.Log($"{slot.Position}", slot);
            //     }
            // }
        }

        public void Random()
        {
            Slot slot;
            for (int i = 0; i < _board.Row; i++)
            {
                for (int j = 0; j < _board.Column; j++)
                {
                    // var slot = _slotPool.Get();
                    // slot.SetGridPosition(new GridPosition(i,j, _board.Row - i));
                    // slot.SetForceWorldPosition(_boardService.GetWorldPosition(i,j));
                    // slot.SetTile(GetRandomTile());
                    // slot.SetActive(true);
                    // _board.Slots[i, j].SetTile(GetRandomTile());
                    
                    slot = _board.Slots[i, j];
                    slot.SetTile(GetRandomTile());
                    slot.SetActive(true);
                }
            }
        }
        
        private Tile GetRandomTile()
        {
            var index = UnityEngine.Random.Range(0, _settings.SpriteModels.Length);
            return _tilePool.Get().Initialise(_settings.SpriteModels[index]);
        }
    }
}
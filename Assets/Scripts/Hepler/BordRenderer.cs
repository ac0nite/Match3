using System;
using Common;
using Common.Debug;
using Match3.Board;

namespace Debug
{
    public interface IBoardRenderer
    {
        void Create();
        void RendererConfig();
        void RendererRandom();
        void Clear();
    }
    
    public class BordRenderer : IBoardRenderer
    {
        private readonly IPool<Slot> _slotPool;
        private readonly IPool<Tile> _tilePool;
        private readonly IBoardService _boardService;
        private readonly IBoardModel _board;
        private readonly Settings _settings;

        private int _roundCounter;

        public BordRenderer(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _slotPool = context.Resolve<IPool<Slot>>();
            _tilePool = context.Resolve<IPool<Tile>>();
            _boardService = context.Resolve<IBoardService>();
            _settings = context.Settings;
            _roundCounter = 0;
        }

        public void Create()
        {
            _boardService.Initialise(_settings.BoardParam);
            for (int i = 0; i < _board.Row; i++)
            {
                for (int j = 0; j < _board.Column; j++)
                {
                    var slot = _slotPool.Get();
                    slot.SetGridPosition(new GridPosition(i,j, _boardService.OrderLayer(i,j)));
                    slot.SetForceWorldPosition(_boardService.GetWorldPosition(i,j));
                    slot.SetActive(true);
                    _board.Slots[i, j] = slot;
                }
            }
        }

        public void RendererConfig()
        {
            if (_roundCounter >= _settings.BoardConfig.Param.Count)
            {
                RendererRandom();
                return;
            }

            UnityEngine.Debug.Log($"BOARD CONFIG");
            var t = _settings.BoardConfig.Param[_roundCounter];
            var details = BoardConverter.ToDetails(_settings.BoardConfig.Param[_roundCounter].text);
            
            foreach (SlotDetails param in details.slots)
            {
                if(param.id == String.Empty)
                    continue;
                
                var tile = GetTile(param.id);
                var slot = _board.Slots[(int)param.grid.x, (int)param.grid.y];
                slot.SetTile(tile);
                slot.SetActive(true);
                    
                InitialiseBoardCounter(tile);
            }
        }

        public void RendererRandom()
        {
            UnityEngine.Debug.Log($"BOARD RANDOM");
            for (int i = 0; i < _board.Row; i++)
            {
                for (int j = 0; j < _board.Column; j++)
                {
                    var slot = _board.Slots[i, j];
                    var tile = GetRandomTile();
                    slot.SetTile(tile);
                    slot.SetActive(true);
                    
                    InitialiseBoardCounter(tile);
                }
            }
        }

        public void Clear()
        {
            foreach (Slot boardSlot in _board.Slots)
                _slotPool.Put(boardSlot);

            _roundCounter++;
        }

        private Tile GetTile(string ID)
        {
            var param = Array.Find(_settings.SpriteModels, v => v.Id == ID);
            return _tilePool.Get().Initialise(param);
        }
        private Tile GetRandomTile()
        {
            var index = UnityEngine.Random.Range(0, _settings.SpriteModels.Length);
            var tile = _settings.SpriteModels[index];

            return _tilePool.Get().Initialise(tile);
        }

        private void InitialiseBoardCounter(Tile tile)
        {
            if (_board.Counter.ContainsKey(tile.ID))
                _board.Counter[tile.ID]++;
            else
                _board.Counter.Add(tile.ID, 1);
        }
    }
}
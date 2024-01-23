using System;
using Board.Config;
using Common;
using ID;
using Match3.Board;
using Match3.Context;
using Match3.Models;
using Match3.Services;
using UnityEngine;

namespace Match3.General
{
    public interface IBoardRenderer
    {
        void Create();
        void RendererConfig();
        void RendererRandom();
        void Clear();
    }
    
    public class BoardRenderer : IBoardRenderer
    {
        private readonly IPool<Slot> _slotPool;
        private readonly IBoardService _boardService;
        private readonly IBoardModel _board;
        private readonly Settings _settings;

        private int _roundCounter;
        private readonly IUpdateBoard _updateBoard;
        private readonly IBoardConfigs _boardConfigs;

        public BoardRenderer(ApplicationContext context)
        {
            _board = context.Resolve<IBoardModel>();
            _slotPool = context.Resolve<IPool<Slot>>();
            _boardService = context.Resolve<IBoardService>();
            _updateBoard = context.Resolve<IUpdateBoard>();
            _settings = context.Settings;
            _boardConfigs = context.Resolve<IBoardConfigs>();
            _roundCounter = 0;
        }

        public void Create()
        {
            _boardConfigs.Update();
            _boardService.Initialise(_settings.boardSettings, _boardConfigs.SlotsConfig.Size);
            
            for (int i = 0; i < _board.Size.Row; i++)
            {
                for (int j = 0; j < _board.Size.Column; j++)
                {
                    _board.Slots[i * _board.Size.Column + j] = _slotPool.Get().Configure(_boardService, i, j);
                }
            }
        }

        public void RendererConfig()
        {
            // var boardDeSerialize = BoardConverter.ToDetails(_settings.boardSerializeConfigs.Param[0].text);
            // RendererRandom();
            
            foreach (var slot in _boardConfigs.SlotsConfig.Slots)
            {
                var tile = GetModel(new UniqueID(slot.Key));
                foreach (GridPosition position in slot.Value)
                {
                    _board[position].Initialise(tile);
                    InitialiseBoardCounter(tile);
                }
            }
            
            foreach (var pair in _board.Counter)
            {
                Debug.Log($"[{pair.Key}]:{pair.Value}");
            }

            // if (_roundCounter >= _settings.BoardConfig.Param.Count)
            // {
            //     RendererRandom();
            //     return;
            // }
            //
            // UnityEngine.Debug.Log($"BOARD CONFIG");
            // var t = _settings.BoardConfig.Param[_roundCounter];
            // var details = BoardConverter.ToDetails(_settings.BoardConfig.Param[_roundCounter].text);
            //
            // foreach (SlotDetails param in details.slots)
            // {
            //     if(param.id == String.Empty)
            //         continue;
            //     
            //     var tile = GetTile(param.id);
            //     var slot = _board.Slots[(int)param.grid.x, (int)param.grid.y];
            //     slot.Initialise(tile);
            //
            //     InitialiseBoardCounter(tile);
            // }
            //
            // _updateBoard.UpdateAsync();
        }

        public void RendererRandom()
        {
            // UnityEngine.Debug.Log($"BOARD RANDOM");
            // for (int i = 0; i < _board.Size.Capacity; i++)
            // {
            //     var slot = _board.Slots[i];
            //     var model = GetRandomModel();
            //     slot.Initialise(model);
            //
            //     InitialiseBoardCounter(model);
            // }
            // for (int i = 0; i < _board.Row; i++)
            // {
            //     for (int j = 0; j < _board.Column; j++)
            //     {
            //         var slot = _board.Slots[i, j];
            //         var model = GetRandomModel();
            //         slot.Initialise(model);
            //
            //         InitialiseBoardCounter(model);
            //     }
            // }

            //Save();
            //_updateBoard.UpdateAsync();
        }

        private void Save()
        {
            var json = BoardConverter.ToJson(_board);
            Debug.Log($"S: {json}");
            // string filepath = System.IO.Path.Combine("Assets//settings", $"Board_{DateTime.Now.ToString("hmmsstt")}.json");
            // File.WriteAllText(filepath, json);
            // UnityEditor.AssetDatabase.SaveAssets();
            // UnityEngine.Debug.Log($"Save to: {filepath}");
        }

        public void Clear()
        {
            foreach (Slot boardSlot in _board.Slots)
                _slotPool.Put(boardSlot);

            _roundCounter++;
        }
        private TileModel GetRandomModel()
        {
            var index = UnityEngine.Random.Range(0, _settings.SpriteModels.Length);
            return _settings.SpriteModels[index];
        }

        private TileModel GetModel(UniqueID id)
        {
            return Array.Find(_settings.SpriteModels, model => model.ID == id);
        }

        private void InitialiseBoardCounter(TileModel tile)
        {
            Debug.Log($"{tile.ID.Value}");
            if (_board.Counter.ContainsKey(tile.ID))
                _board.Counter[tile.ID]++;
            else
                _board.Counter.Add(tile.ID, 1);
        }
    }
}
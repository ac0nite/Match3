using System;
using System.Collections.Generic;
using System.Linq;
using ID;
using Match3.Board;
using Match3.Context;
using Match3.General;
using UnityEngine;
using Random = System.Random;

namespace Board.Config
{
    public interface IBoardConfigs
    {
        void Update();
        BoardConfig SlotsConfig { get; }
    }
    public class BoardConfigs : IBoardConfigs
    {
        private readonly Match3.Context.Settings _settings;
        private readonly IScoreModel _score;
        private readonly List<TextAsset> _configs;

        public BoardConfigs(ApplicationContext context)
        {
            _settings = context.Settings;
            _score = context.Resolve<IScoreModel>();
            _configs = _settings.boardSerializeConfigs.Param;
        }
        
        public BoardConfig SlotsConfig { get; private set; }

        public void Update()
        {
            if (_score.Round < _configs.Count)
                SlotsConfig = BoardConverter.ToDetails(_configs[_score.Round].text);
            else
                SlotsConfig = ConfigRandom();
        }

        private BoardConfig ConfigRandom()
        {
            BoardConfig config = new BoardConfig();
            config.Size = new BoardSize()
            {
                Row = 4,
                Column = 4
            };

            var count = config.Size.Capacity;
            var slots = config.Slots = new Dictionary<string, GridPosition[]>();
            var ids = _settings.SpriteModels.Select(model => model.ID);

            GridPosition[] positions = new GridPosition[count];
            for (int i = 0; i < config.Size.Row; i++)
            {
                for (int j = 0; j < config.Size.Column; j++)
                {
                    positions[i * config.Size.Column + j] = new GridPosition(i, j);
                }   
            }
            
            List<int> list1 = new List<int>() { 1, 2, 3, 4, 5 };

            var rnd = new Random();
            var randomized = list1.OrderBy(item => rnd.Next());

            // foreach (var value in randomized)
            // {
            //     Console.WriteLine(value);
            // }

            /*
             numbers = numbers.Select(x => new { Number = x, Order = random.Next() })
                         .OrderBy(x => x.Order)
                         .Select(x => x.Number)
                         .ToArray();
             */

            var random = new System.Random();
            positions = positions.OrderBy(x => Guid.NewGuid()).ToArray();

            // positions = positions.Select(x => new {Number = x, Order = random.Next()})
            //     .OrderBy(x => x.Order)
            //     .Select(x => x.Number)
            //     .ToArray();

            slots[ids.ElementAt(0)] = positions.Take(count / 2).ToArray();
            slots[ids.ElementAt(1)] = positions.Skip(count / 2).ToArray();
            
            return config;
        }
    }
}
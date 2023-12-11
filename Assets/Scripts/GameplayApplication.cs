using System;
using UnityEngine;

namespace Common
{
    public class GameplayApplication : MonoBehaviour
    {
        
    }

    public interface IGameState
    {
        public void Run();
        event Action Completed;
    }

    public class GameplayInitState : IGameState
    {
        private readonly IGameplayBoardRenderer _boardRenderer;

        public GameplayInitState(ApplicationContext context)
        {
            _boardRenderer = context.Resolve<IGameplayBoardRenderer>();
        }
        
        public void Run()
        {
            
        }

        public event Action Completed;
    }
}
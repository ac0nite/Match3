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
}
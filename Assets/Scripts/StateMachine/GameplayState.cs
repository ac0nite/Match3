using Common;
using Match3.Context;
using Match3.Environment;
using Match3.General;
using Match3.Screen;

namespace Match3.StateMachine
{
    public class GameplayState : BaseState
    {
        private readonly IGameplay _gameplay;
        private readonly ICheckResult _checkResult;
        private readonly IStateMachine<BaseState> _stateMachine;
        private readonly GameplayScreen _gameplayScreen;
        private readonly IAnimationEnvironment _animationEnvironment;

        public GameplayState(ApplicationContext context) : base(context)
        {
            _gameplay = context.Resolve<IGameplay>();
            _checkResult = context.Resolve<ICheckResult>();
            _stateMachine = context.Resolve<IStateMachine<BaseState>>();
            _gameplayScreen = context.Resolve<IScreenService>().Get<GameplayScreen>();
            _animationEnvironment = context.Resolve<IAnimationEnvironment>();
        }

        public override void Enter()
        {
            UnityEngine.Debug.Log($"GAMEPLAY STATE");
            
            _checkResult.RoundCompletedEvent += RoundCompleted;
            _gameplayScreen.EndButtonPressedEvent += RoundCompleted;
            
            _gameplay.SetActive(true);
            _gameplayScreen.Show();
            _animationEnvironment.Play();
        }

        private void RoundCompleted()
        {
            _stateMachine.Next<ResetState>();
        }

        public override void Exit()
        {
            _gameplay.SetActive(false);
            _gameplayScreen.Hide();
            _animationEnvironment.Stop();
            
            _checkResult.RoundCompletedEvent -= RoundCompleted;
            _gameplayScreen.EndButtonPressedEvent -= RoundCompleted;
        }
    }
}
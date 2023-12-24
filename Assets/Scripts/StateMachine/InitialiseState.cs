using Match3.Context;
using Match3.General;

namespace Match3.StateMachine
{
    public class InitialiseState : BaseState
    {
        private readonly IBoardRenderer _boardRenderer;
        private readonly IStateMachine<BaseState> _stateMachine;

        public InitialiseState(ApplicationContext context) : base(context)
        {
            _boardRenderer = context.Resolve<IBoardRenderer>();
            _stateMachine = context.Resolve<IStateMachine<BaseState>>();
        }

        public override void Enter()
        {
            UnityEngine.Debug.Log($"INITIALISE STATE");
            
            _boardRenderer.Create();
            _boardRenderer.RendererRandom();
            // _boardRenderer.RendererConfig();
            //
            _stateMachine.Next<GameplayState>();
        }

        public override void Exit()
        {
        }
    }
}
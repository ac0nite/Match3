using Cysharp.Threading.Tasks;
using Match3.Context;
using Match3.General;

namespace Match3.StateMachine
{
    public class ResetState : BaseState
    {
        private readonly IBoardRenderer _boardRenderer;
        private readonly IStateMachine<BaseState> _stateMachine;
        private readonly IClearingSlots _clearing;

        public ResetState(ApplicationContext context) : base(context)
        {
            _clearing = context.Resolve<IClearingSlots>();
            _boardRenderer = context.Resolve<IBoardRenderer>();
            _stateMachine = context.Resolve<IStateMachine<BaseState>>();
        }

        public override void Enter()
        {
            UnityEngine.Debug.Log($"RESET STATE");
            CleanBoardAsync();
        }

        public override void Exit()
        {
        }

        private async void CleanBoardAsync()
        {
            await UniTask.Delay(200);
            await _clearing.AllExecuteAsync();
            _boardRenderer.Clear();

            _stateMachine.Next<InitialiseState>();
        }
    }
}
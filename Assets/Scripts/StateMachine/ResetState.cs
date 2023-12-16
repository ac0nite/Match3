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
            UnityEngine.Debug.Log($"CleanBoardAsync begin");
            
            await UniTask.Delay(1000);
            await _clearing.AllExecuteAsync();
            
            UnityEngine.Debug.Log($"boardRenderer.Clear");
            _boardRenderer.Clear();
            await UniTask.DelayFrame(120);
            
            UnityEngine.Debug.Log($"CleanBoardAsync end");
            
            _stateMachine.Next<InitialiseState>();
        }
    }
}
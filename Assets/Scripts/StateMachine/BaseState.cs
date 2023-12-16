using Match3.Context;

namespace Match3.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
    
    
    public abstract class BaseState : IState
    {
        protected readonly ApplicationContext _context;
        protected BaseState(ApplicationContext context)
        {
            _context = context;
        }
        public abstract void Enter();
        public abstract void Exit();
    }
}
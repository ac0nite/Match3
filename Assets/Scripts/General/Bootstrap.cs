using Match3.Context;
using Match3.StateMachine;
using UnityEngine;

namespace Match3.General
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private ApplicationContext _applicationContext;
        private IStateMachine<BaseState> _stateMachine;

        private void Awake()
        {
            _applicationContext.Construct();
        }

        private void Start()
        {
            _applicationContext.Build();
            
            _stateMachine = _applicationContext.Resolve<IStateMachine<BaseState>>();
            _stateMachine.Next<InitialiseState>();
        }

        private void OnDestroy()
        {
            _applicationContext.Dispose();
        }
    }
}
namespace Entities.StateMachine
{
    public class FSM<T>
    {
        IState<T> _currentState;
        private T _currentStateEnum;
        private T _lastStateEnum;
        public FSM() { }
        public void SetInit(IState<T> curr,T initEnum)
        {
            curr.StateMachine = this;
            _currentState = curr;
            _currentStateEnum = initEnum;
            _lastStateEnum = _currentStateEnum;
            _currentState.Enter();
        }
        public void OnExecute() => _currentState?.Execute();
        public void OnFixedExecute() => _currentState?.FixedExecute();
        public void Transition(T input)
        {
            IState<T> newState = _currentState.GetTransition(input);
            if (newState == null) return;
            _lastStateEnum = _currentStateEnum;
            _currentStateEnum = input;
            newState.StateMachine = this;
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
        public IState<T> CurrentState()
        {
            return _currentState;
        }

        public T CurrentStateEnum() => _currentStateEnum;

        public T LastStateEnum() => _lastStateEnum;
    }
}

using System.Collections.Generic;
using strange.extensions.injector.api;

namespace Framewerk.AppStateMachine
{
    /// <summary>
    /// _fsm a.k.a Finite State Machine is class responsible for storing main state of the game 
    /// and handling transitions from one state to another. It also notifies <see cref="EventRouter"/> 
    /// that new state transition is finished so it can dispatch waiting events to it.
    /// </summary>
    public enum TransitionType
    {
        None,
        Enter,
        Exit
    }

    public interface IAppFsm
    {
        /// <summary>
        /// Returns current state.
        /// </summary>
        IAppState CurrentState { get; }

        /// <summary>
        /// Returns state of the transition.
        /// </summary>
        TransitionType CurrentTransition { get; }

	    /// <summary>
	    /// Switches current state for a new one. If fsm is already in some state, new state is queued.
	    /// First the current state is closed (transition out) then new state is opened (transition in).
	    /// </summary>
	    /// <param name="newState">New state to be opened</param>
	    void SwitchState(IAppState newState);

        void Destroy();
    }

    public class AppFsm : IAppFsm
    {
	    [Inject] public IInjectionBinder InjectionBinder { get; set; }
	    
	    private Queue<IAppState> _nextStates;
        private IAppState _currentState;
		private TransitionType _currentTransition;
	    
	    /// <summary>
	    /// Constructor of the fsm. _fsm is created with no state and with no active transition.
	    /// </summary>
	    public AppFsm()
		{
			_nextStates = new Queue<IAppState>();
			_currentTransition = TransitionType.None;
		}

        /// <summary>
        /// Returns current state.
        /// </summary>
        public IAppState CurrentState
		{
			get { return _currentState; }
		}

        /// <summary>
        /// Returns state of the transition.
        /// </summary>
		public TransitionType CurrentTransition
		{
			get { return _currentTransition; }
		}

	    /// <summary>
	    /// Switches current state for a new one. If fsm is already in some state, new state is queued.
	    /// First the current state is closed (transition out) then new state is opened (transition in).
	    /// </summary>
	    /// <param name="newState">New state to be opened</param>
	    public virtual void SwitchState(IAppState newState)
		{
			//Debug.LogWarningFormat("<color=\"aqua\">{0}.SwitchState : {1}</color>", this, newState.GetType());
			
			InjectionBinder.injector.Inject(newState);

		    if (_currentTransition == TransitionType.None)
			{
				if (_currentState == null)
				{
					StartState(newState);
				}
				else
				{
					_nextStates.Enqueue(newState);
					CloseCurrentState();
				}
			}
			else
			{
				_nextStates.Enqueue(newState);
			}
		}

        /// <summary>
        /// Finalizes the opening procedure of a state.
        /// </summary>
        private void EnterFinishedHandler()
		{
			_currentTransition = TransitionType.None;

			//if there is other state in queue immediately close current state to start new wehn transition is finished.
		    if (_nextStates.Count > 0)
			{
				CloseCurrentState();
			}
		}

        /// <summary>
        /// Finalizes the closing procedure of a state. 
        /// In conclusion it starts opening of the new state in state queue.
        /// </summary>
		private void ExitFinishedHandler()
		{
			_currentState.Destroy();
            _currentState = null;
			_currentTransition = TransitionType.None;

            if (_nextStates.Count == 0)
	            return;

			var nextState = _nextStates.Dequeue();
			StartState(nextState);
		}

        /// <summary>
        /// Opens a new state. Opening procedure is finished by calling SetTransitionInFinished.
        /// </summary>
        /// <param name="state">State to be opened.</param>
        protected virtual void StartState(IAppState state)
		{
			_currentState = state;
			_currentState.EnterFinishedSignal.AddOnce(EnterFinishedHandler);
			_currentState.ExitFinishedSignal.AddOnce(ExitFinishedHandler);

			_currentTransition = TransitionType.Enter;

			state.PerformEnter();
		}

        /// <summary>
        /// Starts the closing procedure of current state. Closing procedure us finished by
        /// calling SetTransitionOutFinished.
        /// </summary>
		private void CloseCurrentState()
		{
		    _currentTransition = TransitionType.Exit;

			_currentState.PerformExit();
		}

        public virtual void Destroy()
        {
            _nextStates.Clear();
            CloseCurrentState();
        }
	}
}

using strange.extensions.injector.api;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Framewerk.AppStateMachine
{
    /// <summary>
    /// Interface of Application State. All methods are called by Fsm
    /// </summary>
    public interface IAppState
    {
        Signal EnterFinishedSignal { get; }
        Signal ExitFinishedSignal { get; }
        
        /// <summary>
        /// Starts opening transition process of the state
        /// </summary>
        void PerformEnter();

        /// <summary>
        /// Starts closing transition process of the state
        /// </summary>
        void PerformExit();

        /// <summary>
        /// Destroys AppState
        /// </summary>
        void Destroy();
    }

    /// <summary>
    /// AppState class serves as both state definition and it's controller.
    /// State should represent one main state of the application which is usually consistent in terms of display e.g. loading, main menu, game, minigame...
    /// State is connected with ViewScreen which is aggregator of the view logic of the state.
    /// </summary>
    public class AppState<TScreen> : IAppState where TScreen : AppStateScreen
    {
        [Inject] public TScreen Screen { get; set; }

        public Signal EnterFinishedSignal { get; } = new Signal();
        public Signal ExitFinishedSignal { get; } = new Signal();

        public virtual void Destroy()
        {
            Screen.Destroy();
        }

        /// <summary>
        /// Enter state - Called by FSM
        /// </summary>
        public void PerformEnter()
        {
            RegisterHandlers();
            
            Enter();
            Screen.EnterFinishedSignal.AddOnce(EnterFinished);
            Screen.PerformEnter();
        }

        /// <summary>
        /// Exit state - Called by FSM
        /// </summary>
        public virtual void PerformExit()
        {
            Exit();
            Screen.ExitFinishedSignal.AddOnce(ExitFinished);
            Screen.PerformExit();
        }

        /// <summary>
        /// Override in concrete state to do enter state stuff
        /// </summary>
        protected virtual void Enter()
        {   
            
        }
        
        /// <summary>
        /// Override in concrete state to do exit state stuff
        /// </summary>
        protected virtual void Exit()
        {
            
        }
        
        /// <summary>
        /// Notifies fsm that opening transition is finished. 
        /// </summary>
        protected virtual void EnterFinished()
        {
            EnterFinishedSignal.Dispatch();
        }

        /// <summary>
        /// Notifies fsm that closing transition is finished. 
        /// </summary>
        protected virtual void ExitFinished()
        {
            UnregisterHandlers();
            
            ExitFinishedSignal.Dispatch();
        }

        /// <summary>
        /// Registers handlers for all view events.
        /// </summary>
        protected virtual void RegisterHandlers()
        {
            
        }

        /// <summary>
        /// Unregisters handlers for all view events.
        /// </summary>
        protected virtual void UnregisterHandlers()
        {
            
        }
    }
}
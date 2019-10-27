using System.Collections.Generic;
using CKIEditor;
using Framewerk.Managers;
using strange.extensions.mediation.api;
using strange.extensions.signal.impl;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framewerk.AppStateMachine
{
    public abstract class AppStateScreen
    {
        [Inject] public IAssetManager AssetManager { get; set; }
        [Inject] public ViewConfig ViewConfig { get; set; }
        [Inject] public IUiManager UiManager { get; set; }
        
        public readonly Signal EnterFinishedSignal = new Signal();
        public readonly Signal ExitFinishedSignal = new Signal();

        protected TransitionType TransitionType;

        private bool _holdEnter;
        private bool _holdExit;

        private List<GameObject> _views = new List<GameObject>();
        
        #region Instantiating UI / Game Prefabs

        public GameObject InstantiateView(string path = "", Transform parent = null)
        {
            var view = UiManager.InstantiateView(path);
            _views.Add(view);

            return view;
        }

        public T InstantiateView<T>(string path = "", Transform parent = null) where T : MonoBehaviour, IView
        {
            var view = UiManager.InstantiateView<T>(path, parent);
            _views.Add(view.gameObject);

            return view;
        }
        
        #endregion

        #region FSM API

        public void PerformEnter()
        {
            TransitionType = TransitionType.Enter;
            Enter();

            if (!_holdEnter)
                EnterFinished();
        }

        public void PerformExit()
        {
            TransitionType = TransitionType.Exit;
            Exit();

            if (!_holdExit)
                ExitFinished();
        }

        public virtual void Destroy()
        {
            foreach (var gameObject in _views)
            {
                Object.Destroy(gameObject);
            }
        }

        #endregion

        #region Life cycle

        protected virtual void Enter()
        {

        }

        protected virtual void Exit()
        {

        }

        protected void Hold()
        {
            if (TransitionType == TransitionType.Enter)
                _holdEnter = true;
            else if (TransitionType == TransitionType.Exit)
                _holdExit = true;
        }

        protected void Release()
        {
            if (TransitionType == TransitionType.Enter && _holdEnter)
                EnterFinished();
            else if (TransitionType == TransitionType.Exit && _holdExit)
                ExitFinished();
        }

        #endregion

        private void EnterFinished()
        {
            TransitionType = TransitionType.None;
            _holdEnter = false;
            EnterFinishedSignal.Dispatch();
        }

        private void ExitFinished()
        {
            TransitionType = TransitionType.None;
            _holdExit = false;
            ExitFinishedSignal.Dispatch();
        }
    }
}

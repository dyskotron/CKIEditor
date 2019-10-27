using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Framewerk.Managers{

    public static class CoroutineExtensions
    {
        public static void Stop(this Coroutine co)
        {
            CoroutineManager.Instance.StopCoroutine(co);
        }
    }

    public interface ICoroutineManager
    {
        Coroutine RunCoroutine(IEnumerator routine, MonoBehaviour mb = null);
        Coroutine DelayedCall(float secs, Action callBack, MonoBehaviour mb = null);
        Coroutine DelayedCallRealtime(float secs, Action callBack, MonoBehaviour mb = null);
        void KillCoroutine(Coroutine co);
        void KillAllCoroutines();
    }

    public class CoroutineManager : SingletonMono<CoroutineManager>, ICoroutineManager
    {
        public class CoroutineInstance
        {
            public WeakReference coroutine;
            public MonoBehaviour mb;
            public string id; //For debugging purpose

            public event Action<CoroutineInstance> OnRoutineFinished;
            public event Action<CoroutineInstance> OnRoutineStopped;

            public IEnumerator ContainerCoroutine(IEnumerator task)
            {
                while (task.MoveNext())
                {
                    yield return task.Current;
                }

                if (OnRoutineFinished != null)
                    OnRoutineFinished (this);

                ClearListeners ();
            }

            public void Stop()
            {
                //The StopCoroutine call can sometimes produce "Coroutine continue failure" error
                //which is harmless according to the Unity devs from this thread.
                //http://forum.unity3d.com/threads/new-added-stopcoroutine-coroutine-coroutine-override-function-causes-errors.287481/
                //This will probably be finally fixed in next versions

                if (coroutine.IsAlive)
                    mb.StopCoroutine (coroutine.Target as Coroutine);

                if (OnRoutineStopped != null)
                    OnRoutineStopped (this);

                ClearListeners ();
            }

            public void ClearListeners()
            {
                OnRoutineFinished = null;
                OnRoutineStopped = null;
                mb = null;
            }
        }


        private List<CoroutineInstance> activeRoutines = new List<CoroutineInstance>();

        public Coroutine RunCoroutine(IEnumerator routine, MonoBehaviour mb = null)
        {
            if (mb == null)
                mb = this;

            var ci = new CoroutineInstance ();
            ci.mb = mb;

            var cor = mb.StartCoroutine ( ci.ContainerCoroutine (routine) );
            ci.coroutine = new WeakReference( cor );
            if (ci.coroutine != null)
            {
                ci.OnRoutineFinished += OnCoroutineFinshed;
                ci.OnRoutineStopped += OnCoroutineFinshed;

                activeRoutines.Add ( ci );
            }

            return cor;
        }

        public Coroutine DelayedCall(float secs, Action callBack, MonoBehaviour mb = null)
        {
            return RunCoroutine(WaitingFor(secs, callBack), mb);
        }
        private static IEnumerator WaitingFor(float secs, Action callBack)
        {
            yield return new WaitForSeconds(secs);
            try
            {
                callBack();
            }
            catch (Exception ex)
            {
                //Logger.WriteError(ex, "DelayedActions.WaitingFor", String.Format("During {0} the error {1} happened.", id, ex.GetType().Name),LogOutputFlags.IncudeStackTrace);
                Debug.LogErrorFormat("DelayedActions.WaitingFor: {0}, {1}\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
            }
        }

        public Coroutine DelayedCallRealtime(float secs, Action callBack, MonoBehaviour mb = null)
        {
            return RunCoroutine(WaitingForRealtime(secs, callBack), mb);
        }
        private static IEnumerator WaitingForRealtime(float secs, Action callBack)
        {
            yield return new WaitForSecondsRealtime(secs);
            try
            {
                callBack();
            }
            catch (Exception ex)
            {
                //Logger.WriteError(ex, "DelayedActions.WaitingFor", String.Format("During {0} the error {1} happened.", id, ex.GetType().Name),LogOutputFlags.IncudeStackTrace);
                Debug.LogErrorFormat("DelayedActions.WaitingFor: {0}, {1}\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
            }
        }

        private void OnCoroutineFinshed(CoroutineInstance ci)
        {
            activeRoutines.Remove (ci);
            activeRoutines = activeRoutines.FindAll (x =>
            {
                var alive = x.coroutine.IsAlive;
                if ( !alive )
                    x.ClearListeners();

                return alive;
            });
        }

        public void KillCoroutine(Coroutine co)
        {
            var ci = GetBy(co);
            if (ci != null)
                ci.Stop ();
        }

        public CoroutineInstance GetBy(Coroutine coroutine)
        {
            return activeRoutines.Find (x => (x.coroutine.Target as Coroutine) == coroutine);
        }

        public void KillAllCoroutines()
        {
            activeRoutines.ForEach (x => x.Stop ());
        }

        protected override void SingletonMonoInit()
        {
            base.SingletonMonoInit();
            gameObject.name = "CoroutineManager";
        }
    }
    
}


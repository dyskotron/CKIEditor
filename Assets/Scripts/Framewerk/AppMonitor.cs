using UnityEngine.SceneManagement;

namespace Framewerk
{
    public interface IAppMonitor
    {
        bool AppPaused { get; }
    }

    public class AppMonitor : SingletonMono<AppMonitor>, IAppMonitor
    {
        private bool _appPaused;

        public void Start()
        {
            _appPaused = true;
        }

        public override void OnDestroy()
        {
            
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            _appPaused = pauseStatus;
            //Locator.EventDispatcher.DispatchEvent(new ApplicationPausedEvent(pauseStatus));
        }

        protected override void SingletonMonoInit()
        {
            base.SingletonMonoInit();
            gameObject.name = "AppMonitor";
        }

        public bool AppPaused
        {
            get { return _appPaused; }
        }
    }
}
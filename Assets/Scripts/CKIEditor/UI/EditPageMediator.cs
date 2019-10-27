using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.UI.MainMenu;
using Framewerk.UI;

namespace CKIEditor.UI
{
    public abstract class EditPageMediator : ExtendedMediator
    {
        [Inject] public AppSectionChangedSignal AppSectionChangedSignal { get; set; }
        [Inject] public IAppModel AppModel { get; set; }
        
        public abstract EditPage Page { get; }

        public override void OnRegister()
        {
            base.OnRegister();
            
            AppSectionChangedSignal.AddListener(AppSectionChangedHandler);
            
            UpdateVisibility(AppModel.selectedEditPage);        }

        public override void OnRemove()
        {
            base.OnRemove();
            
            AppSectionChangedSignal.RemoveListener(AppSectionChangedHandler);
        }
        
        private void AppSectionChangedHandler(EditPage section)
        {
            UpdateVisibility(section);
        }

        private void UpdateVisibility(EditPage section)
        {
            gameObject.SetActive(section == Page);
        }
    }
}
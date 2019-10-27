using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using Framewerk.UI;

namespace CKIEditor.UI.EditSection
{
    public abstract class EditInstrumentSectionMediator : ExtendedMediator
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
            EditedInstrumentChangedSignal.AddListener(EditedInstrumentChangedHandler);

            var editedInstrument = InstrumentsModel.GetEditedInstrument();
            
            if(editedInstrument != null)
                ShowInstrumentData(editedInstrument);
        }
        
        public override void OnRemove()
        {
            base.OnRemove();
            EditedInstrumentChangedSignal.RemoveListener(EditedInstrumentChangedHandler);
        }
        
        protected abstract void ShowInstrumentData(InstrumentDef instrumentDef);

        private void EditedInstrumentChangedHandler(InstrumentDef instrumentDef)
        {
            if (instrumentDef != null)
                ShowInstrumentData(instrumentDef);

        }
    }
}
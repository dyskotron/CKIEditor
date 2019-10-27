using System.Linq;
using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using CKIEditor.UI.TrackValues.CcList;
using Framewerk.UI.List;

namespace CKIEditor.UI.EditSection.CcEditor.CcList
{
    public class CcListMediator : ListMediator<CcListView, CcDef>
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        [Inject] public InstrumentCcDefsChangedSignal InstrumentCcDefsChangedSignal { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
            
            EditedInstrumentChangedSignal.AddListener(EditedInstrumentChangedHandler);
            InstrumentCcDefsChangedSignal.AddListener(InstrumentCcDefsChangedHandler);

            ShowEditedInstrument();

        }

        public override void OnRemove()
        {
            EditedInstrumentChangedSignal.RemoveListener(EditedInstrumentChangedHandler);
            InstrumentCcDefsChangedSignal.RemoveListener(InstrumentCcDefsChangedHandler);
            base.OnRemove();
        }

        private void EditedInstrumentChangedHandler(InstrumentDef instrumentDef)
        {
            ShowEditedInstrument();
        }
        
        private void InstrumentCcDefsChangedHandler()
        {
            ShowEditedInstrument();
        }

        private void ShowEditedInstrument()
        {
            var instrument = InstrumentsModel.GetEditedInstrument();
            if(instrument != null)
                SetData(InstrumentsModel.GetEditedInstrument().CcDefs.Values.ToList());    
        }
    }
}
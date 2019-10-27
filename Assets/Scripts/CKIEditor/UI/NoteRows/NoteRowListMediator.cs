using System.Linq;
using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using Framewerk.UI.List;

namespace CKIEditor.UI.NoteRows
{
    public class NoteRowListMediator : ListMediator<NoteRowListView, NoteRowDef>
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        [Inject] public InstrumentNoteRowDefsChangedSignal InstrumentNoteRowDefsChangedSignal { get; set; }   
        
        public override void OnRegister()
        {
            base.OnRegister();
            
            EditedInstrumentChangedSignal.AddListener(EditedInstrumentChangedHandler);
            InstrumentNoteRowDefsChangedSignal.AddListener(InstrumentNoteRowDefsChangedHandler);
            
            ShowEditedInstrument();
        }

        public override void OnRemove()
        {
            EditedInstrumentChangedSignal.RemoveListener(EditedInstrumentChangedHandler);
            InstrumentNoteRowDefsChangedSignal.RemoveListener(InstrumentNoteRowDefsChangedHandler);
            base.OnRemove();
        }

        private void ShowEditedInstrument()
        {
            var instrument = InstrumentsModel.GetEditedInstrument();
            if(instrument != null)
                SetData(instrument.NoteRowDefs.Values.ToList());      
        }
        
        private void InstrumentNoteRowDefsChangedHandler()
        {
            ShowEditedInstrument();
        }
        
        private void EditedInstrumentChangedHandler(InstrumentDef instrument)
        {
            ShowEditedInstrument();    
        }
    }
}
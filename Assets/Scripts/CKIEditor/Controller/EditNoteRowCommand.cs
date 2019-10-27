using CKIEditor.Model;
using CKIEditor.Model.Defs;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace CKIEditor.Controller
{
    //edited id, new note row
    public class EditNoteRowSignal : Signal<int, NoteRowDef>
    {
        
    }
    
    public class EditNoteRowCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        [Inject] public InstrumentNoteRowDefsChangedSignal InstrumentNoteRowDefsChangedSignal { get; set; }
        
        [Inject] public int EditedRowId { get; set; }
        [Inject] public NoteRowDef NewRowDef { get; set; }
        
        public override void Execute()
        {
            var instrument = InstrumentsModel.GetEditedInstrument();

            instrument.NoteRowDefs.Remove(EditedRowId);
            instrument.NoteRowDefs[NewRowDef.Note.Id] = NewRowDef;
            
            InstrumentNoteRowDefsChangedSignal.Dispatch();
        }
    }
}
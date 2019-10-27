using CKIEditor.Model;
using CKIEditor.Model.Defs;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace CKIEditor.Controller
{
    public class DeleteNoteRowSignal : Signal<Note>
    {
        
    }
    
    public class DeleteNoteRowCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        [Inject] public InstrumentNoteRowDefsChangedSignal InstrumentNoteRowDefsChangedSignal { get; set; }
        
        [Inject] public Note Note { get; set; }
        
        public override void Execute()
        {
           var instrument = InstrumentsModel.GetEditedInstrument();

           if (instrument == null)
           {
               Debug.LogError($"<color=\"aqua\">DeleteNoteRowCommand.Execute() : NO EDITED INSTRUMENT</color>");
               return;
           }

           instrument.NoteRowDefs.Remove(Note.Id);
           
           InstrumentNoteRowDefsChangedSignal.Dispatch();
        }
    }
}
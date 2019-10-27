using System;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace CKIEditor.Controller
{
    public class AddNoteRowSignal : Signal<NoteRowDef>
    {
        
    }
    
    public class AddNoteRowCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        [Inject] public InstrumentNoteRowDefsChangedSignal InstrumentNoteRowDefsChangedSignal { get; set; }
        
        [Inject] public NoteRowDef NoteDef { get; set; }
        
        public override void Execute()
        {   
            var editedInstrument = InstrumentsModel.GetEditedInstrument();
            
            // TODO noterow exists, overwrite? dialog
            if (editedInstrument.NoteRowDefs.ContainsKey(NoteDef.Note.Id))
            {
                Debug.LogError($"<color=\"aqua\">AddNoteDefCommand.Execute() : NoteDef {NoteDef.Note.Id} already exists!</color>");
                return;
            }
            //var noteId = NoteStringHelper.GetNoteIndex(NoteDef.Note);
            editedInstrument.NoteRowDefs[NoteDef.Note.Id] = NoteDef;
            
            InstrumentNoteRowDefsChangedSignal.Dispatch();    
        }
    }
}
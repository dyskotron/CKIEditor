using CKIEditor.Model;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace CKIEditor.Controller
{
    public class DeleteCcDefSignal : Signal<int>
    {
        
    }
    public class DeleteCcDefCommand : Command
    {   
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
     
        [Inject] public InstrumentCcDefsChangedSignal InstrumentCcDefsChangedSignal { get; set; }
        
        [Inject] public int CcIndex { get; set; }
        
        public override void Execute()
        {
            var instrument = InstrumentsModel.GetEditedInstrument();

            if (instrument == null)
            {
                Debug.LogError($"<color=\"aqua\">DeleteNoteRowCommand.Execute() : NO EDITED INSTRUMENT</color>");
                return;
            }

            instrument.CcDefs.Remove(CcIndex);
           
            InstrumentCcDefsChangedSignal.Dispatch();   
        }
    }
}
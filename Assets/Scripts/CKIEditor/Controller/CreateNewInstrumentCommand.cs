using CKIEditor.Model;
using CKIEditor.Model.Defs;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace CKIEditor.Controller
{
    public class CreateNewInstrumentSignal : Signal
    {
        
    }
    
    public class CreateNewInstrumentCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        
        public override void Execute()
        {
            var newInstrument = new InstrumentDef();
            var newInstrumentId = InstrumentsModel.AddInstrument(newInstrument);
            InstrumentsModel.SelectEditedInstrument(newInstrumentId);
            EditedInstrumentChangedSignal.Dispatch(newInstrument);
        }
    }
}
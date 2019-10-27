using CKIEditor.Model;
using CKIEditor.Model.Defs;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace CKIEditor.Controller
{
    public class EditGeneralSettingsSignal : Signal
    {
        
    }
    
    //TODO: instrument is edited directly in GeneralSettingsMediator currently
    public class EditGeneralSettingsCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public EditGeneralSettingsSignal EditGeneralSettingsSignal { get; set; }
        
        public override void Execute()
        {
            EditGeneralSettingsSignal.Dispatch();
        }
    }
}
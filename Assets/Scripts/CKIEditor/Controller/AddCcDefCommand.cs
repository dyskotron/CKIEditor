using CKIEditor.Model;
using CKIEditor.Model.Defs;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace CKIEditor.Controller
{
    public class AddCcDefSignal : Signal<CcDef>
    {
        
    }
    
    public class AddCcDefCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        [Inject] public InstrumentCcDefsChangedSignal InstrumentCcDefsChangedSignal { get; set; }
        
        [Inject] public CcDef Ccdef { get; set; }
        
        public override void Execute()
        {
            var editedInstrument = InstrumentsModel.GetEditedInstrument();
            
            /* TODO cc exists, overwrite? dialog
            if (editedInstrument.CcDefs.ContainsKey(Ccdef.CcNum))
            {
                command Debug.LogError($"<color=\"aqua\">CreateCcDefMediator.SaveButtonListener() : CcDefs {Ccdef.CcNum} already exists!</color>");
                return;
            }*/

            editedInstrument.CcDefs[Ccdef.CcNum] = Ccdef;
            
            InstrumentCcDefsChangedSignal.Dispatch();    
        }
    }
}
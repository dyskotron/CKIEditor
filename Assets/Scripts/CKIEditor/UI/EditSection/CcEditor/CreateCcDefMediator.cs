using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using Framewerk.UI;
using UnityEngine;

namespace CKIEditor.UI.EditSection.CcEditor
{
    public class CreateCcDefMediator : ExtendedMediator
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        [Inject] public AddCcDefSignal AddCcDefSignal { get; set; }
        
        [Inject] public CreateCcDefView View { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
            AddButtonListener(View.SaveButton, SaveButtonListener);
        }

        private void SaveButtonListener()
        {
            var ccDef = new CcDef(int.Parse(View.CcInput.text));
            ccDef.SetLabel(View.NameInput.text);
            ccDef.SetStartValue(int.Parse(View.StartInput.text));
            ccDef.SetMinValue(int.Parse(View.MinInput.text));
            ccDef.SetMaxValue(int.Parse(View.MaxInput.text));
            
            AddCcDefSignal.Dispatch(ccDef);
            ResetValues();
        }

        public void ResetValues()
        {
            View.CcInput.text = "";
            View.NameInput.text = "";
            View.StartInput.text = CcDef.MIN_CC_VALUE.ToString();
            View.MinInput.text = CcDef.MIN_CC_VALUE.ToString();
            View.MaxInput.text = CcDef.MAX_CC_VALUE.ToString();
        }
    }
}
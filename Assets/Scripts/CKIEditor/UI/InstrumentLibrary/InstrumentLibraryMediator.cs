using System.Collections.Generic;
using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using CKIEditor.UI.General;
using Framewerk.UI.List;
using TMPro;

namespace CKIEditor.UI.InstrumentLibrary
{
    public class InstrumentLibraryMediator : ListMediator<InstrumentLibraryView, InstrumentDef>
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public IOptionsModel OptionsModel { get; set; }
        
        [Inject] public InstrumentGeneralSettingsChangedSignal InstrumentGeneralSettingsChangedSignal { get; set; }
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        [Inject] public InstrumentsImportedSignal InstrumentsImportedSignal { get; set; }
        [Inject] public ImportInstrumentsSignal ImportInstrumentsSignal { get; set; }
        [Inject] public CreateNewInstrumentSignal CreateNewInstrumentSignal { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();

            InstrumentsImportedSignal.AddListener(RefreshInstruments);
            EditedInstrumentChangedSignal.AddListener(EditedInstrumentChangedHandler);
            InstrumentGeneralSettingsChangedSignal.AddListener(RefreshInstruments);

            View.InstrumentDropdown.onValueChanged.AddListener(OnDropdownChangedHandler);
            RefreshInstruments();
            
            AddButtonListener(View.ImportButton, ImportButtonHandler);
            AddButtonListener(View.ExportButton, ExportButtonHandler);
            AddButtonListener(View.NewButton, NewButtonHandler);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            
            InstrumentsImportedSignal.RemoveListener(RefreshInstruments);
            EditedInstrumentChangedSignal.RemoveListener(EditedInstrumentChangedHandler);
            View.InstrumentDropdown.onValueChanged.RemoveListener(OnDropdownChangedHandler);
        }
        
        private void RefreshInstruments()
        {
            var options = OptionsModel.GetInstrumentOptions();
            
            if (options.Count <= 0)
            {
                options.Add(new TMP_Dropdown.OptionData("No Instrument"));
            }
            
            View.InstrumentDropdown.options = options;
            View.InstrumentDropdown.interactable = options.Count > 0;
        }
        
        private void EditedInstrumentChangedHandler(InstrumentDef instrument)
        {
            RefreshInstruments();
        }

        protected override void ListItemSelected(int index, InstrumentDef dataProvider)
        {
            base.ListItemSelected(index, dataProvider);
            
            InstrumentsModel.SelectEditedInstrument(dataProvider.Id);
            EditedInstrumentChangedSignal.Dispatch(dataProvider);
        }
        
        private void OnDropdownChangedHandler(int id)
        {
            InstrumentsModel.SelectEditedInstrument(id); 
            EditedInstrumentChangedSignal.Dispatch(InstrumentsModel.GetEditedInstrument());    
        }
        
        private void ImportButtonHandler()
        {
            ImportInstrumentsSignal.Dispatch();    
        }
        
        private void NewButtonHandler()
        {
            CreateNewInstrumentSignal.Dispatch();    
        }

        private void ExportButtonHandler()
        {
            
        }
    }
}
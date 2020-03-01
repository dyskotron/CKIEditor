using System;
using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using Framewerk.UI.List;

namespace CKIEditor.UI.TrackValues
{
    public class TrackValueItemMediator : ListItemMediator<TrackValueItemView, TrackValueDataProvider>
    {
        [Inject] public IOptionsModel OptionsModel { get; set; }
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public InstrumentCcDefsChangedSignal InstrumentCcDefsChangedSignal { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
            
            AddDropdownListener(View.TrackValueTypeDropdown, TrackValueTypeDropdownChanged);
            AddDropdownListener(View.TrackControlTypeDropdown, TrackControlTypeDropdownChanged);
            AddDropdownListener(View.CcSelectionDropdown, CcSelectionDropdownChanged);
            
            InstrumentCcDefsChangedSignal.AddListener(EditedInstrumentChangedHandler);

            View.TrackValueTypeDropdown.options = OptionsModel.GetTrackValueOptions();
            View.TrackControlTypeDropdown.options = OptionsModel.GetTrackControlOptions();
            View.CcSelectionDropdown.options = OptionsModel.GetCcOptions();
        }

        public override void OnRemove()
        {
            InstrumentCcDefsChangedSignal.RemoveListener(EditedInstrumentChangedHandler);
            base.OnRemove();
        }

        private void EditedInstrumentChangedHandler()
        {
            View.CcSelectionDropdown.options = OptionsModel.GetCcOptions();  
            //TODO: select previously selected cc (in case we renamed / moved it)
        }

        private void TrackValueTypeDropdownChanged(int value)
        {
            DataProvider.TrackValue.Type = (TrackValueType) value;
            DataProvider.TrackValue.Label = InstrumentsModel.GetEditedInstrument().CcDefs[value].Label;
            UpdateView();
        }

        private void TrackControlTypeDropdownChanged(int value)
        {
            
            UpdateView();
        }
        
        private void CcSelectionDropdownChanged(int value)
        {
            DataProvider.TrackValue.MidiCC = OptionsModel.GetCCnumberByOptionId(value);
        }

        public override void SetData(TrackValueDataProvider dataProvider, int index)
        {
            base.SetData(dataProvider, index);

            View.TrackValueTypeDropdown.value = (int) dataProvider.TrackValue.Type;
            View.TrackControlTypeDropdown.value = (int) dataProvider.TrackValue.TrackControl;
            
            UpdateView();
        }

        private void UpdateView()
        {
            View.CcSelectionDropdown.gameObject.SetActive(DataProvider.TrackValue.Type == TrackValueType.MidiCC);
            View.TrackControlTypeDropdown.gameObject.SetActive(DataProvider.TrackValue.Type == TrackValueType.TrackControl);

            switch (DataProvider.TrackValue.Type)
            {
                case TrackValueType.Empty:
                    break;
                case TrackValueType.MidiCC:
                    //TODO - we should use label from DataProvider.TrackValue.Label 
                    //custom label should be passed when asking data for dropdown to override default cc name
                    View.CcSelectionDropdown.value = DataProvider.TrackValue.MidiCC;
                    break;
                case TrackValueType.TrackControl:
                    View.TrackControlTypeDropdown.value = (int)DataProvider.TrackValue.TrackControl;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }    
        }
    }
}
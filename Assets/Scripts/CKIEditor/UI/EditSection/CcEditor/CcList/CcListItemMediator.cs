using CKIEditor.Controller;
using CKIEditor.Model.Defs;
using CKIEditor.UI.TrackValues.CcList;
using Framewerk.UI.List;

namespace CKIEditor.UI.EditSection.CcEditor.CcList
{
    public class CcListItemMediator : ListItemMediator<CcListItemView, CcDef>
    {
        [Inject] public DeleteCcDefSignal AddCcDefSignal { get; set; }
        [Inject] public InstrumentCcDefsChangedSignal InstrumentCcDefsChangedSignal { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
            
            View.NameInput.characterLimit = CkiConsts.CC_NAME_CHARACTER_LIMIT;
            UpdateSelected();

            AddInputListener(View.NameInput, NameInputHandler);
            AddInputListener(View.CcInput, CcInputHandler);
            AddInputListener(View.StartInput, StartInputHandler);
            AddInputListener(View.MinInput, MinInputHandler);
            AddInputListener(View.MaxInput, MaxInputHandler);
            
            AddButtonListener(View.RemoveButton, RemoveButtonClickHandler);
        }

        public override void SetData(CcDef dataProvider, int index)
        {
            base.SetData(dataProvider, index);

            View.CcInput.text = dataProvider.CcNum.ToString();
            View.NameInput.text = dataProvider.Label;
            View.StartInput.text = dataProvider.StartValue.ToString();
            View.MinInput.text = dataProvider.MinValue.ToString();
            View.MaxInput.text = dataProvider.MaxValue.ToString();
        }

        public override void SetSelected(bool selected)
        {
            base.SetSelected(selected);
            UpdateSelected();

            View.NameInput.text = DataProvider.Label;
            View.NameInput.Select();
        }
        
        private void CcInputHandler(string value)
        {
            //todo: move to command
            DataProvider.SetCcNum(int.Parse(value));    
            InstrumentCcDefsChangedSignal.Dispatch();
        }
        
        private void NameInputHandler(string ccLabel)
        {
            //todo: move to command
            DataProvider.SetLabel(ccLabel);
            InstrumentCcDefsChangedSignal.Dispatch();
        }

        private void StartInputHandler(string value)
        {
            //todo: move to command
            DataProvider.SetStartValue(int.Parse(value)); 
            InstrumentCcDefsChangedSignal.Dispatch();
        }

        private void MinInputHandler(string value)
        {
            //todo: move to command
            DataProvider.SetMinValue(int.Parse(value));
            InstrumentCcDefsChangedSignal.Dispatch();
        }

        private void MaxInputHandler(string value)
        {
            //todo: move to command
            DataProvider.SetMaxValue(int.Parse(value));  
            InstrumentCcDefsChangedSignal.Dispatch();
        }
        
        private void RemoveButtonClickHandler()
        {
            AddCcDefSignal.Dispatch(DataProvider.CcNum);    
        }

        private void UpdateSelected()
        {
            //View.SaveButton.gameObject.SetActive(IsSelected);
            //View.SelectButton.gameObject.SetActive(!IsSelected);
            //View.RemoveButton.gameObject.SetActive(!IsSelected);
            
            View.BackgroundImage.color = IsSelected ? View.SelectedColor : View.NormalColor;

            //View.NameInput.gameObject.SetActive(IsSelected);
        }
    }
}
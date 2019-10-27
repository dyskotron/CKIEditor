using System.IO;
using CKIEditor.Model;
using CKIEditor.Serialization;
using CKIEditor.UI;
using Framewerk.Managers;
using strange.extensions.command.impl;
using UnityEngine;

namespace CKIEditor.Controller
{
    public class InitAppCommand : Command
    {
        [Inject] public IUiManager UiManager { get; set; }
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public IInstrumentsParser InstrumentsParser { get; set; }
        
        [Inject] public InstrumentsImportedSignal InstrumentsImportedSignal { get; set; }
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        
        public override void Execute()
        {
            var path = "/TEMP/INSTS.CKI";
            var jsonString = File.ReadAllText(path);
            var instruments = InstrumentsParser.ParseInstruments(jsonString);
            InstrumentsModel.AddInstruments(instruments);
            
            Screen.fullScreen = false;
            
            InstrumentsModel.SelectEditedInstrument(0);
            EditedInstrumentChangedSignal.Dispatch(InstrumentsModel.GetEditedInstrument());
            InstrumentsImportedSignal.Dispatch();
            
            UiManager.InstantiateView<EditorScreenView>();
        }
    }
}
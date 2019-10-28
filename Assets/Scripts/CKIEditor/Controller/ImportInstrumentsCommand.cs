using System.IO;
using CKIEditor.Model;
using CKIEditor.Serialization;
using Crosstales.FB;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace CKIEditor.Controller
{
    public class ImportInstrumentsSignal : Signal
    {
        
    }
    
    public class ImportInstrumentsCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public IInstrumentsParser InstrumentsParser { get; set; }
        
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        [Inject] public InstrumentsImportedSignal InstrumentsImportedSignal { get; set; }

        public override void Execute()
        {
            var path = FileBrowser.OpenSingleFile(JsonKeys.FILE_EXTENSIONS);
            Debug.LogWarning(string.Format("<color=\"aqua\">ImportInstrumentsCommand.Execute() : path:{0}</color>", path));
            var jsonString = File.ReadAllText(path);
            var instruments = InstrumentsParser.ParseInstruments(jsonString);
            InstrumentsModel.AddInstruments(instruments);
            
            InstrumentsModel.SelectEditedInstrument(0);
            EditedInstrumentChangedSignal.Dispatch(InstrumentsModel.GetEditedInstrument());
            InstrumentsImportedSignal.Dispatch();
        }
    }
}
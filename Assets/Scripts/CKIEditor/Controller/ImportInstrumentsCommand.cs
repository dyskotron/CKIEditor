using System.IO;
using CKIEditor.Model;
using CKIEditor.Serialization;
using Crosstales.FB;
using Framewerk.Managers;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace CKIEditor.Controller
{
    public class ImportInstrumentsSignal : Signal
    {
        
    }
    
    public class ImportInstrumentsCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public IInstrumentsParser InstrumentsParser { get; set; }
        [Inject] public IPlayerPrefsManager PrefsManager { get; set; }
        
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        [Inject] public InstrumentsImportedSignal InstrumentsImportedSignal { get; set; }
        
        private const string LOAD_DIRECTORY_KEY = "loadDirectory";

        public override void Execute()
        {
            var loadDirectory =  PrefsManager.GetUserString(LOAD_DIRECTORY_KEY, null);
            var path = FileBrowser.OpenSingleFile("Import CKI file",loadDirectory,JsonKeys.FILE_EXTENSIONS);
            PrefsManager.SetUserData(LOAD_DIRECTORY_KEY, Path.GetDirectoryName(path));
            
            var jsonString = File.ReadAllText(path);
            var instruments = InstrumentsParser.ParseInstruments(jsonString);
            InstrumentsModel.AddInstruments(instruments);
            
            InstrumentsModel.SelectEditedInstrument(0);
            EditedInstrumentChangedSignal.Dispatch(InstrumentsModel.GetEditedInstrument());
            InstrumentsImportedSignal.Dispatch();
            
            
        }
    }
}
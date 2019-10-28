using System.IO;
using CKIEditor.Model;
using CKIEditor.Serialization;
using Crosstales.FB;
using Framewerk.Managers;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace CKIEditor.Controller
{
    public class ExportInstrumentsSignal : Signal
    {
        
    }
    
    public class ExportInstrumentsCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public IInstrumentsParser InstrumentsParser { get; set; }
        [Inject] public IPlayerPrefsManager PrefsManager { get; set; }

        private const string SAVE_DIRECTORY_KEY = "saveDirectory";
            
        public override void Execute()
        {
            var loadDirectory =  PrefsManager.GetUserString(SAVE_DIRECTORY_KEY, null);
            string path = FileBrowser.SaveFile("Export CKI file",loadDirectory,"Library", JsonKeys.FILE_EXTENSIONS);
            PrefsManager.SetUserData(SAVE_DIRECTORY_KEY, Path.GetDirectoryName(path));
            
            var json = InstrumentsParser.BuildCkiFile(InstrumentsModel.GetAllInstruments());
            File.WriteAllText(path, json);
        }
    }
}
using System.IO;
using CKIEditor.Model;
using CKIEditor.Serialization;
using Crosstales.FB;
using Framewerk.Managers;
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
        [Inject(BindingKeys.PARSER_CKI)] public IInstrumentsParser CkiParser { get; set; }
        [Inject(BindingKeys.PARSER_PYRAMID)] public IInstrumentsParser PyramidParser { get; set; }
        [Inject] public IPlayerPrefsManager PrefsManager { get; set; }
        
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal { get; set; }
        [Inject] public InstrumentsImportedSignal InstrumentsImportedSignal { get; set; }
        
        private const string LOAD_DIRECTORY_KEY = "loadDirectory";

        public override void Execute()
        {
            //TODO: Import from clipboard
            //Debug.LogWarning($"<color=\"aqua\">ImportInstrumentsCommand.Execute() : {GUIUtility.systemCopyBuffer}</color>");
            //var instruments = PyramidParser.ParseInstruments(GUIUtility.systemCopyBuffer);
            
            //Get Path
            var loadDirectory =  PrefsManager.GetUserString(LOAD_DIRECTORY_KEY, null);
            var path = FileBrowser.OpenSingleFile("Import CKI file",loadDirectory, JsonKeys.FILE_EXTENSIONS);
            PrefsManager.SetUserData(LOAD_DIRECTORY_KEY, Path.GetDirectoryName(path));
            
            //Decide which parser to use based on extension
            var jsonString = File.ReadAllText(path);
            var extension = Path.GetExtension(path).Substring(1);
            var parser = extension == JsonKeys.PYRAMID_EXTENSION ? PyramidParser : CkiParser;
            var instruments = parser.ParseInstruments(jsonString);
            
            //return if no instruments found TODO: inform user
            if(instruments == null || instruments.Count == 0)
                return;

            var instrumentIdAfterImport = InstrumentsModel.GetAllInstruments().Count;
            InstrumentsModel.AddInstruments(instruments);
            InstrumentsModel.SelectEditedInstrument(instrumentIdAfterImport);
            EditedInstrumentChangedSignal.Dispatch(InstrumentsModel.GetEditedInstrument());
            InstrumentsImportedSignal.Dispatch();
        }
    }
}
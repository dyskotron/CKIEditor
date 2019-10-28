using System.IO;
using CKIEditor.Model;
using CKIEditor.Serialization;
using Crosstales.FB;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace CKIEditor.Controller
{
    public class ExportInstrumentsSignal : Signal
    {
        
    }
    
    public class ExportInstrumentsCommand : Command
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        [Inject] public IInstrumentsParser InstrumentsParser { get; set; }
        
        public override void Execute()
        {
            var json = InstrumentsParser.BuildCkiFile(InstrumentsModel.GetAllInstruments());
            //Debug.LogWarning($"<color=\"aqua\">InstrumentsParser.UnparseInstruments() : {json}</color>");
            string path = FileBrowser.SaveFile("Library", JsonKeys.FILE_EXTENSIONS);
            File.WriteAllText(path, json);
            Debug.Log($"Save file: {path}");
        }
    }
}
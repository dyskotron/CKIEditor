using System;
using System.Collections.Generic;
using CKIEditor.Model.Defs;
using TMPro;

namespace CKIEditor.Model
{
    public interface IOptionsModel
    {
        List<TMP_Dropdown.OptionData> GetInstrumentOptions();
        List<TMP_Dropdown.OptionData> GetMidiPortOptions();
        List<TMP_Dropdown.OptionData> GetMidiChannelOptions();
        List<TMP_Dropdown.OptionData> GetNoteOptions();
        List<TMP_Dropdown.OptionData> GetOctaveOptions();
        List<TMP_Dropdown.OptionData> GetPatternOptions();
    }
    
    public class OptionsModel : IOptionsModel
    {
        [Inject] public IInstrumentsModel InstrumentsModel { get; set; }
        
        public List<TMP_Dropdown.OptionData> GetInstrumentOptions()
        {
            var options = new List<TMP_Dropdown.OptionData>();
            var inst = InstrumentsModel.GetAllInstruments();

            foreach (var instrumentDef in inst)
            {
                options.Add(new TMP_Dropdown.OptionData(instrumentDef.Name));
            }

            return options;
        }
        
        public List<TMP_Dropdown.OptionData> GetMidiPortOptions()
        {
            return GenerateOptionsFromEnum(typeof(MidiPortOption));
        }

        public List<TMP_Dropdown.OptionData> GetMidiChannelOptions()
        {
            const int MIDI_CHANNEL_COUNT = 16;
            
            var options = new List<TMP_Dropdown.OptionData>();
            for (var i = 1; i <= MIDI_CHANNEL_COUNT; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(i.ToString()));    
            }

            return options;
        }
        
        public List<TMP_Dropdown.OptionData> GetNoteOptions()
        {
            var options = new List<TMP_Dropdown.OptionData>();  
            options.Add(new TMP_Dropdown.OptionData("C "));
            options.Add(new TMP_Dropdown.OptionData("C#"));
            options.Add(new TMP_Dropdown.OptionData("D "));
            options.Add(new TMP_Dropdown.OptionData("D#"));
            options.Add(new TMP_Dropdown.OptionData("E "));
            options.Add(new TMP_Dropdown.OptionData("F "));
            options.Add(new TMP_Dropdown.OptionData("F#"));
            options.Add(new TMP_Dropdown.OptionData("G "));
            options.Add(new TMP_Dropdown.OptionData("G#"));
            options.Add(new TMP_Dropdown.OptionData("A "));
            options.Add(new TMP_Dropdown.OptionData("A#"));
            options.Add(new TMP_Dropdown.OptionData("B "));
            return options;
        }
        
        public List<TMP_Dropdown.OptionData> GetOctaveOptions()
        {
            const int OCTAVE_COUNT = 10;
            
            var options = new List<TMP_Dropdown.OptionData>();
            for (var i = 1; i <= OCTAVE_COUNT; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(i.ToString()));    
            }

            return options;
        }
        

        public List<TMP_Dropdown.OptionData> GetPatternOptions()
        {
            var options = new List<TMP_Dropdown.OptionData>();  
            options.Add(new TMP_Dropdown.OptionData("P3"));
            options.Add(new TMP_Dropdown.OptionData("CK"));
            return options;
        }

        private List<TMP_Dropdown.OptionData> GenerateOptionsFromEnum(Type enumType)
        {
            var options = new List<TMP_Dropdown.OptionData>();

            var names = Enum.GetNames(enumType);
            foreach (var name in names)
            {
                options.Add(new TMP_Dropdown.OptionData(name.Replace('_', ' ')));    
            }

            return options;    
        } 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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
        List<TMP_Dropdown.OptionData> GetTrackControlOptions();
        List<TMP_Dropdown.OptionData> GetTrackValueOptions();
        List<TMP_Dropdown.OptionData> GetCcOptions();
        int GetCCnumberByOptionId(int value);
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

        public List<TMP_Dropdown.OptionData> GetTrackControlOptions()
        {
            return GenerateOptionsFromEnum(typeof(TrackControlType));
        }
        
        public List<TMP_Dropdown.OptionData> GetTrackValueOptions()
        {
            return GenerateOptionsFromEnum(typeof(TrackValueType));
        }

        public List<TMP_Dropdown.OptionData> GetCcOptions()
        {
            var instrument = InstrumentsModel.GetEditedInstrument();
            var options = new List<TMP_Dropdown.OptionData>();  
            
            foreach (var instrumentCcDef in instrument.CcDefs.Values)
            {
                options.Add(new TMP_Dropdown.OptionData(instrumentCcDef.Label));    
            }

            return options;
        }

        public int GetCCnumberByOptionId(int value)
        {
            //TODO: Refactor this super ugly way of getting cc num by option id
            var instrument = InstrumentsModel.GetEditedInstrument();
            return instrument.CcDefs.Values.ToList()[value].CcNum;
        }
        
        public int GetOptionIdByCcNumber(int value)
        {
            //TODO: Refactor this super ugly way of getting cc num by option id
            var instrument = InstrumentsModel.GetEditedInstrument();
            return instrument.CcDefs.Values.ToList()[value].CcNum;
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
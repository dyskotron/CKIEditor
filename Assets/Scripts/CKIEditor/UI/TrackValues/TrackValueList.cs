using System.Linq;
using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using Framewerk.UI.List;

namespace CKIEditor.UI.TrackValues
{
    public class TrackValueListMediator : ListMediator<TrackValueListView, TrackValueDataProvider>
    {
        [Inject] public IInstrumentsModel InstrumentsModel{ get; set; }
        
        [Inject] public EditedInstrumentChangedSignal EditedInstrumentChangedSignal{ get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            
            var instrument = InstrumentsModel.GetEditedInstrument();
            UpdateInstrument(instrument);
            
            EditedInstrumentChangedSignal.AddListener(UpdateInstrument);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            
            EditedInstrumentChangedSignal.RemoveListener(UpdateInstrument);
        }

        private void UpdateInstrument(InstrumentDef instrument)
        {
            var listData = instrument.TrackValues.Values.Select(tv => new TrackValueDataProvider(tv)).ToList();
            
            SetData(listData);    
        }
    }

    public class TrackValueDataProvider : IListItemDataProvider
    {
        public TrackValueDef TrackValue { get; private set; }

        public TrackValueDataProvider(TrackValueDef trackValue)
        {
            TrackValue = trackValue;
        }
    }
}
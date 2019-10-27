using CKIEditor.Model.Defs;
using Framewerk.UI.List;

namespace CKIEditor.UI.General
{
    public class InstrumentListItemMediator : ListItemMediator<InstrumentListItemView, InstrumentDef>
    {
        public override void SetData(InstrumentDef dataProvider, int index)
        {
            base.SetData(dataProvider, index);

            View.NameLabel.text = dataProvider.Name;
        }
    }
}
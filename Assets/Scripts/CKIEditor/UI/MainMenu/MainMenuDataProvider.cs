using Framewerk.UI.List;

namespace CKIEditor.UI.MainMenu
{
    public class MainMenuDataProvider : IListItemDataProvider
    {
        public EditPage Type { get; }
        public string Label { get; }

        public MainMenuDataProvider(EditPage type)
        {
            Type = type;
            Label = type.ToString();
        }
    }
}
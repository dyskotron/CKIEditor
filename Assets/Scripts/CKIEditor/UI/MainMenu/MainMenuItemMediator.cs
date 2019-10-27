using Framewerk.UI.List;

namespace CKIEditor.UI.MainMenu
{
    public class MainMenuItemMediator : ListItemMediator<MainMenuItemView, MainMenuDataProvider>
    {
        public override void OnRegister()
        {
            base.OnRegister();
            View.BackgroundImage.color = View.BaseColor;
        }

        public override void SetData(MainMenuDataProvider dataProvider, int index)
        {
            base.SetData(dataProvider, index);

            View.Label.text = dataProvider.Label;
        }

        public override void SetSelected(bool selected)
        {
            base.SetSelected(selected);
            View.BackgroundImage.color = selected ? View.SelectedColor : View.BaseColor;
        }
    }
}
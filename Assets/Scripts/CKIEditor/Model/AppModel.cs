using CKIEditor.UI.MainMenu;

namespace CKIEditor.Model
{
    public interface IAppModel
    {
        EditPage selectedEditPage { get; set; }
    }
    
    public class AppModel : IAppModel
    {
        public EditPage selectedEditPage { get; set; }
    }
}
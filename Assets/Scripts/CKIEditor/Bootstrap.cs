using strange.extensions.context.impl;

namespace CKIEditor
{
    public class Bootstrap : ContextView
    {
        public ViewConfig ViewConfig;

        private void Start()
        {
            //Client context
            context = new CkiEditorContext(this, ViewConfig);
            context.Start();
        }
    }
}
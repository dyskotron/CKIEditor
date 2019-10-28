using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Serialization;
using CKIEditor.UI;
using CKIEditor.UI.EditSection.CcEditor;
using CKIEditor.UI.EditSection.CcEditor.CcList;
using CKIEditor.UI.EditSection.General;
using CKIEditor.UI.EditSection.GeneralSettings;
using CKIEditor.UI.General;
using CKIEditor.UI.InstrumentLibrary;
using CKIEditor.UI.MainMenu;
using CKIEditor.UI.NoteRows;
using CKIEditor.UI.TrackValues;
using CKIEditor.UI.TrackValues.CcList;
using Framewerk;
using Framewerk.Managers;
using strange.extensions.context.impl;
using UnityEngine;

namespace CKIEditor
{
    /// <summary>
    /// TODO:
    /// Fix Notes
    /// TrackValues
    /// Export
    /// </summary>
    public class CkiEditorContext : MVCSContext
    {
        private ViewConfig _viewConfig;

        public CkiEditorContext(MonoBehaviour view, ViewConfig viewConfig) : base(view, true)
        {
            _viewConfig = viewConfig;
        }

        protected override void mapBindings()
        {
            base.mapBindings();
            
            injectionBinder.Bind<ViewConfig>().ToValue(_viewConfig);
            
            injectionBinder.Bind<IAssetManager>().To<AssetManager>().ToSingleton();
            injectionBinder.Bind<IUiManager>().To<UiManager>().ToSingleton();
            injectionBinder.Bind<IPlayerPrefsManager>().To<PlayerPrefsManager>().ToSingleton();
            
            injectionBinder.Bind<IAppModel>().To<AppModel>().ToSingleton();
            injectionBinder.Bind<IInstrumentsParser>().To<InstrumentsParser>().ToSingleton();
            injectionBinder.Bind<IInstrumentsModel>().To<InstrumentsModel>().ToSingleton();
            injectionBinder.Bind<IOptionsModel>().To<OptionsModel>().ToSingleton();
            
            injectionBinder.Bind<AppSectionChangedSignal>().To<AppSectionChangedSignal>().ToSingleton();
            injectionBinder.Bind<EditedInstrumentChangedSignal>().To<EditedInstrumentChangedSignal>().ToSingleton();
            injectionBinder.Bind<InstrumentsImportedSignal>().To<InstrumentsImportedSignal>().ToSingleton();
            
            injectionBinder.Bind<InstrumentCcDefsChangedSignal>().To<InstrumentCcDefsChangedSignal>().ToSingleton();
            injectionBinder.Bind<InstrumentNoteRowDefsChangedSignal>().To<InstrumentNoteRowDefsChangedSignal>().ToSingleton();
            injectionBinder.Bind<InstrumentGeneralSettingsChangedSignal>().To<InstrumentGeneralSettingsChangedSignal>().ToSingleton();
            
            // ========================================== VIEW MEDIATION ==============================================
            
            mediationBinder.Bind<EditorScreenView>().To<EditorScreenMediator>();
            
            mediationBinder.Bind<GeneralSettingsView>().To<GeneralSettingsMediator>();
            mediationBinder.Bind<TrackValuesPageView>().To<TrackValuesPageMediator>();
            
            mediationBinder.Bind<MainMenuView>().To<MainMenuMediator>();
            mediationBinder.Bind<MainMenuItemView>().To<MainMenuItemMediator>();
            
            mediationBinder.Bind<InstrumentLibraryView>().To<InstrumentLibraryMediator>();
            mediationBinder.Bind<InstrumentListItemView>().To<InstrumentListItemMediator>();
            
            mediationBinder.Bind<CcListView>().To<CcListMediator>();
            mediationBinder.Bind<CcListItemView>().To<CcListItemMediator>();
            mediationBinder.Bind<CreateCcDefView>().To<CreateCcDefMediator>();
            
            mediationBinder.Bind<NoteRowListView>().To<NoteRowListMediator>();
            mediationBinder.Bind<NoteRowListItemView>().To<NoteRowListItemMediator>();
            mediationBinder.Bind<AddNoteDefView>().To<AddNoteDefMediator>();

            // ============================================= COMMANDS =================================================
            
            commandBinder.Bind<ContextStartSignal>().To<InitAppCommand>();
            
            commandBinder.Bind<ImportInstrumentsSignal>().To<ImportInstrumentsCommand>();
            commandBinder.Bind<ExportInstrumentsSignal>().To<ExportInstrumentsCommand>();
            
            commandBinder.Bind<CreateNewInstrumentSignal>().To<CreateNewInstrumentCommand>();
            commandBinder.Bind<EditGeneralSettingsSignal>().To<EditGeneralSettingsCommand>();
            
            //cc
            commandBinder.Bind<AddCcDefSignal>().To<AddCcDefCommand>();
            commandBinder.Bind<DeleteCcDefSignal>().To<DeleteCcDefCommand>();
            
            //note row
            commandBinder.Bind<AddNoteRowSignal>().To<AddNoteRowCommand>();
            commandBinder.Bind<DeleteNoteRowSignal>().To<DeleteNoteRowCommand>();
            commandBinder.Bind<EditNoteRowSignal>().To<EditNoteRowCommand>();
            
            
        }
    }
}
using System;
using System.Collections.Generic;
using CKIEditor.Controller;
using CKIEditor.Model;
using Framewerk.UI.List;
using UnityEngine;

namespace CKIEditor.UI.MainMenu
{
    public class MainMenuMediator : ListMediator<MainMenuView, MainMenuDataProvider>
    {
        [Inject] public IAppModel AppModel { get; set; }
        [Inject] public AppSectionChangedSignal AppSectionChangedSignal { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();

            //generate menu data
            var providers = new List<MainMenuDataProvider>();
            var values = Enum.GetValues(typeof(EditPage));
            foreach (EditPage itemType in values)
            {
                providers.Add(new MainMenuDataProvider(itemType));
            }
            
            SetData(providers);
            SelectItemAt(0);
        }

        protected override void ListItemSelected(int index, MainMenuDataProvider dataProvider)
        {
            base.ListItemSelected(index, dataProvider);
            Debug.LogWarning($"<color=\"aqua\">MainMenuMediator.ListItemClicked() : {dataProvider.Label}</color>");

            AppModel.selectedEditPage = dataProvider.Type;
            AppSectionChangedSignal.Dispatch(dataProvider.Type);
        }
    }
}
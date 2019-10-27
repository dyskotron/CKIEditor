using System.Collections.Generic;

namespace Framewerk.UI.List
{
    /// <summary>
    /// Mediates View containing list of items.
    /// </summary>
    /// <typeparam name="TView">View scipt attached to container gameobject</typeparam>
    /// <typeparam name="TMediator">Mediator of single item in list</typeparam>
    /// <typeparam name="TData">Dataprovider for single item in list </typeparam>
    public class ListMediator<TView, TData> : ListItemBaseMediator<TView, TData>, IListMediator<TData>  where TView : ListView
                                                                                                             where TData : class, IListItemDataProvider
    {
        public virtual void SetData(List<TData> dataProviders)
        {
            base.SetData(dataProviders);

            //reenable / create item mediators when there's more data than spawned mediators
            //fill mediator with data
            for (var i = 0; i < dataProviders.Count; i++)
            {
                //SET DATA
                if (i < ItemMediators.Count)
                {
                    var listItem = ItemMediators[i];
                    listItem.SetActive(true);
                    SetItemData(listItem, dataProviders[i], i);
                }
                //CREATE NEW
                else if (i >= CreatedMediatorsCount)
                {
                    CreateItemMediator(i, View.ItemPrefab, View.ContentsParent);
                }
            }

            //hide item renderers when there's more spawned mediators than data
            for (var i = dataProviders.Count; i < ItemMediators.Count; i++)
            {
                ItemMediators[i].SetActive(false);
            }
        }
    }
}
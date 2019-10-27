using System;
using System.Collections.Generic;
using Framewerk.Managers;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Framewerk.UI.List
{
    /// <summary>
    /// Mediates View containing list of items.
    /// </summary>
    /// <typeparam name="TView">View scipt attached to container GameObject</typeparam>
    /// <typeparam name="TData">Dataprovider for single item in list </typeparam>
    public class ListItemBaseMediator<TView, TData> : ExtendedMediator, IListMediator<TData> where TView : ListView 
                                                                                          where TData : class, IListItemDataProvider
                                                                                              
    {
        [Inject] public IUiManager UiManager { get; set; }
        [Inject] public TView View { get; set; }
        
        public List<int> SelectedItemIndexes { get; private set; }
        public bool Multiselect { get; set; }
        public bool Unselectable { get; set; }

        protected int CreatedMediatorsCount = 0;
        protected List<IListItemMediator<TData>> ItemMediators = new List<IListItemMediator<TData>>();
        protected List<TData> DataProviders = new List<TData>();
        
        public override void OnRegister()
        {
            SelectedItemIndexes = new List<int>();
            
            base.OnRegister();
        }

        #region Public API

        public virtual void RegisterMediator(IMediator mediator)
        {
            var itemIndex = ItemMediators.Count;
            var ItemMediator = (IListItemMediator<TData>) mediator;
            
            ItemMediators.Add(ItemMediator);  
            
            //if we have data set them to mediator
            if(DataProviders.Count >= ItemMediators.Count)
                SetItemData(ItemMediator, DataProviders[itemIndex], itemIndex);
            //if we dont, hide item
            else
                ItemMediator.SetActive(false);
            
            ItemMediator.ListItemClickedSignal.AddListener(ListItemClickedHandler);
        }
        
        public virtual void SetData(List<TData> dataProviders)
        {
            //TODO: just remove selected indexes and shit, dont' do actual unselecting on list items?
            UnselectAll();
            
            if (View.EmptyContent != null)
                 View.EmptyContent.SetActive(dataProviders.Count == 0);

            DataProviders = dataProviders;
        }

        public void Destroy()
        {
            foreach (var itemMediator in ItemMediators)
            {
                itemMediator.Destroy();
            }
            
            ItemMediators.Clear();
            GameObject.Destroy(View.gameObject);
        }
        
        /// <summary>
        /// Method for selecting item in list from code instead of user input.
        /// Does not call internal list method ListItemSelected.
        /// </summary>
        /// <param name="index">item index</param>
        public void SelectItemAt(int index)
        {
            if(SelectedItemIndexes.Contains(index))
                return;
            
            //unselect old item
            if (!Multiselect && SelectedItemIndexes.Count > 0)
                ApplyItemUnselection(SelectedItemIndexes[0]);
            
            ApplyItemSelection(index);
            
            SelectionUpdated(index);
        }

        /// <summary>
        /// Method for unselecting item in list from code instead of user input.
        /// Does not call internal list method ListItemUnselected.
        /// </summary>
        /// <param name="index">item index</param>
        public void UnselectItemAt(int index)
        {   
            if(!SelectedItemIndexes.Contains(index))
                return;
            
            ApplyItemUnselection(index);
            
            SelectionUpdated(index);
        }
        
        public TData GetSelectedItem()
        {
            return SelectedItemIndexes.Count == 0 ? null : DataProviders[SelectedItemIndexes[0]];
        }
        
        public List<TData> GetSelectedItems()
        {
            List<TData> selectedItems = new List<TData>();
            foreach (var SelectedItemIndex in SelectedItemIndexes)
            {
                selectedItems.Add(DataProviders[SelectedItemIndex]);    
            }
            
            return selectedItems;
        }

        public int? GetSelectedIndex()
        {
            if (SelectedItemIndexes.Count > 0)
                return SelectedItemIndexes[0];
            
            return null;
        }

        public virtual void UnselectAll()
        {
            foreach (var selectedItemIndex in SelectedItemIndexes)
            {
                GetMediatorAt(selectedItemIndex).SetSelected(false);    
            }
            
            SelectedItemIndexes.Clear();
        }
        
        public virtual void RemoveItemAt(int index)
        {
            var provider = GetDataproviderAt(index);
            if (provider == null)
                return;

            DataProviders.Remove(provider);
            
            //set new index & disable unused items
            for (var i = index; i < ItemMediators.Count; i++)
            {
                if(i < DataProviders.Count)
                    GetMediatorAt(i).SetIndex(i);
                else
                    GetMediatorAt(i).SetActive(false);
            }
        }
        
        public TData GetDataproviderAt(int index)
        {
            return index < DataProviders.Count ? DataProviders[index] : null;
        }
        
        public int FindItemIndex(TData data)
        {
            var index = DataProviders.FindIndex((d) => d == data);
            return index;
        }
        
        #endregion

        #region Search helpers
        
        protected IListItemMediator<TData> FindItemMediatorByCustom<TSearched>(Func<TData, TSearched, bool> comparator, TSearched searchedItem)
        {
            var index = FindItemIndexByCustom(comparator, searchedItem);
            return index >= 0 ? GetMediatorAt(index) : null;
        }
        
        protected TData FindDataproviderByCustom<TSearched>(Func<TData, TSearched, bool> comparator, TSearched searchedItem)
        {
            var index = FindItemIndexByCustom(comparator, searchedItem);
            return index >= 0 ? GetDataproviderAt(index) : null;
        }
        
        protected int FindItemIndexByCustom<TSearched>(Func<TData, TSearched, bool> comparator, TSearched searchedItem)
        {
            for (var i = 0; i < DataProviders.Count; i++)
            {
                if (comparator(DataProviders[i], searchedItem))
                    return i;
            }

            return -1;
        }

        protected virtual IListItemMediator<TData> GetMediatorAt(int index)
        {
            return index < ItemMediators.Count ? ItemMediators[index] : null;
        }
        
        #endregion
        
        protected void CreateItemMediator(int index, GameObject prefab, RectTransform parent)
        {
            UiManager.InstantiateView(prefab, parent);
            CreatedMediatorsCount++;
        }
        
        protected virtual void SetItemData(IListItemMediator<TData> itemMediator, TData itemDataProvider, int index)
        {
            itemMediator.SetData(itemDataProvider, index);    
        }
        
        protected virtual void ApplyItemSelection(int index)
        {
            SelectedItemIndexes.Add(index); 
            GetMediatorAt(index).SetSelected(true);    
        }
        
        protected virtual void ApplyItemUnselection(int index)
        {
            SelectedItemIndexes.Remove(index);
            GetMediatorAt(index).SetSelected(false);    
        }

        protected virtual void ListItemClicked(int index, TData dataProvider)
        {
            
        }
        
        protected virtual void ListItemSelected(int index, TData dataProvider)
        {
            
        }
        
        protected virtual void ListItemUnselected(int index, TData dataProvider)
        {
            
        }

        protected virtual void SelectionUpdated(int index)
        {
            
        }
        
        private void ListItemClickedHandler(int itemIndex, TData dataProvider)
        {   
            //select/unselected new item
            var selected = SelectedItemIndexes.Contains(itemIndex);
            if (!selected)
            {
                SelectItemAt(itemIndex);
                SelectionUpdated(itemIndex);
                ListItemSelected(itemIndex, dataProvider);
            }
            else if (Unselectable)
            {
                UnselectItemAt(itemIndex);
                SelectionUpdated(itemIndex);
                ListItemUnselected(itemIndex, dataProvider);
            }
            
            ListItemClicked(itemIndex, dataProvider);
        }
    }
}
using Framewerk.UI.Components;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Framewerk.UI.List
{
    public class ListView : View
    {
        [Header("List Container")]
        public RectTransform ContentsParent;
        public GameObject ItemPrefab;
        public GameObject EmptyContent;
        
        //public VirtualListScroller ListScroller;
        public DragElement DragElement;
        public bool UnselectAllOnDrag = false;
    }
}
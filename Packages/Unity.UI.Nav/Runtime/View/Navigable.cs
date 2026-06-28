using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity.UI.Navs
{

    public class Navigable : MonoBehaviour, INavigable
    {

        private GameObject lastSelected;
        public int ViewId => Context.Id;
        private int lastRefreshFrame;
        List<Selectable> disableSelectables = new();
        CanvasGroup canvasGroup;

        public Dictionary<string, object> ViewData { get; set; }

        public object Model { get; set; }

        public NavContext Context { get; private set; }


        public void SetContext(NavContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 准备数据
        /// </summary>
        public virtual void OnLoad()
        {
            NavUtility.Log($"[OnLoad] [{name}]");
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (!canvasGroup)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            //Refresh();
        }


        public virtual void OnUnload()
        {
            NavUtility.Log($"[OnUnload] [{name}]");

        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public virtual void OnNavigationFrom(NavContext from)
        {
            NavUtility.Log($"[OnNavigationFrom] [{name}] \nFrom: {from?.Url}");
            if (disableInteractableCanvasGroup)
            {
                disableInteractableCanvasGroup.interactable = true;
                disableInteractableCanvasGroup = null;
            }

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (lastSelected)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
            if (lastRefreshFrame != Time.frameCount)
            {
                Refresh();
            }
        }
        CanvasGroup disableInteractableCanvasGroup;

        public virtual void OnNavigationTo(NavContext to)
        {
            NavUtility.Log($"[OnNavigationTo] [{name}] \nTo: {to?.Url}");

            lastSelected = EventSystem.current.currentSelectedGameObject;
            bool isActive = false;
            if ((Context.Flags & NavFlags.Home) != 0)
            {
                isActive = true;
            }
            if (to != null && (to.Flags & NavFlags.Float) != 0)
            {
                isActive = true;
            }
            if (gameObject && gameObject.activeSelf != isActive)
                gameObject.SetActive(isActive);
            bool interactable = false;

            if (to != null)
            {
                if ((to.Flags & NavFlags.Float) != 0)
                {
                    interactable = true;
                }
                if ((to.Flags & NavFlags.Exclusive) != 0)
                {
                    interactable = false;
                }
            }


            if (!interactable && canvasGroup && canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                disableInteractableCanvasGroup = canvasGroup;
            }
            /*
            NoAllocArray = GetArray<Selectable>(Selectable.allSelectableCount);
            NoAllocArrayLength = Selectable.AllSelectablesNoAlloc(NoAllocArray);
            for (int i = 0; i < NoAllocArrayLength; i++)
            {
                var sel = NoAllocArray[i];
                if (!sel.IsInteractable()) continue;
                if (IsAncestor(transform, sel.transform))
                {

                }
            }
            */
        }

        static bool IsAncestor(Transform t, Transform ancestor)
        {
            if (!t) return false;
            Transform parent = t.parent;
            while (parent)
            {
                if (parent == ancestor)
                    return true;
                parent = parent.parent;
            }
            return false;
        }

        Selectable[] NoAllocArray;
        int NoAllocArrayLength;
        T[] GetArray<T>(int count)
        {
            int n = 1 << 2;
            while (n < count)
            {
                n <<= 1;
            }
            var array = new T[n];
            return array;
        }

        protected virtual void Refresh()
        {
            lastRefreshFrame = Time.frameCount;
        }



        public virtual void Back()
        {
            Nav.Back(this);
        }


    }
}
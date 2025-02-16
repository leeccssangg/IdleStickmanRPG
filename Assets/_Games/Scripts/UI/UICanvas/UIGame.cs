using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pextension
{
    public class UIGame : Singleton<UIGame>
    {
        //dict contain ui game object active
        private Dictionary<System.Type, UICanvas> UICanvas = new Dictionary<System.Type, UICanvas>();
        //array for load
        private UICanvas[] uis;
        //dict contain prefab to quick query
        private Dictionary<System.Type, UICanvas> UICanvasPrefab = new Dictionary<System.Type, UICanvas>();

        //holder for canvas
        public Transform CanvasParentTF;
        public Transform SubCanvasParentTF;

        #region Canvas
        private void Start()
        {
           //OpenUI<UICGamePlay>();
        }
        private T GetPrefab<T>() where T : UICanvas
        {
            uis ??= Resources.LoadAll<UICanvas>("UI/");

            if (UICanvasPrefab.ContainsKey(typeof(T))) return UICanvasPrefab[typeof(T)] as T;
            for (int i = 0; i < uis.Length; i++)
            {
                if (uis[i] is not T) continue;
                UICanvasPrefab.Add(typeof(T), uis[i]);
                break;
            }

            return UICanvasPrefab[typeof(T)] as T;
        }

        public bool IsOpenedUI<T>() where T : UICanvas
        {
            System.Type ui = typeof(T);
            return UICanvas.ContainsKey(ui) && UICanvas[ui] != null && UICanvas[ui].gameObject.activeInHierarchy;
        }

        public T GetUI<T>() where T : UICanvas
        {
            System.Type ui = typeof(T);
            if (UICanvas.ContainsKey(ui) && UICanvas[ui] != null) return UICanvas[ui] as T;
            UICanvas canvas = Instantiate(GetPrefab<T>(), CanvasParentTF);
            canvas.gameObject.SetActive(false);
            UICanvas[ui] = canvas;

            return (T)UICanvas[ui];
        }
        public T OpenUI<T>() where T : UICanvas
        {
            UICanvas canvas = GetUI<T>();

            canvas.Setup();
            canvas.Open();

            return (T)canvas;
        }

        //was init ui
        public bool IsLoaded<T>() where T : UICanvas
        {
            System.Type ui = typeof(T);
            return UICanvas.ContainsKey(ui) && UICanvas[ui] != null;
        }

        public void CloseUI<T>() where T : UICanvas
        {
            if (IsLoaded<T>())
            {
                GetUI<T>().Close();
            }
        }

        #endregion

        #region Back Button

        private Dictionary<UICanvas, UnityAction> BackActionEvents = new Dictionary<UICanvas, UnityAction>();
        private List<UICanvas> backCanvas = new List<UICanvas>();
        UICanvas BackTopUI
        {
            get
            {
                UICanvas canvas = null;
                if (backCanvas.Count > 0)
                {
                    canvas = backCanvas[backCanvas.Count - 1];
                }

                return canvas;
            }
        }


        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.Escape) && BackTopUI != null)
            {
                BackActionEvents[BackTopUI]?.Invoke();
            }
        }

        public void PushBackAction(UICanvas canvas, UnityAction action)
        {
            if (!BackActionEvents.ContainsKey(canvas))
            {
                BackActionEvents.Add(canvas, action);
            }
        }

        public void AddBackUI(UICanvas canvas)
        {
            if (!backCanvas.Contains(canvas))
            {
                backCanvas.Add(canvas);
            }
        }

        public void RemoveBackUI(UICanvas canvas)
        {
            backCanvas.Remove(canvas);
        }

        /// <summary>
        /// CLear backey when comeback index UI canvas
        /// </summary>
        public void ClearBackKey()
        {
            backCanvas.Clear();
        }

        #endregion

    }
}


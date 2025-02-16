using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace Pextension{

    public class TabGroupButton : MonoBehaviour{
        [SerializeField] private Button[] m_Buttons;
        [SerializeField] private Transform m_ImageSelect;
        private UnityAction<Enum> m_ClickButton;
        public void Setup<T>(UnityAction<T> actionCallback) where T : Enum{
            m_ClickButton = (Enum type) => actionCallback((T)type);
            int count = m_Buttons.Length;
            for(int i = 0; i < count; i++){
                Button button = m_Buttons[i];
                Enum typeButton = (Enum)Enum.GetValues(typeof(T)).GetValue(i);
                button.onClick.AddListener(() => OnClickButton(typeButton));
            }
        }
        public void OnClickButton(Enum type){
            // m_ImageSelect.DOKill();
            var trans = m_Buttons[Convert.ToInt32(type)].transform;
            //Vector3 pos = m_Buttons[1].transform.position;
            // m_ImageSelect.DOMoveX(pos.x, 0.15f);
            m_ImageSelect.transform.SetParent(trans);
            // m_ImageSelect.transform.localPosition = Vector3.zero;
            m_ImageSelect.transform.SetAsFirstSibling();
            m_ImageSelect.DOLocalMove(Vector3.zero, 0.15f);
            // Debug.Log(m_ImageSelect.transform.position + " " + pos);
            m_ClickButton?.Invoke(type);
        }
    }

}
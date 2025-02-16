using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelPower : MonoBehaviour
{
    [field:SerializeField] private TextMeshProUGUI m_TextPower;
    
    
    public void ChangePowerValue(BigNumber value, BigNumber changeValue){
        m_TextPower.text = value.ToString3();
    }
}

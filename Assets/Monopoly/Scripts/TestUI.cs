using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.Utility.Extension;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            UIManager.Ins.OpenUI<CvMonopoly>();
        }
        // if (Input.GetKeyDown(KeyCode.V))
        // {
        //     PropertiesManager.Ins.MonopolyControl.AddDice(100);
        // }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStickmanRace : MonoBehaviour
{
    public Image raceBG;
    public Image raceIcon;

    public void Setup(RaceSprite stickmanRace) {
        raceBG.sprite = stickmanRace.raceBg;
        raceIcon.sprite = stickmanRace.raceIcon;
    }
}

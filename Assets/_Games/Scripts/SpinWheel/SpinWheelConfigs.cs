using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "SpinWheelConfigs", menuName = "GlobalConfigs/SpinWheelConfigs")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class SpinWheelConfigs : GlobalConfig<SpinWheelConfigs>
{ 
    public List<SpinWheelDataConfigs<GiftType>> SpinWheelDataConfigs = new List<SpinWheelDataConfigs<GiftType>>();
}


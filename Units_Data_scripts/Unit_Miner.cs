using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Unit_Miner : Unit {
    
    public Unit_Miner()
    {
        type = UnitType.PLAYER_MINER;
    }

    public override void LoadUnitProperties()
    {
        base.LoadUnitProperties();
        unitHP = 5;
        unitSpeed = 5;
    }
}

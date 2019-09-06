using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class HazardGround : GroundTile
{
    public HazardGround()
    {
        type = GroundType.HAZARD;
    }

    public int damagePerTick;
    public float dmgTickRepeat;

    void DamagingGroundSet() {

        damagePerTick = 1;
        dmgTickRepeat = 0.8f;
        //Debug.Log("Sick burns!"); // works properly
    }

    public override void LoadProperties()
    {
        base.LoadProperties();
        //name = "Hazardous Tile";
        tileSpeedModifier = 1f;
        //tileMovementPenaltyForPF = 20; // deprecated
        isCrossable = false;
        tileCurrentMaterial = Resources.Load<Material>("Materials/Terrain_Hazard");
        tileRenderer.material = tileCurrentMaterial;
        DamagingGroundSet();

    }
}

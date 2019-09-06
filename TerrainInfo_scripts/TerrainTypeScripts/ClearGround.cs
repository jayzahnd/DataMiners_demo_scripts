using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ClearGround : GroundTile {

    public ClearGround(){

        type = GroundType.CLEAR;
        //Debug.Log("Adding clear ground...");
    }

    public override void LoadProperties()
    {
        base.LoadProperties();
        
        tileSpeedModifier = 1f;
        //tileMovementPenaltyForPF = 0; // deprecated
        tileCurrentMaterial = Resources.Load<Material>("Materials/Terrain_Default");
        tileRenderer.material = tileCurrentMaterial;

        //Debug.Log("Clear Ground, loading properties.....");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoughGround : GroundTile
{
    public RoughGround()
    {
        type = GroundType.ROUGH;
    }

    public override void LoadProperties()
    {
        base.LoadProperties();        
        
        tileSpeedModifier = 0.5f;
        //tileMovementPenaltyForPF = 10; // deprecated
        tileCurrentMaterial = Resources.Load<Material>("Materials/Terrain_Rough");
        tileRenderer.material = tileCurrentMaterial;

        //Debug.Log("Rough Ground, loading properties.....");

    }
}

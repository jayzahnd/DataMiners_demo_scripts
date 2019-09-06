using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class BaseGround : GroundTile 
{
    public BaseGround() {

        type = GroundType.BASE;
    }

    public override void LoadProperties() {
        base.LoadProperties();
        
        tileSpeedModifier = 1f;
        //tileMovementPenaltyForPF = 0; // deprecated
        tileCurrentMaterial = Resources.Load<Material>("Materials/Terrain_Base");
        tileRenderer.material = tileCurrentMaterial;
        tileRenderer.tag = "Base_Area";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ObjectiveGround : GroundTile 
{
    public ObjectiveGround() {

        type = GroundType.OBJECTIVE;
    }

    public override void LoadProperties() {
        base.LoadProperties();
        
        tileSpeedModifier = 1f;
        //tileMovementPenaltyForPF = 0; // deprecated
        tileCurrentMaterial = Resources.Load<Material>("Materials/Terrain_Objective");
        tileRenderer.material = tileCurrentMaterial;

    }

}

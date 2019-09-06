using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ImpassableGround : GroundTile 
{

    public ImpassableGround() {

        type = GroundType.IMPASSABLE;
    }

    public override void LoadProperties() {
        base.LoadProperties();
        
        tileSpeedModifier = 1f;
        isWalkable = false;
        

        isCrossable = false;
        tileCurrentMaterial = Resources.Load<Material>("Materials/Terrain_Impassable");
        tileRenderer.material = tileCurrentMaterial;
    }

}

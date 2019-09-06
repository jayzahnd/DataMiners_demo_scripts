using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class CircuitGround : GroundTile
{
    public CircuitGround()
    {
        type = GroundType.CIRCUIT;
    }

    public override void LoadProperties()
    {
        base.LoadProperties();        
        
        tileSpeedModifier = 2f;
        tileCurrentMaterial = Resources.Load<Material>("Materials/Terrain_Circuit");
        tileRenderer.material = tileCurrentMaterial;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GroundTile {

    public enum GroundType { CLEAR, ROUGH, HAZARD, IMPASSABLE, OBJECTIVE, BASE, CIRCUIT }
    public GroundType type;

    //public GameObject tileHighlight;  // can later be replaced by a sprite.
    //public string tileName;

    public int tileID;

    public float tileSpeedModifier;  // for unit

    // public int tileMovementPenaltyForPF; // for pathfinding calculations, deprecated

    public bool isWalkable = true;  // set to false for unaccessible places and unbreakable obstacles.
    public bool isCrossable = true; // used for pathfinding
    

    public bool hasBlockOnIt = false;

    public MeshRenderer tileRenderer;

    [SerializeField]
    protected Material tileCurrentMaterial;





    

    
    public void OnSelected() {

    }

    public void OnDeselected() {

    }


    public virtual void LoadProperties() {
        //Debug.Log("Attempting to load properties.....");
        //gameObject.GetComponent<MeshRenderer>().material = tileCurrentMaterial;        
    }   

}

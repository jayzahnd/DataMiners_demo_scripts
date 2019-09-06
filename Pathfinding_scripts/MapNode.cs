using System.Collections;
using UnityEngine;
using System;

[Serializable]
public class MapNode: IHeapItem<MapNode> {

    public bool walkableNode;

    public Vector3 worldPosition;
    public Vector3 pfOffsetPosition;
    public int lvlGrid_X;
    public int lvlGrid_YorZ;


    public int gCost; // G cost
    public int hCost; // H cost
    public MapNode parentTileNode;
    public int fCost {
        get {
            return gCost + hCost;
        }
    }
    public int movementModifier;


    public MapNode(bool _walkable, Vector3 _worldpos, int _gridX, int _gridYorZ, int _mvtMod) {
        walkableNode = _walkable;
        worldPosition = _worldpos;
        lvlGrid_X = _gridX;
        lvlGrid_YorZ = _gridYorZ;
        movementModifier = _mvtMod;
    }

    public int NodeHeapIndex { get; set; }

    //private int nodeHeapIndex;          // replaced above by auto-property.
    //public int NodeHeapIndex {
    //    get { return nodeHeapIndex; }
    //    set { nodeHeapIndex = value; }
    //}

    public int CompareTo(MapNode nodeToCompare) {
        int compareValue = fCost.CompareTo(nodeToCompare.fCost);
        if(compareValue == 0) {
            compareValue = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compareValue;
    }
}

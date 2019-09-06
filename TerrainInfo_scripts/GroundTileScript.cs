using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundTileScript : MonoBehaviour {

    public GroundTile tileReference; // To load a tile's properties.
    public MapNode nodeReference;  // To load a tile's pathfinding node information.

    
    //------------------------------------------------------------------------------------------------
    // Pathfinding-related vars.

    //public bool selectable = false;  // "selectable" from a unit's perspective when pathfinding.
    //public bool current = false;
    //public bool isDestination = false;

    //public int f; // F cost  = g + h
    //public int g; // G cost
    //public int h; // H cost heurisitc

    //public List<GroundTileScript> adjacentTiles = new List<GroundTileScript>();  //  should be a list of 4 when hitting play.

    //public GroundTileScript parentTile = null;
    //public int distance = 0;

    //public bool visited = false;


    //------------------------------------------------------------------------------------------------
    // Pathfinding-related functions.

    //public void Reset()
    //{
    //    adjacentTiles.Clear();

    //    selectable = false;
    //    current = false;
    //    isDestination = false;

    //    f = g = h = 0;

    //    GroundTileScript parentTile = null;
    //    distance = 0;
    //    visited = false;
    //}
    //
    //public void CheckTileForMvt(Vector3 direction, GroundTileScript origin)
    //{
    //    Vector3 halfExtents = new Vector3(gridDisplace/2, 0.3f, gridDisplace/2);
    //    Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents); // needs work
    //    for(int i=0; i < colliders.Length; i++) {
    //        //Output all of the collider names
    //        Debug.Log("Hit : " + colliders[i].name + i);
    //    }

    //    foreach (Collider c in colliders)
    //    {
    //        GroundTileScript gts = c.GetComponent<GroundTileScript>();
    //        if (gts != null && gts.tileReference.isWalkable)
    //        {
    //            RaycastHit hit;
    //            if (!Physics.Raycast(gts.transform.position, Vector3.up, out hit, 1) || gts == origin) // This is where to check for obstacle presence blocking tile access.
    //            {
    //                adjacentTiles.Add(gts);
    //            }
    //        }
    //    }
    //}
    //public void SearchForNeighbours(GroundTileScript targetTile)
    //{
    //    Reset();

    //    CheckTileForMvt(Vector3.forward, targetTile);
    //    CheckTileForMvt(-Vector3.forward, targetTile);
    //    CheckTileForMvt(Vector3.right, targetTile);
    //    CheckTileForMvt(-Vector3.right, targetTile);
    //}

    //------------------------------------------------------------------------------------------------
}

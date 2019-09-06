using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding_a3 : MonoBehaviour {


    //PathFinderRequestManager pfReqManager;
    TerrainBuilder_02 mapGrid;

    private void Awake() {
        mapGrid = gameObject.GetComponent<TerrainBuilder_02>();
        //pfReqManager = gameObject.GetComponent<PathFinderRequestManager>();
    }
    
    // Update is called once per frame
    void Update () {
        // update the path finding method
        //GoToTile(seeking.position, destination.position);
	}

    //public void QueueGoToTile (Vector3 startPos, Vector3 destPos) {  No longer needed since we will now call GoToTile directly.
    //    //Debug.Log("QueueGoToTile called");
    //    StartCoroutine(GoToTile(startPos, destPos));
    //}

    public void GoToTile(PathRequest request, Action<PathResult> callback) {

        //Clean A* first

        //Debug.Log("GoToTile started for: "+startPos+" to "+destPos);
        Vector3[] waypoints = new Vector3[0];
        bool pathProcessed = false;

        MapNode startNode = mapGrid.NodeFromWorldPoint(request.pStart);       // could be modified later on. Was startPos
        //Debug.Log("GTT Start node: "+ startNode.lvlGrid_X + "; " + startNode.lvlGrid_YorZ +" walkable= "+ startNode.walkableNode);
        MapNode destNode = mapGrid.NodeFromWorldPoint(request.pEnd); // was destPos . NOTE: corresponds to "goThereModif" in the UnitComponent class.
        //Debug.Log("GTT End node: " + destNode.lvlGrid_X + "; " + destNode.lvlGrid_YorZ + " walkable= " +destNode.walkableNode);

        if (startNode.walkableNode && destNode.walkableNode) {

            //Debug.Log("GTT Both are walkable, proceeding... MapTotalSize= "+ mapGrid.TotalMapSize);

            HeapOptim<MapNode> openListHeap = new HeapOptim<MapNode>(mapGrid.TotalMapSize);
            HashSet<MapNode> closedList = new HashSet<MapNode>();

            openListHeap.HeapAdd(startNode);
                        
            while (openListHeap.HeapCount > 0) {
                //Debug.Log("GTT while Loop initiated"); // starts properly
                MapNode currentNode = openListHeap.RemoveFirst();
                closedList.Add(currentNode);

                if (currentNode == destNode) {

                    //Need to add destNode to path array.

                    
                    //Debug.Log("GTT "+(currentNode == destNode) + " Path computed. current;dest: "+ currentNode.worldPosition +"; "+ destNode.worldPosition);
                    pathProcessed = true;
                    break;
                }

                foreach (MapNode neighbour in mapGrid.GetNeighbours(currentNode)) {
                    if (!neighbour.walkableNode || closedList.Contains(neighbour)) {  // add crossable condition here, not in GetNeighbours.
                        continue;
                    }

                    int newCostToNeighbour = currentNode.gCost + ReturnDistance(currentNode, neighbour) + neighbour.movementModifier; // This is where we take into account a tile's unit movement penalty for pathfinding.

                    //Debug.Log("newCostToNeighbour: "+newCostToNeighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openListHeap.Contains(neighbour)) {

                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = ReturnDistance(neighbour, destNode);
                        neighbour.parentTileNode = currentNode;

                        if (!openListHeap.Contains(neighbour)) {

                            openListHeap.HeapAdd(neighbour);
                        }
                        else {
                            openListHeap.UpdateHeapNode(neighbour);
                        }
                    }
                }
            }
        }
        if (pathProcessed) {
            //Debug.Log("Retracing path from: "+ startNode.worldPosition +" to "+ destNode.worldPosition);
            waypoints = RetracePath(startNode, destNode, request.pEnd);
            pathProcessed = waypoints.Length > 0;
        }
        //pfReqManager.DoneProcessing(waypoints, pathProcessed);
        callback(new PathResult(waypoints, pathProcessed, request.callback));

    }

    //int endlessCheck = 0;
    //void EditEndNodeOffset(MapNode _endNode, int _count, int _iteration, int _turn)
    //{
    //    //Debug.Log("endlessCheck "+endlessCheck);
    //    endlessCheck++;        

    //    float angle = (360 / _count * _turn);

    //    Vector3 positionXYZ = _endNode.worldPosition + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * _turn;

    //    RaycastHit localHit;
    //    Ray localRay = new Ray();

    //    localRay.origin = positionXYZ + new Vector3(0, 10, 0);
    //    localRay.direction = positionXYZ - localRay.origin;

    //    Debug.DrawRay(localRay.origin, localRay.direction, Color.red);

    //    if (Physics.Raycast(localRay, out localHit))
    //    {
    //        if (localHit.collider.GetComponent<GroundTileScript>())
    //        {

    //            if (_iteration < _count && endlessCheck < 100)
    //            {
    //                Debug.Log("GTS FOUND: " + localHit.point + " -- Local endNode.worldPosition: " + _endNode.worldPosition);
    //                _endNode.pfOffsetPosition.x = localHit.point.x - _endNode.worldPosition.x;
    //                _endNode.pfOffsetPosition.y = 0;  // always keep this at 0, as tiles with displacement will return non-zero values and mess up the pathfinding.
    //                _endNode.pfOffsetPosition.z = localHit.point.z - _endNode.worldPosition.z;

    //                EditEndNodeOffset(_endNode, _count, _iteration + 1, _turn + 1);
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Obstacle detected at PF stage");
    //            if (endlessCheck < 100)
    //            {
    //                EditEndNodeOffset(_endNode, _count, _iteration, _turn + 1);
    //            }
    //        }
    //    }

    //}

    int ReturnDistance(MapNode nodeA, MapNode nodeB) {
        int distX = Mathf.Abs(nodeA.lvlGrid_X - nodeB.lvlGrid_X);
        int distY = Mathf.Abs(nodeA.lvlGrid_YorZ - nodeB.lvlGrid_YorZ);

        if (distX > distY) {
            return 14 * distY + 10 * (distX - distY);  // 14 is roughly the square root of 2, times 10.
        }
        else { return 14 * distX + 10 * (distY - distX); }
    }

    Vector3[] RetracePath(MapNode startNode, MapNode endNode, Vector3 _actualEndPoint) {
        List<MapNode> newPath = new List<MapNode>();

        //   IMPLEMENT JOE'S PLOT POINT COMMAND HERE, at least for now.
        //endlessCheck = 0;
        ////Debug.Log("endlessCheck reset" + endlessCheck);

        //endNode.pfOffsetPosition = new Vector3(0,0,0);
        ////Debug.Log("endNode.pfOffsetPosition reset: " + endNode.pfOffsetPosition);
        //int unitsInGroup = UnitSelectionManager.usmInstance.currentlySelectedUnits.Count;
        ////Debug.Log("unitsInGroup: " + unitsInGroup);
        //for (int i=0; i<unitsInGroup; i++)
        //{
        //    EditEndNodeOffset(endNode, 1+i, i, 0); // Note : replace 1 later by the count of selected units in a group 1+i.
        //    //Debug.Log("New endNode.pfOffsetPosition: "+ endNode.pfOffsetPosition);
        //}

        endNode.pfOffsetPosition = _actualEndPoint - endNode.worldPosition;  // how much we will have to sway from the world position to avoid merging units together.
        
        //Debug.Log("New endNode.pfOffsetPosition: " + endNode.pfOffsetPosition);

        MapNode currentNode = endNode;
        //Debug.Log("currentNode set to EndNode: "+ currentNode.worldPosition);   

        while (currentNode != startNode) {
            newPath.Add(currentNode);
            currentNode = currentNode.parentTileNode;
        }
        //Debug.Log(newPath[0].worldPosition +" to "+ newPath[newPath.Count-1].worldPosition +" | newPath Count: "+ newPath.Count +" Simplifying...");
        Vector3[] waypoints = SimplerPath(newPath);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplerPath(List<MapNode> path) {
        List<Vector3> wp = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        if (path.Count == 1) {
            Vector2 directionNew = new Vector2((path[0].lvlGrid_X), path[0].lvlGrid_YorZ);
            //Debug.Log(directionNew);

            Vector3 additive = path[0].worldPosition + path[0].pfOffsetPosition;
            
            wp.Add(additive);
            
            directionOld = directionNew;
        }

        {
            for (int i = 1; i < path.Count; i++) {
                Vector2 directionNew = new Vector2((path[i - 1].lvlGrid_X - path[i].lvlGrid_X), (path[i - 1].lvlGrid_YorZ - path[i].lvlGrid_YorZ));

                if (directionNew != directionOld) {
                    Vector3 additive2 = path[i - 1].worldPosition + path[i - 1].pfOffsetPosition;
                    wp.Add(additive2);
                }
                directionOld = directionNew;
            }
        }
        
        //Debug.Log(wp[0] + " to " + wp[wp.Count - 1] + " | simplified wp Count: " + wp.Count + " Returning waypoints to array...");
        return wp.ToArray();
    }


}

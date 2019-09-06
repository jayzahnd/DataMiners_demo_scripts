using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitComponent : MonoBehaviour {

    //Selection and pathfinding should now work properly.

    public Unit unitReference;

    public Transform goThere = null;
        
    Vector3 whereAmI;

    GameObject destinationIndicator;

    public bool isHighlighted = false;

    [SerializeField]
    Vector3[] unitPath;
    public List<Vector3> unitPathList = new List<Vector3>();

    int targetIndex;

    [SerializeField]private static Camera portCam;  // fetched from TerainBuilder_02 upon level creation.

    private void OnEnable() {
        
        //PathFinderRequestManager.RequestPath(whereAmI.position, goThere.position, OnPathFound);
        unitReference.unitModel = transform.GetChild(0).gameObject;
        unitReference.friendUnitAnimator = unitReference.unitModel.GetComponent<Animator>(); // this should ensure that we always get the right animator on the right model.
        //UnitSelectionManager.lmbSelectToggleEvent += UnitSelectionToggle;
        UnitSelectionManager.rmbMoveOrderEvent += SendPathRequestForUnit;
        destinationIndicator = GetComponent<DestinationPoint>().destinationPointObject;
        GetComponent<StorageStatus>().isStored = false;


    }
    public static void FetchCameraprefab(GameObject _mainCam) {

        portCam = _mainCam.GetComponentInChildren<Camera>();

    }
    

    void Update() {

       
    }
    //----------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------

    #region SELECTION FEEDBACK
    //void UnitSelectionToggle(RaycastHit hit, bool ctrlKeyDown) {

    //    GameObject objectClicked = hit.collider.transform.parent.gameObject;
    //    UnitComponent objectClickedScript = hit.collider.GetComponentInParent<UnitComponent>();  // retrieve from map node note ground tile script
    //    List<UnitComponent> currentlySelectedUnitsUC = UnitSelectionManager.usmInstance.currentlySelectedUnits;

    //    if (objectClicked == gameObject) // Weird problem in there. Need fix.
    //    {

    //        //Debug.Log("BEEP BOOP YES I am a " + hit.transform.gameObject.name);
    //        if (ctrlKeyDown == true)
    //        {
    //            if (!currentlySelectedUnitsUC.Contains(objectClickedScript))
    //            {


    //                unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.magenta; // just for fun
    //                unitReference.isSelected = true;
    //                //objectClickedScript.unitReference.SelectionFeedback();
    //                currentlySelectedUnitsUC.Add(objectClickedScript);
    //            }
    //            else
    //            {
    //                unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.white; // just for fun
    //                unitReference.isSelected = false;
    //                //objectClickedScript.unitReference.SelectionFeedback();
    //                currentlySelectedUnitsUC.Remove(objectClickedScript);
    //            }
    //        }
    //        else
    //        {
    //            UnitSelectionManager.usmInstance.UCListCleanup();


    //            unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.magenta; // just for fun
    //            unitReference.isSelected = true;
    //            //objectClickedScript.unitReference.SelectionFeedback();

    //            currentlySelectedUnitsUC.Add(objectClickedScript);
    //        }
    //    }



    //}

    #endregion

    //----------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------
 
    #region DESTINATION SCAN 

    int endlessCheckUC = 0;
    Vector3 ScanDestination(RaycastHit _hitUC, int _countUC, int _turnUC)
    {
        endlessCheckUC++;
        
        Vector3 storeHitPointInfoV3 = new Vector3(0, 0, 0);//_hitUC.collider.transform.position;
        float angleUC = (360 / _countUC * _turnUC);
        Vector3 positionXYZ = _hitUC.collider.transform.position + new Vector3(Mathf.Sin(angleUC), 0, Mathf.Cos(angleUC)) * _turnUC;

        RaycastHit localHit;
        Ray localRay = new Ray();
        localRay.origin = positionXYZ + new Vector3(0, 10, 0);
        localRay.direction = positionXYZ - localRay.origin;

        if (Physics.Raycast(localRay, out localHit))
        {
            if (localHit.collider.GetComponent<GroundTileScript>() && UnitSelectionManager.usmInstance.currentlySelectedUnits.Contains(this) && endlessCheckUC < 100)
            {
                //MapNode destnodeRef = localHit.collider.GetComponent<GroundTileScript>().nodeReference;
                //Debug.Log("Local hit position: " + localHit.point + " -- Local storeHitPointV3: " + storeHitPointInfoV3);
                storeHitPointInfoV3.x = localHit.point.x;// - destnodeRef.worldPosition.x;
                storeHitPointInfoV3.y = 0;  // always keep this at 0, as tiles with displacement will return non-zero values and mess up the pathfinding.
                storeHitPointInfoV3.z = localHit.point.z;// - destnodeRef.worldPosition.z;

                destinationIndicator.transform.position = storeHitPointInfoV3 +  new Vector3(0, 0.5f, 0);
                destinationIndicator.SetActive(true);
                //Debug.Log("Dest V3: " + localHit.point + " +  FIRST Offset storeHitPointV3: " + storeHitPointInfoV3);

                //Debug.Log("Loop scan with iteration +1 and turn+1");
                //Debug.Log("This is number:" + _iterationUC);
                //ScanDestination(_hitUC, _countUC, _turnUC + 1);
                return storeHitPointInfoV3;

            }
            else
            {
                
                if (endlessCheckUC < 100)
                {
                    //Debug.Log("Obstacle detected at ScanDestination stage");
                    //Debug.Log("Looping scan with turn+1");
                    return ScanDestination(_hitUC, _countUC,  _turnUC + 1);                    
                }
                else { return new Vector3(0, 0, 0); }
            }
            //Debug.Log("Dest V3: " + localHit.point + " + Offset storeHitPointV3: " + storeHitPointInfoV3);
        }
        else { return new Vector3(0,0,0); }
    }
    #endregion

    //----------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------

    #region PATH MANAGEMENT

    public void SendPathRequestForUnit(RaycastHit hit, int listCount) {

        Debug.Log("RMB clicked, Move order received for: "+ gameObject.name);
        
        whereAmI = gameObject.transform.position;
        goThere = hit.collider.transform;
        Vector3 goThereModif = new Vector3 (goThere.position.x, 0, goThere.position.z);
        //Debug.Log(goThere.position +"| original goThereModif: "+ goThereModif);

        GroundTileScript gts = hit.collider.GetComponent<GroundTileScript>();  // retrieve from map node note ground tile script
        
        if (!(gts.transform.position == whereAmI) && unitReference.isSelected == true)
        {
            goThereModif = ScanDestination(hit, listCount, 0);

            //destinationIndicator.transform.position = goThereModif + new Vector3(0, 0.2f, 0);
            //destinationIndicator.SetActive(true);

            //Debug.Log(goThere.position + "| POSTSCAN Modif: " + goThereModif);
            //Debug.Log("Unit " + gameObject.name + " is at: " + gameObject.transform.position.x + "; " + gameObject.transform.position.z + " and wants to head to: " + goThereModif.x + "; " + goThereModif.z);

            PathFinderRequestManager.RequestPath(new PathRequest(whereAmI, goThereModif, OnPathFound)); // changed in accordance with new pf manager version.
        }
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccess) {
        if (pathSuccess) {
            Debug.Log("Path success returned for: "+gameObject.name);
            unitPath = newPath;
            //StopCoroutine("FollowRoute");  // in case any previous routine is already running, this will allow for overriding.
            //StartCoroutine("FollowRoute");

            unitPathList = unitPath.ToList<Vector3>();
            
        }
    }

    //IEnumerator FollowRoute() {                                   // currently not used
    //    Vector3 currentWP = unitPath[0];
    //    float speed = unitReference.unitSpeed;
    //    while (true) {
    //        if(transform.position == currentWP) {
    //            targetIndex++;
    //            if(targetIndex >= unitPath.Length) {
    //                yield break;
    //            }
    //            currentWP = unitPath[targetIndex];
    //        }
    //        //Debug.Log("I am at: " + transform.position);
    //        transform.position = Vector3.MoveTowards(transform.position, currentWP, speed*Time.deltaTime);
    //        //Debug.Log("I am going to: " + transform.position);
    //        yield return null;
    //    }
    //}


    private void FixedUpdate()
    {
        if(unitPathList.Count > 0)
        {
            unitReference.isMoving = true;
            unitReference.friendUnitAnimator.SetBool("isMovingAnim", true); // if 1 doesn't work, use "isMovingAnim" instead. 
            //destinationIndicator.SetActive(true);
            float speed = unitReference.unitSpeed;
            transform.position = Vector3.MoveTowards(transform.position, unitPathList[0], speed * Time.deltaTime);
            transform.LookAt(unitPathList[0]); // jittery, need to smooth out later

            if (transform.position == unitPathList[0])
            {
                unitPathList.Remove(unitPathList[0]);
            }

        }
        else
        {
            destinationIndicator.transform.position = gameObject.transform.parent.transform.position + new Vector3(0, 0.2f, 0);
            destinationIndicator.SetActive(false);

            unitReference.friendUnitAnimator.SetBool("isMovingAnim", false);
            unitReference.isMoving = false;

            
        }        

    }
    #endregion

    //----------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------

    private void OnDisable() {
        //UnitSelectionManager.lmbSelectToggleEvent -= UnitSelectionToggle;
        GetComponent<StorageStatus>().isStored = true;
        UnitSelectionManager.rmbMoveOrderEvent -= SendPathRequestForUnit;
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class UnitSelectionManager : MonoBehaviour {
        
    //TerrainBuilder_02 levelScript;
    public static UnitSelectionManager usmInstance;

    [SerializeField]Camera playerCam;

    

    public bool ctrlKeyDown = false; // hold down ctrl to select multiple friendly units.
    string playerUnitTag = "PlayerUnit";
    string baseBuildingTag = "Base_BuildingMain";
    string uiTag = "UI_raycastBlock";

    [SerializeField]
    public List<UnitComponent> currentlySelectedUnits = new List<UnitComponent>();

    public List<UnitComponent> idleUnits = new List<UnitComponent>();
    public List<UnitComponent> busyUnits = new List<UnitComponent>();
    
    //public delegate void SelectToggle(RaycastHit hit, bool ctrlKey);  // Old, kept for documentation purposes.
    //public static event SelectToggle lmbSelectToggleEvent;

    public delegate void MoveOrder(RaycastHit hit, int listCount);
    public static event MoveOrder rmbMoveOrderEvent;

    public delegate void Disable_UI_Info();
    public static event Disable_UI_Info disableUIInfoEvent;
    
    Vector3 mousePosition1 = Vector3.zero;
    bool isSelecting;

    Vector3 pingMousePos;
    bool isDragging= false;
    [SerializeField] private bool doNotDrag = false;

    LayerMask pureselectionFilter = 1 << 14 | 1 << 16;

    // Use this for initialization
    void Awake () {
        
        
        usmInstance = this;
        // Old stuff
        //levelScript = gameObject.GetComponent<TerrainBuilder_02>();
        //playerCam = levelScript.cameraHolderPrefab.GetComponentInChildren<Camera>();
    }
    public void FetchCameraPrefab(GameObject cam) {
        playerCam = cam.GetComponentInChildren<Camera>();
    }


    #region UPDATE FUNCTION
    // Update is called once per frame
    void Update () {

        if (Input.GetKey(KeyCode.LeftControl)) {
            ctrlKeyDown = true;
        }
        else {
            ctrlKeyDown = false;
        }
                

        if (Input.GetMouseButtonUp(1))  // RMB UP
        {
            //Debug.Log("Right MB clicked. Deselecting all units");
            //foreach (playerUnitStruct _PuS in TerrainBuilder_02.player_inf_Array)
            //{
            //    _PuS.unitPrefab.transform.GetChild(1).gameObject.SetActive(false);
            //}
            //isSelecting = false;
            //UCListCleanup();
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rmbHit;

            if (currentlySelectedUnits.Count > 0) {
                if (Physics.Raycast(ray, out rmbHit)) 
                {
                    if (rmbHit.collider.GetComponent<GroundTileScript>())
                    {
                        //for (int i = 0; i < currentlySelectedUnits.Count; i++)
                        //{
                        //}

                        //Debug.Log("MOVE ORDER SENT");
                        //rmbMoveOrderEvent(rmbHit, currentlySelectedUnits.Count, i);
                        rmbMoveOrderEvent(rmbHit, currentlySelectedUnits.Count);
                    }
                        
                    

                }
                    
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            

            pingMousePos = Input.mousePosition;
            RaycastHit hit;
            if (Physics.Raycast(playerCam.ScreenPointToRay(Input.mousePosition), out hit, 200f, 5 << 9)) {
                mousePosition1= hit.point;
            }
            //if (!EventSystem.current.IsPointerOverGameObject()) {  // I tried initially ~(1 << 5). Bitwise "~" (invert) here means that we check for collision on all layers EXCEPT the 5th one (the UI layer)
            //    disableUIInfoEvent();                                               //Debug.Log("UI hit");

            //}
            
        }

        if (Input.GetMouseButton(0)) {

            if (doNotDrag == false)
            {
                isSelecting = true;

                if (pingMousePos != Input.mousePosition)
                {
                    //Debug.Log("Mouse Moved");
                    isDragging = true;
                }
            }
            
        }


        if (Input.GetMouseButtonUp(0)) {
            //Debug.Log("Left MB clicked");
            //Ray ray2 = playerCam.ScreenPointToRay(Input.mousePosition);
            //RaycastHit lmbHit2;
            //if (Physics.Raycast(ray2, out lmbHit2, Mathf.Infinity, ~pureselectionFilter))
            //{
            //    disableUIInfoEvent();
            //}
                
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit lmbHit;

            if (Physics.Raycast(ray, out lmbHit, 200f, pureselectionFilter))
            {
                //disableUIInfoEvent();
                //Debug.Log("Hit!");
                //GameObject hitObject = hit.collider.transform.gameObject;
                //Debug.Log("I am a " + hit.collider.transform.gameObject.name);
                if (lmbHit.collider.CompareTag(playerUnitTag))
                {

                    // Use delegate here.
                    //lmbSelectToggleEvent(lmbHit, ctrlKeyDown);

                    UnitSelected(lmbHit.collider.gameObject, ctrlKeyDown);
                    //Debug.Log("lmbSelectToggleEvent");

                }
                else if (lmbHit.collider.CompareTag(baseBuildingTag))
                {

                    lmbHit.collider.gameObject.GetComponentInParent<UI_ActionsButtons>().EnableButtons();
                }
            }

            foreach (playerUnitStruct _PuS in TerrainBuilder_02.player_inf_Array)
            {
                if (_PuS.unitPrefab.GetComponent<UnitComponent>().isHighlighted == true) // && _PuS.unitPrefab.GetComponent<StorageStatus>().isStored == false)
                {
                    GameObject modelunit = _PuS.unitPrefab.GetComponent<UnitComponent>().unitReference.unitModel;
                    UnitSelected(modelunit, ctrlKeyDown); // need to switch to child object, currently on parent
                }
                //_PuS.unitPrefab.transform.GetChild(1).gameObject.SetActive(false);
            }

            isDragging = false;
            isSelecting = false;
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            StartCoroutine(RepoolUnit());
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            disableUIInfoEvent();
        }

        if (isSelecting)
        {
            foreach (playerUnitStruct _PuS in TerrainBuilder_02.player_inf_Array)
            {
                //if (_PuS.unitPrefab.GetComponent<StorageStatus>().isStored == true)
                //{
                //    continue;
                //}

                if (IsWithinSelectionBounds(_PuS.unitPrefab))
                {
                    if (_PuS.unitPrefab.transform.GetChild(1).gameObject.activeSelf == false)
                    {
                        _PuS.unitPrefab.transform.GetChild(1).gameObject.SetActive(true);
                        currentlySelectedUnits.Add(_PuS.unitPrefab.GetComponent<UnitComponent>());
                        //_PuS.unitPrefab.GetComponent<UnitComponent>().unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.magenta; // just for fun
                        _PuS.unitPrefab.GetComponent<UnitComponent>().isHighlighted = true;
                    }
                }
                else
                {
                    if (_PuS.unitPrefab.transform.GetChild(1).gameObject.activeSelf == true && ctrlKeyDown == false)
                    {
                        _PuS.unitPrefab.transform.GetChild(1).gameObject.SetActive(false);
                        currentlySelectedUnits.Remove(_PuS.unitPrefab.GetComponent<UnitComponent>());
                        _PuS.unitPrefab.GetComponent<UnitComponent>().unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.white; // just for fun
                        _PuS.unitPrefab.GetComponent<UnitComponent>().isHighlighted = false;
                        _PuS.unitPrefab.GetComponent<UnitComponent>().unitReference.isSelected = false;
                    }
                }
            }
        }
    }

    #endregion //UPDATE

 

    IEnumerator RepoolUnit()
    {
        if (currentlySelectedUnits.Count > 0)
        {
            foreach (UnitComponent _uc in currentlySelectedUnits)
            {

                _uc.unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.white; // just for fun
                _uc.unitReference.isSelected = false;
                //_uc.unitReference.SelectionFeedback();

                _uc.unitReference.friendUnitAnimator.SetTrigger("FriendUnitDie");
                

                

            }
            yield return new WaitForSeconds(2.6f);

            foreach (UnitComponent _uc in currentlySelectedUnits) {
                _uc.gameObject.transform.position = gameObject.GetComponent<TerrainBuilder_02>().player_inf_Pool.transform.position + new Vector3(0, 0.2f, 0);

                _uc.gameObject.SetActive(false);

            }
            currentlySelectedUnits = new List<UnitComponent>();
            //Debug.Log("UC list cleaned up. Units repooled");
            yield return null;
        }
        yield return null;
    }

    Vector3 mousePosition2;
    public bool IsWithinSelectionBounds(GameObject controlObject)
    {
        if (!isSelecting)
            return false;

        
        RaycastHit hit;
        if (Physics.Raycast(playerCam.ScreenPointToRay(Input.mousePosition), out hit, 200f, 5<<9))
        {
            mousePosition2 = hit.point;
        }
        Bounds viewportBounds = RectangleDragSelection.GetViewportBounds(playerCam, mousePosition1, mousePosition2);
        return viewportBounds.Contains(playerCam.WorldToViewportPoint(controlObject.transform.position));
    }

    void UnitSelected(GameObject unitClicked, bool ctrlKeyState) // note: unit clicked here is the unit model, not the holder.
    {
        //UnitComponent currentUnit = unitClicked.transform.parent.gameObject.GetComponent<UnitComponent>();        
        UnitComponent currentUnit = unitClicked.transform.parent.gameObject.GetComponent<UnitComponent>();
        //Debug.Log(currentUnit == null);
        GameObject projectorRef = unitClicked.transform.parent.transform.GetChild(1).gameObject;

        if (ctrlKeyState == true)
        {
            if(!currentlySelectedUnits.Contains(currentUnit))
            {
                currentlySelectedUnits.Add(currentUnit);
                projectorRef.SetActive(true);
                currentUnit.unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.magenta; // just for fun
                currentUnit.unitReference.isSelected = true;
            }

        } else
        {
            if (isDragging == false)
            {
                UCListCleanup();
                currentlySelectedUnits.Add(currentUnit);
                projectorRef.SetActive(true);
                currentUnit.unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.magenta; // just for fun
                currentUnit.unitReference.isSelected = true;
            }
            else
            {
                //UCListCleanup();
                //currentlySelectedUnits.Add(currentUnit);
                //projectorRef.SetActive(true);
                currentUnit.unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.magenta; // just for fun
                currentUnit.unitReference.isSelected = true;
            }
        }
    }

    void OnGUI() // Not great to use onGui here, as it renders above everything.
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = RectangleDragSelection.GetScreenRect(playerCam, mousePosition1, mousePosition2);
            RectangleDragSelection.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            RectangleDragSelection.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }



    //public void SelectToggle_old() {


    //    if (Input.GetMouseButtonUp(0)) {
    //        //Debug.Log("Left MB clicked");

    //        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit)) {
    //            //Debug.Log("Hit!");
    //            //GameObject hitObject = hit.collider.transform.gameObject;
    //            //Debug.Log("I am a " + hit.collider.transform.gameObject.name);
    //            if (hit.collider.CompareTag(playerUnitTag)) {

    //                // Use delegate here.

    //                UnitComponent objectClickedScript = hit.collider.GetComponentInParent<UnitComponent>();  // retrieve from map node note ground tile script

    //                //Debug.Log("BEEP BOOP YES I am a " + hit.transform.gameObject.name);
    //                if (ctrlKeyDown) {
    //                    if (!currentlySelectedUnits.Contains(objectClickedScript)) {

    //                        currentlySelectedUnits.Add(objectClickedScript);

    //                        objectClickedScript.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.magenta; // just for fun
    //                        objectClickedScript.unitReference.isSelected = true;
    //                        //objectClickedScript.unitReference.SelectionFeedback();
    //                    }
    //                    else {
    //                        objectClickedScript.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white; // just for fun
    //                        objectClickedScript.unitReference.isSelected = false;
    //                        //objectClickedScript.unitReference.SelectionFeedback();
    //                        currentlySelectedUnits.Remove(objectClickedScript);
    //                    }
    //                }
    //                else {
    //                    UCListCleanup();
    //                    currentlySelectedUnits.Add(objectClickedScript);

    //                    objectClickedScript.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.magenta; // just for fun
    //                    objectClickedScript.unitReference.isSelected = true;
    //                    //objectClickedScript.unitReference.SelectionFeedback();
    //                }
    //            }
    //        }
    //    }
    //}

    public void UCListCleanup() {

        if (currentlySelectedUnits.Count > 0) {
            foreach (UnitComponent _uc in currentlySelectedUnits) {

                _uc.unitReference.unitModel.GetComponent<MeshRenderer>().material.color = Color.white; // just for fun

                _uc.unitReference.isSelected = false;
                //_uc.unitReference.SelectionFeedback();
            }
            currentlySelectedUnits = new List<UnitComponent>();
            //Debug.Log("UC list cleaned up");
        }
    }
    


}

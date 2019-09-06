using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SetCamsOrder : MonoBehaviour {

    public Canvas[] canvasWithScreenspaceCamera;
    [SerializeField] private static Camera portCam;  // fetched from TerainBuilder_02 upon level creation.


    //float bob = float.MinValue; old, but kept for documenting purposes.

    // Use this for initialization
    void Start () {
		
	}

    public void SetCams(GameObject _mainCam)        // only place stuff you want to have as "screenspace-camera" canvas.
    {
        portCam = _mainCam.GetComponentInChildren<Camera>();

        foreach (Canvas _c in canvasWithScreenspaceCamera)
        {
            _c.worldCamera = portCam;
            _c.planeDistance = 5.0f;
        }
    }
		
}

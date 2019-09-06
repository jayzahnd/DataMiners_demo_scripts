using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReferenceSingleton : MonoBehaviour {

    private static CameraReferenceSingleton instance;
    public static CameraReferenceSingleton Instance
    {
        get
        {
            return instance ?? (instance = new GameObject("Camera Reference Singleton").AddComponent<CameraReferenceSingleton>());
        }
    }

    public GameObject gameplayCamHolderPrefab_01;
    public Camera minimapCamPrefab;

    public void InitialiseCam()
    {
        gameplayCamHolderPrefab_01 = Resources.Load<GameObject>("Cameras/MainCameraHolder");
        minimapCamPrefab = Resources.Load<Camera>("Cameras/MinimapCamera");
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

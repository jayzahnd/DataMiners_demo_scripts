using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner_a01 : MonoBehaviour {

    //public GameObject baseSpawnPoint;
    public Vector3 baseRallyPoint;
    public Vector3 basePosisiton;
    string baseTag = "Base_BuildingMain";

    private static Camera camRef;

    public int displace = 10;
        

    // Use this for initialization
    void Start () {
        UI_ButtonPackAssociator.spawnMinerEvent += SpawnUnit;        

        Vector3 offset = new Vector3(0,0,displace);
        basePosisiton = gameObject.transform.position;
        baseRallyPoint = gameObject.transform.position + offset;
	}

    public static void FetchCameraPrefab(GameObject cam)
    {
        camRef = cam.GetComponentInChildren<Camera>();
        //Debug.Log("Cam set for base spawner");
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnDisable() {
        UI_ButtonPackAssociator.spawnMinerEvent -= SpawnUnit;
    }

    IEnumerator LerpTranslation(Transform objTransform, Vector3 ptA, Vector3 ptB,float speed)
    {
        float ClampedLerpRate = 1.0f / Vector3.Distance(ptA, ptB) * speed;
        float tLerp = 0.0f;
        while (tLerp < 1.0f)
        {
            tLerp += Time.deltaTime * ClampedLerpRate;
            objTransform.transform.position = Vector3.Lerp(ptA, ptB, Mathf.SmoothStep(0.0f, 1.0f, tLerp));
            yield return null;
        }

    }
    
    public void SpawnUnit(int unitTypeIndex) // 0 for Miner, 1 for Brawler, 2 for Vehicle
    {
        //Ray ray = camRef.ScreenPointToRay(Input.mousePosition);
        //RaycastHit lmbHit;

        //if(Physics.Raycast(ray, out lmbHit))
        //{
        //    if (!lmbHit.collider.CompareTag(baseTag))
        //    {
        //        return;
        //    }
        //}

        float xRan = Random.Range(-4.5f, 4.5f);
        float zRan = Random.Range(-4.5f, 4.5f);
        Vector3 radius = new Vector3(xRan,0,zRan);

        if(unitTypeIndex != 2) {  // First we check that the order is not for a vehicle.

            for (int i = 0; i < TerrainBuilder_02.player_inf_Array.Length; i++) {


                if (TerrainBuilder_02.player_inf_Array[i].unitPrefab.GetComponent<StorageStatus>().isStored == true) {

                    if (unitTypeIndex == 0) {  // Set the index to "0" on the button's click event box.

                        GameObject miner = TerrainBuilder_02.player_inf_Array[i].unitPrefab;
                        UnitComponent minerComponent = miner.GetComponent<UnitComponent>() ?? miner.AddComponent<UnitComponent>();
                        minerComponent.enabled = true;


                        Unit_Miner minerProperties = new Unit_Miner();
                        minerComponent.unitReference = minerProperties;
                        minerComponent.unitReference.type = Unit.UnitType.PLAYER_MINER;

                        minerComponent.unitReference.LoadUnitProperties();

                        miner.transform.position = basePosisiton;
                        miner.transform.rotation = transform.rotation;
                        miner.SetActive(true);

                        StartCoroutine(LerpTranslation(miner.transform, basePosisiton, (baseRallyPoint + radius), minerComponent.unitReference.unitSpeed));

                        break;
                    }
                    
                }
                else {
                    continue;
                }
            }
        }
        

        

        //GameObject miner_old = GameObject.Instantiate(minerPrefab, baseRallyPoint + radius, transform.rotation) as GameObject;

        
                
        //miner.name = "Unit";

    }
}

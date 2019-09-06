using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonPackAssociator : MonoBehaviour {

    public static UI_ButtonPackAssociator uiBPA_Instance;

    [SerializeField] public List<GameObject> UIButtonPack;

    /* NOTE ON INDICES:
     * 
     * 0:   BASE
     * 1:   MINER
     * 2:   BRAWLER
     * 3:   VEHICLE 
     * 
     */

    public delegate void SpawnMinerOrder(int unitTypeIndex);
    public static event SpawnMinerOrder spawnMinerEvent;

    public void SpawnUnit(int _unitTypeIndex) {
        spawnMinerEvent(_unitTypeIndex);
    }


    private void Awake() {
        uiBPA_Instance = this;
    }

}

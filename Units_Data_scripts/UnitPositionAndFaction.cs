using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPositionAndFaction : MonoBehaviour {

    public string itsNameIs;
    public bool isPlayer = false;  // true is player controlled, false for AI.
    public Vector3 positionU;

    
    // Use this for initialization
	private void OnEnable () {
        if (gameObject.GetComponent<UnitComponent>())
        {
            isPlayer = true;
        }
        itsNameIs = gameObject.name;

    }
		
	private void FixedUpdate () {
        positionU = transform.position;
	}

    private void OnDisable()
    {
        positionU = Vector3.zero;
    }
}

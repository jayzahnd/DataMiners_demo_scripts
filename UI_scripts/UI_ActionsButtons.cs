using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActionsButtons : MonoBehaviour {
    // This should have a list of buttons that should be enabled when the unit this script is attached to is selected.

    [SerializeField] public GameObject UIButtonPack;
    [SerializeField] private Button[] unitActionButtons;
    
    public void EnableButtons() {

        if(UIButtonPack.activeSelf == false) {
            UIButtonPack.SetActive(true);
        }
    }
    public void DisableButtons() {
        if (UIButtonPack.activeSelf == true) {
            UIButtonPack.SetActive(false);
        }
    }

	// Use this for initialization
	void Start () {
        UnitSelectionManager.disableUIInfoEvent += DisableButtons;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDisable() {
        UnitSelectionManager.disableUIInfoEvent -= DisableButtons;
    }
}

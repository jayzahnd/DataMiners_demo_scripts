using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverUnit : MonoBehaviour {

    [SerializeField]GameObject projectorRef;
    [SerializeField]UnitComponent ucRef;
    
    private void OnEnable()
    {
        projectorRef = transform.parent.transform.GetChild(1).gameObject;
        ucRef = transform.parent.gameObject.GetComponent<UnitComponent>();
    }

    private void OnMouseOver()
    {
        projectorRef.SetActive(true);
        ucRef.isHighlighted = true;
    }
    private void OnMouseExit()
    {
        //Debug.Log(ucRef.unitReference.isSelected == false);
        if (ucRef.unitReference.isSelected == false)
        {
            projectorRef.SetActive(false);
            ucRef.isHighlighted = false;
        }
        
    }
}

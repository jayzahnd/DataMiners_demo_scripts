using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[Serializable]
public class Unit {

    

    public GameObject unitModel; // to implement later on.
    public Animator friendUnitAnimator;

    public enum UnitType { PLAYER_MINER, PLAYER_SOLDIER, ENEMY_VIRUSBOT}
    public UnitType type;

    //public bool isHighlightedOLD;
    public bool isSelected;
    public bool isMoving;

    public float unitHP;
    public float unitSpeed;

    public virtual void LoadUnitProperties()
    {
        //isHighlightedOLD = false;
        isSelected = false;
        isMoving = false;
        //Stuff
    }

}

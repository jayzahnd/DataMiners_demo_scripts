using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlled : MonoBehaviour {

    public GameObject selectionCircle;

    private void OnCollisionEnter(Collision collisionInfo)
    {
        Collider myCollider = GetComponent<Collider>();
        if ((collisionInfo.gameObject.GetComponent<PlayerControlled>()))
        {
            Physics.IgnoreCollision(collisionInfo.collider, myCollider);
        }
    }
}

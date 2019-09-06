using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingScript : MonoBehaviour {

    public Transform player;
    public float range = 50.0f;
    public float bulletImpulse = 5.0f;
    //public float lifetime;

    private bool onRange = false;

    public bool shooting;

    public Rigidbody projectile;

    void Start()
    {
        shooting = false;
        InvokeRepeating("Shoot", 1, 3.0f);
    }

    public void Shoot()
    {

        if (onRange)
        {

            Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            bullet.AddForce(transform.forward * bulletImpulse, ForceMode.Impulse);

            Destroy(bullet.gameObject, 2);
            //Destroy(bullet.gameObject, lifetime);
        }


    }

    public void PrepareToShoot()
    {
        shooting = true;

        if (shooting == true)
        {
           onRange = Vector3.Distance(transform.position, player.position) < range;

            if (onRange)
                transform.LookAt(player);
        }

       
    }

    public void OutOfRange()
    {
        shooting = false;
    }


}
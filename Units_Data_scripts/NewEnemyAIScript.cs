using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyAIScript : MonoBehaviour
{

    public GameObject playerTarget;
    private string playerTag = "PlayerUnit";

    [SerializeField]private GameObject enemyHolder;
    [SerializeField] public GameObject enemyModel;
    public float MovementSpeed;
    public float MaxDist;
    public float MinDist;

    public float EnemyHealth = 5;

    public bool idle = true;
    public bool walking;
    public bool carryidle;
    public bool carrywalk;
    public bool running;
    public bool fighting;
    public bool dying;

    public EnemyAnimationScript enemyAnimationScript;

    public EnemyShootingScript enemyShootingScript;

    //public float sphereRadius;

    //public Animator enemyAnimation;

    // Use this for initialization
    void Start()
    {
        //player = GameObject.Find("Player");
        enemyHolder = transform.gameObject;//GameObject.FindGameObjectWithTag("Enemy");

        //enemyAnimation = gameObject.GetComponent<Animator>();
    }


    //public void FixedUpdate()
    //{   

    // https://blogs.unity3d.com/2017/07/26/spotlight-team-best-practices-collision-performance-optimization/

    //}
    float scanRadius = 15f;
    void SphereScan(float radius) // Should do a checksphere on update, then have overlapsphere whenever checksphere returns true, to return colliders, check for the nearest one, then aggro it.
    {

        if (Physics.CheckSphere(transform.position, radius, 1 << 14))
        {
            //Debug.Log("Detection"); // Works

            Collider[] hitColliders;

        }
        
        //RaycastHit hitInfoSphere;

        //if (Physics.SphereCast(transform.position, 15, Vector3.zero, out hitInfoSphere))
        //{

            
        //    idle = false;

        //    playerTarget = hitInfoSphere.collider.transform.gameObject; // the player unit model

        //    enemyHolder.transform.rotation = Quaternion.Slerp(enemyHolder.transform.rotation
        //                             , Quaternion.LookRotation(playerTarget.transform.position - enemyHolder.transform.position), 3 * Time.deltaTime);


        //} else
        //{
        //    idle = true;
        //    playerTarget = null;
        //}
    }

    void CloseInAndFight(GameObject _targetUnit)
    {
        if (_targetUnit != null && Vector3.Distance(transform.position, _targetUnit.transform.position) <= MaxDist)
        {

            //transform.position += transform.forward * MovementSpeed * Time.deltaTime;

            enemyHolder.transform.position += enemyHolder.transform.forward * MovementSpeed * Time.deltaTime;

            //enemyAnimation.Play("Enemy Walk");

            walking = true;

            Debug.Log("Enemy Moving!");

            if (walking == true)
            {
                enemyAnimationScript.EnemyWalk();
            }

            enemyShootingScript.OutOfRange();


            if (Vector3.Distance(transform.position, _targetUnit.transform.position) <= MinDist)
            {
                walking = false;

                if (walking == false)
                {
                    fighting = true;
                }

                if (fighting == true)
                {
                    enemyAnimationScript.EnemyFight();
                }


                //StartCoroutine(StartShootingAtFriendUnit());
                enemyShootingScript.PrepareToShoot();

                Debug.Log("Enemy Stopped Moving!");

                //enemyAnimation.Play("Enemy Fight");

            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        


        if (idle == true)
        {
            enemyAnimationScript.EnemyIdle();
        }

        if (playerTarget != null)
        {
            CloseInAndFight(playerTarget);
        }

        

    }

    private void FixedUpdate()
    {
        SphereScan(scanRadius);
    }

    public void GotHit(float Damage)
    {
        EnemyHealth -= Damage;

        if (EnemyHealth <= 0)
        {
            enemyHolder.transform.position += enemyHolder.transform.forward * MovementSpeed * Time.deltaTime;
            enemyAnimationScript.EnemyDead();
        }
    }

    //IEnumerator StartShootingAtFriendUnit()
    //{
        //enemyShootingScript.Shoot();
        //yield return new WaitForSeconds(2);
        
    //}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour {

    public GameObject enemyModelAnimation;
    public Animator enemyAnimation;

	// Use this for initialization
	void Start () {

        enemyAnimation = gameObject.GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
		


	}

    public void EnemyIdle()
    {
        enemyAnimation.Play("Enemy Idle");
    }

    public void EnemyWalk()
    {
        enemyAnimation.Play("Enemy Walk");
    }

    public void EnemyCarryIdle()
    {
        enemyAnimation.Play("Enemy Idle (Carry)");
    }

    public void EnemyCarryWalk()
    {
        enemyAnimation.Play("Enemy Walk (Carry)");
    }

    public void EnemyRun()
    {
        enemyAnimation.Play("Enemy Run");
    }

    public void EnemyFight()
    {
        enemyAnimation.Play("Enemy Fight");
    }

    public void EnemyDead()
    {
        enemyAnimation.Play("Enemy Dead");
    }
}

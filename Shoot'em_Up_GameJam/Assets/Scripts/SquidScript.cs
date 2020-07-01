using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidScript : BaseEnemy
{
    protected override void Start()
    {
        int randomDimension = Random.Range(0, 2);
        if (randomDimension == 0)
        {
            actualDimension = EnemyManager.instance.dimensionALayout;
            for (int i = 0; i < transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = 8;
            }
            gameObject.layer = 8;
        }
        else
        {
            actualDimension = EnemyManager.instance.dimensionBLayout;
            for (int i = 0; i < transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = 11;
            }
            gameObject.layer = 11;
        }
        EnemyManager.instance.enemyList.Add(gameObject);
        EnemyManager.instance.CheckLight(gameObject);
        enemyAnimator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ship = GameObject.Find("Ship");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidScript : BaseEnemy
{
    protected override void Start()
    {
        base.Start();
        enemyAnimators.Clear();
        enemyAnimators.Add(transform.GetChild(0).GetComponent<Animator>());
        rb = GetComponent<Rigidbody2D>();
    }

}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidScript : BaseEnemy
{
    protected override void Start()
    {
        enemyAnimator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ship = GameObject.Find("Ship");
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected float speed = 0,freezeDuration,burnDuration;
    [SerializeField] protected int hp, expDrop;
    protected bool freeze = false,burn = false ;
    protected Rigidbody2D rb;
    protected GameObject ship;
    public LayerMask actualDimension;
    private float tickBurn = 1f;
    protected Animator enemyAnimator;
    public GameObject light;

    protected virtual void Start()
    {
        int randomDimension = Random.Range(0, 2);
        if(randomDimension ==0)
        {
            actualDimension = EnemyManager.instance.dimensionALayout;
            for(int i = 0; i<transform.childCount;i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = 8;
            }
            gameObject.layer = 8;
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.layer = 11;
            }
            actualDimension = EnemyManager.instance.dimensionBLayout;
            gameObject.layer = 11;
        }
        EnemyManager.instance.enemyList.Add(gameObject);
        EnemyManager.instance.CheckLight(gameObject);
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ship = GameObject.Find("Ship");
    }

    protected virtual void Update()
    {
        RotateToPlayer();
    }

    protected virtual void RotateToPlayer() => transform.right = (Vector2)(ship.transform.position - transform.position);
    public virtual void TakeDammage(int damage)
    {
        hp -= damage;
        enemyAnimator.SetTrigger("Hurt");
        if (hp <= 0)
        {
            Died();
        }
    }

    protected virtual void FixedUpdate() //Se déplacer vers le joueur
    {
        if(!freeze)
        {
            Vector2 dir = (ship.transform.position - transform.position).normalized;
            rb.position += dir * speed * Time.fixedDeltaTime;
        }
        
    }


    private void Died()
    {
        ParticuleManagerScript.instance.CreateExplosion(transform.position);
        SliderManager.instance.GainExp(expDrop);
        EnemyManager.instance.enemyList.Remove(gameObject);
        Destroy(gameObject);
    }


    public void Freeze()
    {
        StartCoroutine(FreezeTimer());
    }

    public void Burn()
    {
        if(!burn)
        {
            ColorRedEnemy();
            burn = true;
            StartCoroutine(BurnTimer(burnDuration));
            
        }
        
    }

    public IEnumerator BurnTimer(float burnDurationLeft)
    {
        yield return new WaitForSeconds(tickBurn);
        burnDurationLeft -= tickBurn;
        if(burnDurationLeft > 0)
        {
            StartCoroutine(BurnTimer(burnDurationLeft));
            this.TakeDammage(1);
        }
        else
        {
            ResetColorEnemy();
            burn = false;
        }
    }
    public IEnumerator FreezeTimer()
    {
        ColorBlueEnemy();
        freeze = true;
        yield return new WaitForSeconds(freezeDuration);
        ResetColorEnemy();
        freeze = false;
    }

    public void ResetColorEnemy()
    {
        for(int child = 0; child < transform.childCount;child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void ColorRedEnemy()
    {
        for (int child = 0; child < transform.childCount; child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void ColorBlueEnemy()
    {
        for (int child = 0; child < transform.childCount; child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color =  new Color(0, 54, 255);
        }
    }

    public virtual void AddHp(int hp)
    {
        this.hp += hp;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerScript>().TakeDamage(1);
        }

    }

}

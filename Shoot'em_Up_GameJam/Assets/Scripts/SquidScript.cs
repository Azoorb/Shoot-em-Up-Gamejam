using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidScript : MonoBehaviour,IEnemy
{
    [SerializeField] private float speed = 0;
    [SerializeField] private int hp;

    private Rigidbody2D rb;
    private GameObject ship;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ship = GameObject.Find("Ship");
    }

    void Update()
    {
        RotateToPlayer();
    }

    private void RotateToPlayer() => transform.right = (Vector2)(ship.transform.position - transform.position); //Rotate vers le joueur

    private void FixedUpdate() //Se déplacer vers le joueur
    {
        Vector2 dir = (ship.transform.position - transform.position).normalized;
        rb.position += dir * speed * Time.fixedDeltaTime;
    }

    public void TakeDammage(int damage)
    {
        hp -= damage;
        if(hp<=0)
        {
            Died();
        }
    }

    private void Died()
    {
        ParticuleManagerScript.instance.CreateExplosion(transform.position);
        Destroy(gameObject);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{
    [SerializeField] private float speed;

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

    private void RotateToPlayer() => transform.right = ship.transform.position - transform.position; //Rotate vers le joueur

    private void FixedUpdate() //Se déplacer vers le joueur
    {
        Vector2 dir = (ship.transform.position - transform.position).normalized;
        rb.position += dir * speed * Time.fixedDeltaTime;
    }
}

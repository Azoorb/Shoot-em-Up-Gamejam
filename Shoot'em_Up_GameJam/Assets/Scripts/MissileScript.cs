using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float speed;

    private GameObject ship;
    private Rigidbody2D rb;
    private Vector2 move;

    void Start()
    {
        ship = GameObject.Find("Ship");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move = (ship.transform.position - transform.position).normalized * speed;
        transform.up = ship.transform.position - transform.position;

        if (lifeTime <= 0)
            Destroy(gameObject);
        else
            lifeTime -= Time.deltaTime;
    }

    private void FixedUpdate() => rb.position += move * Time.fixedDeltaTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
            lifeTime -= 1;
    }
}

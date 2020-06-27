using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyGunnerScript : MonoBehaviour,IEnemy
{
    [SerializeField] private float speed,minDistance,fireRate;
    [SerializeField] private int hp;
    private bool readyToShoot = true,canShoot;
    private Rigidbody2D rb;
    private GameObject ship;
    [SerializeField]
    GameObject prefabShoot,spawnShoot;
    Collider2D colliderEnemy;
    void Start()
    {
        colliderEnemy = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        ship = GameObject.Find("Ship");
        
    }

    void Update()
    {
        RotateToPlayer();
        if(readyToShoot && canShoot)
        {
            StartCoroutine(TimerShoot());
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet =  Instantiate(prefabShoot, spawnShoot.transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(colliderEnemy, bullet.GetComponent<Collider2D>());
        bullet.GetComponent<BulletEnemyScript>().SetTarget(spawnShoot.transform.position - ship.transform.position);

    }
    private IEnumerator TimerShoot()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(fireRate);
        readyToShoot = true;
    }

    private void RotateToPlayer() => transform.right = (Vector2)(ship.transform.position - transform.position); //Rotate vers le joueur

    private void FixedUpdate() //Se déplacer vers le joueur
    {
        
        if (Vector2.Distance(ship.transform.position, transform.position) > minDistance)
        {
            Vector2 dir = (ship.transform.position - transform.position).normalized;
            rb.position += dir * speed * Time.fixedDeltaTime;
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }
   
        
    }


    public void TakeDammage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
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


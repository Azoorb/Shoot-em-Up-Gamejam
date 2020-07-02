using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyGunnerScript : BaseEnemy
{
    [SerializeField] private float minDistance, fireRate;
    private bool readyToShoot = true, canShoot;
    [SerializeField]
    GameObject prefabShoot, spawnShoot;
    Collider2D colliderEnemy;

    protected override void Start()
    {
        base.Start();
        colliderEnemy = GetComponent<Collider2D>();

    }

    protected override void Update()
    {
        base.Update();
        if (readyToShoot && canShoot)
        {
            StartCoroutine(TimerShoot());
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(prefabShoot, spawnShoot.transform.position, Quaternion.identity);

        bullet.GetComponent<BulletEnemyScript>().SetTarget(spawnShoot.transform.position - PlayerScript.instance.transform.position);

    }
    private IEnumerator TimerShoot()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(fireRate);
        readyToShoot = true;
    }




    protected override void FixedUpdate() //Se déplacer vers le joueur
    {

        if (Vector2.Distance(PlayerScript.instance.transform.position, transform.position) > minDistance)
        {
            base.FixedUpdate();
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }


    }
}


    

    


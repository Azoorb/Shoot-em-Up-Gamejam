using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Controller controller;
    Vector2 vectorMovement,vectorAim;
    bool canShoot = true, canLaser = false;
    [SerializeField]
    public float speed ,fireRate,laserRate,laserTime,timeAfterNextDash,dashForce;
    [SerializeField]
    GameObject bulletPrefab,laserPrefab,spawnBullet;
    Collider2D colliderShip;
    private bool canDash = true;
    [SerializeField]
    Animator boostAnim;
    [SerializeField]
    int hp;
    

    private void Awake()
    {
        controller = new Controller();

        controller.Player.Movement.performed += ctx => vectorMovement = ctx.ReadValue<Vector2>();
        controller.Player.Movement.canceled += ctx => vectorMovement = Vector2.zero;
        controller.Player.Aim.performed += ctx => vectorAim = ctx.ReadValue<Vector2>();
        controller.Player.Aim.canceled += ctx => vectorAim = Vector2.zero;
        controller.Player.Dash.performed += ctx =>
        {
            if (canDash)
            {
                Dash();
            }
        };
        colliderShip = GetComponent<Collider2D>();
        Physics2D.IgnoreLayerCollision(8, 9);

    }

    private void Update()
    {
        if(vectorMovement != Vector2.zero)
        {
            boostAnim.SetBool("Move", true);
        }
        else
        {
            boostAnim.SetBool("Move", false);
        }
        if(vectorAim != Vector2.zero)
        {
            MakeRotation(vectorAim);
        }
        else if(vectorMovement != Vector2.zero)
        {
            MakeRotation(vectorMovement);
        }
        if (vectorAim != Vector2.zero && canShoot)
        {
            LevelManager.instance.GainLevel();
            Shoot();
            
        }
        
    }

    private void MakeRotation(Vector2 vector)
    {
        float angle = Mathf.Atan2(-vector.x, vector.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnBullet.transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(colliderShip, bullet.GetComponent<Collider2D>());
        bullet.GetComponent<BulletScript>().SetTarget(vectorAim);
        StartCoroutine(ShootTimer());
    }

    private void Lazer()
    {
        GameObject laser = Instantiate(laserPrefab, spawnBullet.transform);
        StartCoroutine(LaserTimer());
    }


    private IEnumerator ShootTimer()
    {
        canShoot = false;
        canLaser = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        canLaser = true;
    }

    private IEnumerator LaserTimer()
    {
        canShoot = false;
        canLaser = false;
        yield return new WaitForSeconds(laserTime);
        canShoot = true;
        yield return new WaitForSeconds(laserRate);
        canLaser = true;
    }


    private void FixedUpdate()
    {
        transform.parent.Translate(vectorMovement * speed * Time.deltaTime);
        
        
    }

    private void Dash()
    {
        transform.parent.GetComponent<Rigidbody2D>().AddForce(vectorMovement * dashForce);
        StartCoroutine(TimerDash(timeAfterNextDash));
        StartCoroutine(StopDash());
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(0.1f);
        transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private IEnumerator TimerDash(float timeAfterNextDash)
    {
        canDash = false;
        yield return new WaitForSeconds(timeAfterNextDash);
        canDash = true;
    }

    private void OnEnable()
    {
        controller.Player.Enable();
    }

    private void OnDisable()
    {
        controller.Player.Disable();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }
}

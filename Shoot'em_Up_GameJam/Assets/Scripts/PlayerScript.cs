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
    bool canShoot = true;
    [SerializeField]
    float speed ,fireRate,timeAfterNextDash,dashForce;
    [SerializeField]
    GameObject bulletPrefab,spawnBullet;
    Collider2D colliderShip;
    private bool canDash = true;
    [SerializeField]
    Animator boostAnim;

    

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
            float angle = Mathf.Atan2(-vectorAim.x, vectorAim.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
        }
        else if(vectorMovement != Vector2.zero)
        {
            float angle = Mathf.Atan2(-vectorMovement.x, vectorMovement.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
        }
        if (vectorAim != Vector2.zero && canShoot)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnBullet.transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(colliderShip, bullet.GetComponent<Collider2D>());
            bullet.GetComponent<BulletScript>().SetTarget(vectorAim);
            StartCoroutine(ShootTimer());
        }
    }

    private IEnumerator ShootTimer()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
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
}

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
    float speed,fireRate;
    [SerializeField]
    GameObject bulletPrefab;
    Collider2D colliderShip;
    

    private void Awake()
    {
        controller = new Controller();

        controller.Player.Movement.performed += ctx => vectorMovement = ctx.ReadValue<Vector2>();
        controller.Player.Movement.canceled += ctx => vectorMovement = Vector2.zero;
        controller.Player.Aim.performed += ctx => vectorAim = ctx.ReadValue<Vector2>();
        controller.Player.Aim.canceled += ctx => vectorAim = Vector2.zero;
        colliderShip = GetComponent<Collider2D>();
        
    }

    private void Update()
    {
        float angle = Mathf.Atan2(-vectorAim.x, vectorAim.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
        if (vectorAim != Vector2.zero && canShoot)
        {
            //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //Physics2D.IgnoreCollision(colliderShip, bullet.GetComponent<Collider2D>());
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

    private void OnEnable()
    {
        controller.Player.Enable();
    }

    private void OnDisable()
    {
        controller.Player.Disable();
    }
}

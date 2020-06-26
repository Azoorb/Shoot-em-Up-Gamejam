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
    float speed;
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
        if(vectorAim != Vector2.zero && canShoot)
        {
            transform.Rotate(vectorAim);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(colliderShip, bullet.GetComponent<Collider2D>());
        }
    }


    private void FixedUpdate()
    {
        transform.Translate(vectorMovement * speed * Time.deltaTime);
        if(vectorMovement.x > 0)
        {
            
        }
        else if ( vectorMovement.x < 0)
        {
            //Animation gauche
        }
        if(vectorMovement.y > 0)
        {
            //animation haut
        }
        if (vectorMovement.y < 0)
        {
            //animation bas
        }
        if(vectorMovement == Vector2.zero)
        {
            //animation idle;
        }
    }

    private void OnEnable()
    {
        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }
}

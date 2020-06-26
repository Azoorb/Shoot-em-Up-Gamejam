using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Controller controller;
    Vector2 vectorMovement;
    [SerializeField]
    private float speed;

    private void Awake()
    {
        controller = new Controller();
        controller.Player.Movement.performed += ctx => vectorMovement = ctx.ReadValue<Vector2>();
        controller.Player.Movement.canceled += ctx => vectorMovement = Vector2.zero;
    }


    private void FixedUpdate()
    {
        transform.Translate(vectorMovement * speed * Time.deltaTime);
        if(vectorMovement.x > 0)
        {
            //Animation droite;
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

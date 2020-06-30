using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class P40WScript : MonoBehaviour
{
    private GameObject ship;
    private GameObject body;
    private Rigidbody2D rb;

    private int lastAttack;

    [SerializeField] float timeBeetweenAttacks;

    [Header("Missiles")]
    [SerializeField] GameObject missilePrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] int missilesCount;
    [SerializeField] float timeBeetweenMissiles;

    [Header("Charges")]
    [SerializeField] float chargeSpeed;
    [SerializeField] int chargesCount;
    private int chargesLeft;
    [SerializeField] float stunTime, timeBeforeCharge;
    private Vector2 chargeMovement;
    private bool isCharging = false, stunned = false;

    private void Start()
    {
        ship = GameObject.Find("Ship");
        body = GetComponentsInChildren<SpriteRenderer>()[0].gameObject;
        rb = GetComponent<Rigidbody2D>();

        SetupAnimatons();
        StartCoroutine(NewAttack());

        Physics2D.IgnoreLayerCollision(8, 9);
    }

    void Update()
    {
        if (!isCharging && !stunned)
            body.transform.up = -(Vector2)(ship.transform.position - transform.position);
    }

    private void SetupAnimatons()
    {
        Animator[] anims = GetComponentsInChildren<Animator>();

        foreach (Animator a in anims)
        {
            if (a.gameObject.name == "14")
                a.SetBool("14", true);
        }
    }

    IEnumerator NewAttack()
    {
        yield return new WaitForSeconds(timeBeetweenAttacks);

        //a terminer
        int i = Random.Range(0, 2);
        while (i == lastAttack)
            i = Random.Range(0, 2);

        lastAttack = i;

        if (i == 0)
            StartCoroutine(A_Missiles());
        else if (i == 1)
            StartCoroutine(A_Charge());
    }


    //ATTACK fireballs
    private IEnumerator A_Missiles()
    {
        int _count = missilesCount;

        while (_count > 0)
        {
            GameObject fb = Instantiate(missilePrefab, shootPoint.position, Quaternion.identity);

            _count--;

            yield return new WaitForSeconds(timeBeetweenMissiles);
        }

        StartCoroutine(NewAttack());
    }



    //ATTACK charges
    private IEnumerator A_Charge()
    {
        yield return new WaitForSeconds(timeBeforeCharge);

        if (chargesLeft == 0)
            chargesLeft = chargesCount;

        chargesLeft -= 1;

        chargeMovement = (ship.transform.position - transform.position).normalized * chargeSpeed;

        isCharging = true;
    }
    private IEnumerator Stun()
    {
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;

        if (chargesLeft > 0)
            StartCoroutine(A_Charge());
        else
            StartCoroutine(NewAttack());
            
    }
    private void FixedUpdate()
    {
        if (isCharging)
            rb.position += chargeMovement * Time.fixedDeltaTime;
    }
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Terrain") && isCharging)
        {
            isCharging = false;
            StartCoroutine(Stun());
        }
    }
}

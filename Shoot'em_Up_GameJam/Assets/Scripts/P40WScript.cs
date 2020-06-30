using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class P40WScript : MonoBehaviour
{
    private GameObject ship;
    private GameObject renderer;

    private string[] attacks;
    private string lastAttack;
    [SerializeField] float timeBeetweenAttacks;

    [Header("Fireball")]
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] int fireballsCount;
    [SerializeField] float timeBeetweenFireballs;

    private void Start()
    {
        ship = GameObject.Find("Ship");
        renderer = GetComponentsInChildren<SpriteRenderer>()[0].gameObject;

        SetupAnimatons();
        StartCoroutine(NewAttack());
    }

    void Update() => renderer.transform.up = -(Vector2)(ship.transform.position - transform.position);

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
        StartCoroutine(A_Fireballs());
    }


    //ATTACK fireballs
    private IEnumerator A_Fireballs()
    {
        int _count = fireballsCount;

        while (_count > 0)
        {
            GameObject fb = Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);
            fb.GetComponent<BulletEnemyScript>().SetTarget(shootPoint.position - ship.transform.position);

            _count--;

            yield return new WaitForSeconds(timeBeetweenFireballs);
        }

        StartCoroutine(NewAttack());
    }
}

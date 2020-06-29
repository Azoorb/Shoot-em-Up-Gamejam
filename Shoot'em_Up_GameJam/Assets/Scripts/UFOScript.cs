using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class UFOScript : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed, tpDistToPlayer, tpRate, maxDist;
    [SerializeField] private int hp, expDrop;

    private Rigidbody2D rb;
    private GameObject ship;

    private bool readyTp = true, wantTp, mustTp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ship = GameObject.Find("Ship");

        MetasSpriteSetup();
    }

    private void FixedUpdate()
    {
        Vector2 dir = (ship.transform.position - transform.position).normalized;
        rb.position += dir * speed * Time.fixedDeltaTime;
    }

    private void Update()
    {
        RotateToPlayer();
        if (Vector2.Distance(ship.transform.position, transform.position) >= maxDist)
        {
            wantTp = true;
        }
        else
        {
            wantTp = false;
        }

        if (readyTp && (wantTp || mustTp))
        {
            StartCoroutine(TimerTeleport());
            Teleport();
        }
    }

    private void RotateToPlayer() => transform.up = (Vector2)(ship.transform.position - transform.position); //Rotate vers le joueur

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("PlayerBullet"))
        {
            mustTp = true;
        }
    }

    private void Teleport()
    {
        Vector2 tpVector = new Vector2(Random.Range(-1, 1),Random.Range(-1, 1)).normalized * tpDistToPlayer;
        rb.position = (Vector2)ship.transform.position + tpVector;
    }

    private IEnumerator TimerTeleport()
    {
        readyTp = false;
        wantTp = false;
        mustTp = false;
        yield return new WaitForSeconds(tpRate);
        readyTp = true;
    }

    private void MetasSpriteSetup()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();

        //Debug.Log(animators.Length);

        foreach (Animator a in animators)
        {
            if (a.gameObject.name == "Left")
                a.SetBool("Left", true);
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
        SliderManager.instance.GainExp(expDrop);
        Destroy(gameObject);
    }
}

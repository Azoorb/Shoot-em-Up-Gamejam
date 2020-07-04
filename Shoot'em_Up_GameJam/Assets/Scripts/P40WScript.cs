using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class P40WScript : BaseEnemy
{
    private GameObject body;
    private Animator[] anims;
    [SerializeField]
    Slider sliderBoss;

    private int lastAttack;

    [Header("P40W Settings")]
    [SerializeField] GameObject lifeFiller;
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

    [Header("Ennemy Spawn")]
    [SerializeField] GameObject[] enemiesPrefabs;
    [SerializeField] float spawnTime;

    protected override void Start()
    {
        base.Start();
        AddHp(0);
        body = GetComponentsInChildren<SpriteRenderer>()[0].gameObject;
        rb = GetComponent<Rigidbody2D>();
        sliderBoss.maxValue = hp;

        SetupAnimations();
        StartCoroutine(NewAttack());

    }

    protected override void  Update()
    {
        if(PlayerScript.instance != null)
        {
            if (!isCharging && !stunned)
                body.transform.up = -(Vector2)(PlayerScript.instance.transform.position - transform.position);
        }
        
    }

    private void SetupAnimations()
    {
        anims = GetComponentsInChildren<Animator>();

        foreach (Animator a in anims)
        {
            if (a.gameObject.name == "14")
                a.SetBool("14", true);
        }
    }
    protected override void OnCollisionStay2D(Collision2D c)
    {
        base.OnCollisionStay2D(c);
        if (c.gameObject.CompareTag("Terrain") && isCharging)
        {
            isCharging = false;
            StartCoroutine(Stun());
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {        
        if (c.gameObject.CompareTag("PlayerBullet"))
        {
            foreach (Animator a in anims)
            {
                a.SetTrigger("Hit");
            }
        }
    }



    IEnumerator NewAttack()
    {
        yield return new WaitForSeconds(timeBeetweenAttacks);

        //a terminer
        int i = Random.Range(0, 3);

        while (i == lastAttack)
            i = Random.Range(0, 3);

        lastAttack = i;

        if (i == 0)
            StartCoroutine(A_Missiles());
        else if (i == 1)
            StartCoroutine(A_Charge());
        else if (i == 2)
            StartCoroutine(A_Spawn());
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

        if(PlayerScript.instance != null)
        {
            chargeMovement = (PlayerScript.instance.transform.position - transform.position).normalized * chargeSpeed;
        }

        

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
    protected override void FixedUpdate()
    {
        if (isCharging)
            rb.position += chargeMovement * Time.fixedDeltaTime;
    }



    //ATTTACK spawn
    private IEnumerator A_Spawn()
    {
        foreach (GameObject e in enemiesPrefabs)
        {
            yield return new WaitForSeconds(spawnTime);

            Instantiate(e, transform.position, Quaternion.identity);
            if (Random.Range(0, 2) == 1)
                Instantiate(e, transform.position, Quaternion.identity);

        }

        StartCoroutine(NewAttack());
    }




    //ENEMY BASE

    public override void TakeDammage(int damage)
    {
        base.TakeDammage(damage);
        Debug.Log("yo");
        sliderBoss.value -= damage;
        
    }
    private void Died()
    {
        ParticuleManagerScript.instance.CreateExplosion(transform.position);
        SliderManager.instance.GainExp(expDrop);
        Destroy(gameObject);
    }

    public override void AddHp(int hp)
    {
        base.AddHp(hp);
        sliderBoss.maxValue = this.hp;
        sliderBoss.value = this.hp;
    }






}

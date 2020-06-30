using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class P40WScript : MonoBehaviour, IEnemy
{
    private GameObject ship;
    private GameObject body;
    private Rigidbody2D rb;

    private int lastAttack;

    [Header("P40W Settings")]
    [SerializeField] int life;
    private int maxLife;
    [SerializeField] int expDrop;
    [SerializeField] float timeBeetweenAttacks;
    private bool freeze = false, burn = false;
    private RectTransform rt;

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

    private void Start()
    {
        ship = GameObject.Find("Ship");
        body = GetComponentsInChildren<SpriteRenderer>()[0].gameObject;
        rb = GetComponent<Rigidbody2D>();
        rt = GetComponentsInChildren<RectTransform>()[1];

        maxLife = life;

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
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Terrain") && isCharging)
        {
            isCharging = false;
            StartCoroutine(Stun());
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

    public void TakeDammage(int damage)
    {
        Debug.Log("HIT");

        life -= damage;

        if (life <= 0)
        {
            Died();
        }
        else
        {
            rt.localScale = new Vector3(life / maxLife, 1f, 1f);
        }
    }
    private void Died()
    {
        ParticuleManagerScript.instance.CreateExplosion(transform.position);
        SliderManager.instance.GainExp(expDrop);
        Destroy(gameObject);
    }

    public void Freeze()
    {
        StartCoroutine(FreezeTimer());
    }

    public void Burn()
    {
        if (!burn)
        {
            ColorRedEnemy();
            burn = true;
            StartCoroutine(BurnTimer(0.5f));

        }
    }

    public void AddHp(int hp) => Debug.Log("Add hp");

    public IEnumerator BurnTimer(float burnDurationLeft)
    {
        yield return new WaitForSeconds(0.5f);
        burnDurationLeft -= 0.5f;
        if (burnDurationLeft > 0)
        {
            StartCoroutine(BurnTimer(burnDurationLeft));
            TakeDammage(1);
        }
        else
        {
            ResetColorEnemy();
            burn = false;
        }
    }
    public IEnumerator FreezeTimer()
    {
        ColorBlueEnemy();
        freeze = true;
        yield return new WaitForSeconds(0.5f);
        ResetColorEnemy();
        freeze = false;
    }

    public void ResetColorEnemy()
    {
        for (int child = 0; child < transform.childCount; child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void ColorRedEnemy()
    {
        for (int child = 0; child < transform.childCount; child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void ColorBlueEnemy()
    {
        for (int child = 0; child < transform.childCount; child++)
        {
            transform.GetChild(child).GetComponent<SpriteRenderer>().color = new Color(0, 54, 255);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected float speed = 0,freezeDuration,burnDuration;
    [SerializeField] protected int hp, expDrop;
    protected bool freeze = false,burn = false ;
    protected Rigidbody2D rb;
    protected GameObject ship;
    private float tickBurn = 1f;
    protected Animator enemyAnimator;

    protected virtual void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ship = GameObject.Find("Ship");
    }

    protected virtual void Update()
    {
        RotateToPlayer();
    }

    protected virtual void RotateToPlayer() => transform.right = (Vector2)(ship.transform.position - transform.position);
    public virtual void TakeDammage(int damage)
    {
        hp -= damage;
        enemyAnimator.SetTrigger("Hurt");
        if (hp <= 0)
        {
            Died();
        }
    }

    protected virtual void FixedUpdate() //Se déplacer vers le joueur
    {
        if(!freeze)
        {
            Vector2 dir = (ship.transform.position - transform.position).normalized;
            rb.position += dir * speed * Time.fixedDeltaTime;
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
        if(!burn)
        {
            ColorRedEnemy();
            burn = true;
            StartCoroutine(BurnTimer(burnDuration));
            
        }
        
    }

    public IEnumerator BurnTimer(float burnDurationLeft)
    {
        yield return new WaitForSeconds(tickBurn);
        burnDurationLeft -= tickBurn;
        if(burnDurationLeft > 0)
        {
            StartCoroutine(BurnTimer(burnDurationLeft));
            this.TakeDammage(1);
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
        yield return new WaitForSeconds(freezeDuration);
        ResetColorEnemy();
        freeze = false;
    }

    public void ResetColorEnemy()
    {
        for(int child = 0; child < transform.childCount;child++)
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
            transform.GetChild(child).GetComponent<SpriteRenderer>().color =  new Color(0, 54, 255);
        }
    }

    public void AddHp(int hp)
    {
        this.hp += hp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerScript>().TakeDamage(1);
        }

    }

}

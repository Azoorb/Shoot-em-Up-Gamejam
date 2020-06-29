using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] protected float speed = 0,freezeDuration;
    [SerializeField] protected int hp, expDrop;
    protected bool freeze;
    protected Rigidbody2D rb;
    protected GameObject ship;

    protected virtual void Start()
    {
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
    public IEnumerator FreezeTimer()
    {
        freeze = true;
        yield return new WaitForSeconds(freezeDuration);
        freeze = false;
    }
}

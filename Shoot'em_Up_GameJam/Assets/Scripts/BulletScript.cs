using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    Vector2 target;
    [SerializeField]
    float speedBullet;
    [SerializeField]
    int damage;
    [SerializeField]
    float liveTime;
    [SerializeField]
    Sprite normalBulletSprite, fireBulletSprite, iceBulletSprite;
    bool ice, fire;

    public void InitializeBullet(Vector2 target,bool ice,bool fire)
    {
        SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.ice = ice;
        this.fire = fire;
        if(ice)
        {
            spriteRenderer.sprite = iceBulletSprite;
        }
        else if(fire)
        {
            spriteRenderer.sprite = fireBulletSprite;
        }
        else
        {
            spriteRenderer.sprite = normalBulletSprite;
        }

        SetTarget(target);
    }

    
    public void SetTarget(Vector2 target)
    {
        this.target = target;
        float angle = Mathf.Atan2(-target.x, target.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.GetChild(0).rotation = rotation;
        StartCoroutine(LifeTime(liveTime));
        
    }

    private IEnumerator LifeTime(float liveTime)
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.Translate(target.normalized * speedBullet * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                if(ice)
                {
                    collision.gameObject.GetComponent<IEnemy>().Freeze();
                }
                collision.gameObject.GetComponent<IEnemy>().TakeDammage(damage+ GameObject.Find("ShipRenderer").GetComponent<PlayerScript>().damageBonus);
            }
            Destroy(gameObject);
        }
        
    }
}

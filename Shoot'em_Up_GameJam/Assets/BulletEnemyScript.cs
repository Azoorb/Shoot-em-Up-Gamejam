using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyScript : MonoBehaviour
{
    Vector2 target;
    [SerializeField]
    float speedBullet;
    [SerializeField]
    int damage;
    [SerializeField]
    float liveTime;


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
        Debug.Log("Yo c'est ma target :" + target);
        transform.Translate(-target.normalized * speedBullet * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }

    }
}

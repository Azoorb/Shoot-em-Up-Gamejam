
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Controller controller;
    Vector2 vectorMovement,vectorAim;
    bool canShoot = true, canLaser = false;
    [SerializeField]
    public float speed ,fireRate,laserRate,laserTime,timeAfterNextDash,dashForce;
    public int damageBonus;
    public float freezeProbabily = 0,fireProbabily = 0;
    [SerializeField]
    GameObject bulletPrefab,laserPrefab,spawnBullet,spawnLaser,boostDashObject,boostObject;
    Collider2D colliderShip;
    private bool canDash = true;
    private bool canTakeDamage = true;
    public LayerMask actualDimension;
    [SerializeField]
    public LayerMask layerDimensionA,layerDimensionB;
    [SerializeField]
    Animator boostAnim;
    Animator shipAnim;
    [SerializeField]
    int hp;
    

    private void Awake()
    {
        controller = new Controller();

        controller.Player.Movement.performed += ctx => vectorMovement = ctx.ReadValue<Vector2>();
        controller.Player.Movement.canceled += ctx => vectorMovement = Vector2.zero;
        controller.Player.Aim.performed += ctx => vectorAim = ctx.ReadValue<Vector2>();
        controller.Player.Aim.canceled += ctx => vectorAim = Vector2.zero;
        controller.Player.Dash.performed += ctx =>
        {
            if (canDash)
            {
                Dash();
            }
        };
        controller.Player.Laser.performed += ctx => Laser();
        controller.Player.SwitchDimension.performed += ctx => ChangeDimension();
        
        colliderShip = GetComponent<Collider2D>();
        Physics2D.IgnoreLayerCollision(8, 9);
        Physics2D.IgnoreLayerCollision(8, 10);
        Physics2D.IgnoreLayerCollision(9, 10);
        Physics2D.IgnoreLayerCollision(11, 10);
        Physics2D.IgnoreLayerCollision(11, 9);
        Physics2D.IgnoreLayerCollision(11, 8);
        Physics2D.IgnoreLayerCollision(12, 8, false);
        Physics2D.IgnoreLayerCollision(12, 11, true);
        shipAnim = GetComponent<Animator>();
        actualDimension = layerDimensionA;

    }

    private void Update()
    {
        if(vectorMovement != Vector2.zero)
        {
            boostAnim.SetBool("Move", true);
        }
        else
        {
            boostAnim.SetBool("Move", false);
        }
        if(vectorAim != Vector2.zero)
        {
            MakeRotation(vectorAim);
        }
        else if(vectorMovement != Vector2.zero)
        {
            MakeRotation(vectorMovement);
        }
        if (vectorAim != Vector2.zero && canShoot)
        {
            //LevelManager.instance.GainLevel();
            Shoot();
            
        }
        
    }

    public void CanTakeDamageTrue()
    {
        canTakeDamage = true;
    }

    public void CanTakeDamageFalse()
    {
        canTakeDamage = false;
    }

    private void ChangeDimension()
    {
        
        if(actualDimension == layerDimensionA)
        {
            Camera.main.cullingMask -= layerDimensionA;
            Camera.main.cullingMask += layerDimensionB;
            actualDimension = layerDimensionB;
            Physics2D.IgnoreLayerCollision(12, 8,true);
            Physics2D.IgnoreLayerCollision(12, 11,false);
        }
        else
        {
            Camera.main.cullingMask += layerDimensionA;
            Camera.main.cullingMask -= layerDimensionB;
            actualDimension = layerDimensionA;
            Physics2D.IgnoreLayerCollision(12, 8,false);
            Physics2D.IgnoreLayerCollision(12, 11, true);
        }
        EnemyManager.instance.ChangeDimensionEnemy();
    }
    private void MakeRotation(Vector2 vector)
    {
        float angle = Mathf.Atan2(-vector.x, vector.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }

    private void Shoot()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, spawnBullet.transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(colliderShip, bullet.GetComponent<Collider2D>());
        bullet.GetComponent<BulletScript>().InitializeBullet(vectorAim, CheckShootState(freezeProbabily), CheckShootState(fireProbabily));
        StartCoroutine(ShootTimer());
        
    }

    public void Laser()
    {
        GameObject laser = Instantiate(laserPrefab, spawnLaser.transform);
        StartCoroutine(LaserTimer());
    }


    private IEnumerator ShootTimer()
    {
        canShoot = false;
        canLaser = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        canLaser = true;
    }

    private IEnumerator LaserTimer()
    {
        canShoot = false;
        canLaser = false;
        yield return new WaitForSeconds(laserTime);
        Destroy(spawnLaser.transform.GetChild(0).gameObject);
        canShoot = true;
        yield return new WaitForSeconds(laserRate);
        canLaser = true;
    }


    private void FixedUpdate()
    {
        transform.parent.Translate(vectorMovement * speed * Time.deltaTime);
        
        
    }

    private void Dash()
    {
        transform.parent.GetComponent<Rigidbody2D>().AddForce(vectorMovement * dashForce);
        boostDashObject.SetActive(true);
        boostObject.SetActive(false);
        StartCoroutine(TimerDash(timeAfterNextDash));
        StartCoroutine(StopDash());
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(0.1f);
        transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        boostDashObject.SetActive(false);
        boostObject.SetActive(true);
    }

    private IEnumerator TimerDash(float timeAfterNextDash)
    {
        canDash = false;
        yield return new WaitForSeconds(timeAfterNextDash);
        canDash = true;
    }

    private bool CheckShootState(float probabilty)
    {
        float randomProbabily = Random.Range(0f, 1f);
        if(randomProbabily<probabilty)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnEnable()
    {
        controller.Player.Enable();
    }

    private void OnDisable()
    {
        controller.Player.Disable();
    }

    public void TakeDamage(int damage)
    {
        shipAnim.SetTrigger("TakeDamage");
        hp -= damage;
    }
}

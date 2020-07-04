
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Controller controller;
    Vector2 vectorMovement,vectorAim;
    bool canShoot = true, canLaser = true;
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
    public int hp;
    public static PlayerScript instance;
    bool inEchap = false;
    [SerializeField]
    GameObject pauseText;
    

    private void Awake()
    {
        Time.timeScale = 1;
        Time.timeScale = 1;
        if(instance == null)
        {
            instance = this;
        }
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
        controller.Player.Laser.performed += ctx =>
        {
            if (canLaser)
            {
                Laser();
            }

        };
        controller.Player.SwitchDimension.performed += ctx => ChangeDimension();
        controller.Player.Pause.performed += ctx => PauseGame();
        
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

    private void Start()
    {
        SliderManager.instance.InitializeSliderPlayer();
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
            Shoot();
            
        }
        
    }

   private IEnumerator TakeDamageTimer()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(1);
        canTakeDamage = true;
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
        canShoot = false;
        StopCoroutine(ShootTimer());
        GameObject bullet = Instantiate(bulletPrefab, spawnBullet.transform.position, Quaternion.identity);
        SoundManager.instance.PlayBulletPlayerSound();
        Physics2D.IgnoreCollision(colliderShip, bullet.GetComponent<Collider2D>());
        bullet.GetComponent<BulletScript>().InitializeBullet(vectorAim, CheckShootState(freezeProbabily), CheckShootState(fireProbabily));
        StartCoroutine(ShootTimer());
        
    }

    public void Laser()
    {
        GameObject laser = Instantiate(laserPrefab, spawnLaser.transform);
        canShoot = false;
        StopAllCoroutines();
        StartCoroutine(LaserTimer());
    }


    private IEnumerator ShootTimer()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private IEnumerator LaserTimer()
    {
        canLaser = false;
        yield return new WaitForSeconds(laserTime);
        Destroy(spawnLaser.transform.GetChild(0).gameObject);
        canShoot = true;
        canDash = true;
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
        if(canTakeDamage)
        {
            SliderManager.instance.TakeDamage(damage);
            shipAnim.SetTrigger("TakeDamage");
            hp -= damage;
            
            if (hp <= 0)
            {
                Died();
            }
            StartCoroutine(TakeDamageTimer());
        }
        
    }

    public void Died()
    {
        
        LevelManager.instance.ShowRetry();
        Destroy(gameObject);
    }
    public void PauseGame()
    {
        if(!inEchap)
        {
            Time.timeScale = 0;
            inEchap = true;
            pauseText.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            inEchap = false;
            pauseText.SetActive(false);
        }
    }

}

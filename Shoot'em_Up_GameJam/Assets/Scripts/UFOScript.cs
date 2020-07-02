using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOScript : BaseEnemy
{
    [SerializeField] private float tpDistToPlayer, tpRate, maxDist;
    private Animator[] anims;

    private bool readyTp = true, wantTp, mustTp;

    protected override void Start()
    {
        base.Start();

        MetasSpriteSetup();
    }



    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(PlayerScript.instance.transform.position, transform.position) >= maxDist)
        {
            wantTp = true;
        }
        else
        {
            wantTp = false;
        }

        if (readyTp && (wantTp || mustTp) && !freeze)
        {
            StartCoroutine(TimerTeleport());
            Teleport();
        }
    }

    protected override void RotateToPlayer() => transform.up = (Vector2) (PlayerScript.instance.transform.position - transform.position);



    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("PlayerBullet"))
        {
            mustTp = true;
        }
    }

    private void Teleport()
    {
        Vector2 tpVector = new Vector2(Random.Range(-1f, 1f),Random.Range(-1f, 1f)).normalized * tpDistToPlayer;
        rb.position = (Vector2)PlayerScript.instance.transform.position + tpVector;
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
        anims = GetComponentsInChildren<Animator>();

        //Debug.Log(animators.Length);

        foreach (Animator a in anims)
        {
            if (a.gameObject.name == "Left")
                a.SetBool("Left", true);
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

}

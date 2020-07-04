using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject explosionParticules,explosionSprites,tpSprites;
    [SerializeField]
    bool explosionWithParticules;

    public static ParticuleManagerScript instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateExplosion(Vector2 position)
    {
        if(explosionWithParticules)
        {
            GameObject explo = Instantiate(explosionParticules, position, Quaternion.identity);
            Destroy(explo, 1f);
        }
        else
        {
            GameObject explo = Instantiate(explosionSprites, position, Quaternion.identity);
            Destroy(explo, 0.3f);
        }
        
    }

    public void CreateTpParticules(Vector2 position)
    {
        GameObject tp = Instantiate(tpSprites, position, Quaternion.identity);
        Destroy(tp, 0.3f);
    }
}

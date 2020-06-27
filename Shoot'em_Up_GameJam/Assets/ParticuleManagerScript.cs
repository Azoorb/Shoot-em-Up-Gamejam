using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject prefabExplosion;

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
        GameObject explo = Instantiate(prefabExplosion, position, Quaternion.identity);
        Destroy(explo, 0.3f);
    }
}

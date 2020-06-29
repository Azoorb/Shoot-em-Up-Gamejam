using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private GameObject laserBody;
    [SerializeField] private int bodyLength;
    private int remainingLength;
    public bool instantiateBody = false;

    void Start()
    {
        transform.right = transform.up;
    }

    void Update()
    {
        if (instantiateBody == true && bodyLength > 0)
        {
            GameObject b = Instantiate(laserBody, transform);
            b.transform.right = transform.right;
            b.transform.position = transform.right * bodyLength;

            bodyLength--;
        }
    }
}

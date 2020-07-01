using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private GameObject laserBody,laserEnd;
    [SerializeField] private int bodyLength;
    public bool instantiateBody = false;
    GameObject previousBody;

    void Start()
    {
        transform.right = transform.up;
    }

    void Update()
    {
        
    }

    public void LaunchBody()
    {
        for(int i=0;i<bodyLength;i++)
        {
            if(i==0)
            {
                GameObject b = Instantiate(laserBody, transform.GetChild(0).position, Quaternion.identity, transform.GetChild(0));
                b.transform.right = transform.right;
                previousBody = b;
            }
            else if(i == bodyLength-1)
            {
                GameObject b = Instantiate(laserEnd, previousBody.transform.GetChild(0).position, Quaternion.identity, transform.GetChild(0));
                b.transform.right = -transform.right;
            }
            else
            {
                GameObject b = Instantiate(laserBody, previousBody.transform.GetChild(0).position, Quaternion.identity, transform.GetChild(0));
                b.transform.right = transform.right;
                previousBody = b;
            }
        }
    }
}

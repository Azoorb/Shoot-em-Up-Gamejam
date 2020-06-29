using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public bool instatiateBody = false;

    [SerializeField] private GameObject laserBody;
    [SerializeField] private int laserLength;

    private void Start()
    {
        transform.right = transform.up;
    }

    void Update()
    {
        if (instatiateBody == true)
        {
            for (int i = 1; i < laserLength; i++)
            {
                GameObject b = Instantiate(laserBody, transform);
                b.transform.localPosition = transform.up * -i;
                b.transform.right = transform.right;
            }

            instatiateBody = false;

        }
    }
}

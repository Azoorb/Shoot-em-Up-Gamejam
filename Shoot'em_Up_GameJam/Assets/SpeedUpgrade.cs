using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : MonoBehaviour, IUpgrade
{
    [SerializeField]
    float speedUp;
    [SerializeField]
    GameObject card;
    public void UpgradePlayer()
    {
        GameObject.Find("ShipRenderer").GetComponent<PlayerScript>().speed += speedUp;
    }
}

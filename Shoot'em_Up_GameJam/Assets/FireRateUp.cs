using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateUp : UpgradeBase, IUpgrade
{
    [SerializeField]
    float fireRateBonus;
    public void UpgradePlayer()
    {
        GameObject.Find("ShipRenderer").GetComponent<PlayerScript>().fireRate -= fireRateBonus;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeUpgrade : UpgradeBase, IUpgrade
{
    [SerializeField]
    float freezeProbabilyBonus;
    public void UpgradePlayer()
    {
        GameObject.Find("ShipRenderer").GetComponent<PlayerScript>().freezeProbabily += freezeProbabilyBonus;
    }
}

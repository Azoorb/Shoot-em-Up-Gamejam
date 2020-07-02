using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgrade : UpgradeBase,IUpgrade
{
    [SerializeField]
    private int damageUp;

    public void UpgradePlayer()
    {
        PlayerScript.instance.damageBonus += damageUp;
    }
}

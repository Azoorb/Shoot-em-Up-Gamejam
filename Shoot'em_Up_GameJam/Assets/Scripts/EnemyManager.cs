using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public LayerMask dimensionALayout, dimensionBLayout;

    public List<GameObject> enemyList;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        Debug.Log(dimensionALayout.value);
    }

    public void CheckLight(GameObject enemy)
    {
        
        if (enemy.GetComponent<BaseEnemy>().actualDimension != PlayerScript.instance.actualDimension)
        {
            enemy.GetComponent<BaseEnemy>().redLight.SetActive(true);
        }
        else
        {
            enemy.GetComponent<BaseEnemy>().redLight.SetActive(false);
        }
    }


    public void ChangeDimensionEnemy()
    {
        foreach (GameObject enemy in enemyList)
        {
            CheckLight(enemy);
        }
    }
}

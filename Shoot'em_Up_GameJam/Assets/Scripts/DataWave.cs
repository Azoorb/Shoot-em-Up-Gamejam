using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataWave")]
public class DataWave : ScriptableObject
{
    public List<GameObject> enemyInWave;
    public List<int> numberEnemyInWave;
    public List<int> hpBonusForEnemy;
    public float minIntervalSpawn;
    public float maxIntervalSpawn;
}

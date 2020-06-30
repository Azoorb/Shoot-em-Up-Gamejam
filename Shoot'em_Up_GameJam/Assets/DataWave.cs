using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataWave")]
public class DataWave : ScriptableObject
{
    public List<GameObject> enemyInWave;
    public List<int> numberEnemyInWave;
    public float minIntervalSpawn;
    public float maxIntervalSpawn;
}

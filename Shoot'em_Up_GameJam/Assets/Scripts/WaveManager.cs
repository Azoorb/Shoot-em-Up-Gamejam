using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    List<DataWave> dataWaveList;

    private int actualRound;
    private float  actualMinInterval;
    private float actualMaxInterval;
    private List<int> actualNumberEnemyLeft;
    private List<GameObject> actualEnemyLeft;
    private List<int> actualHpBonusEnemy;

    private void Awake()
    {
        actualNumberEnemyLeft = new List<int>();
        actualHpBonusEnemy = new List<int>();
        actualEnemyLeft = new List<GameObject>();
    }


    public void StartRound(int round)
    {
        actualNumberEnemyLeft.Clear();
        actualNumberEnemyLeft.Clear();
        actualHpBonusEnemy.Clear();
        actualNumberEnemyLeft.AddRange(dataWaveList[round].numberEnemyInWave);
        actualEnemyLeft.AddRange(dataWaveList[round].enemyInWave);
        actualHpBonusEnemy.AddRange(dataWaveList[round].hpBonusForEnemy);
        actualMinInterval = dataWaveList[round].minIntervalSpawn;
        actualMaxInterval = dataWaveList[round].maxIntervalSpawn;
        StartCoroutine(SpawnTimer());
    }

    private void Start()
    {
        if(dataWaveList.Count!=0)
        {
            StartRound(0);
        }
        
    }

    public IEnumerator SpawnTimer()
    {
        if(actualNumberEnemyLeft.Count != 0 && actualEnemyLeft.Count != 0)
        {
            int randomEnemy = Random.Range(0, actualEnemyLeft.Count);
            //Faut modifier le spawn ici 
            int randomSpawnPoint = Random.Range(0, transform.childCount);
            GameObject enemy = Instantiate(actualEnemyLeft[randomEnemy], transform.GetChild(randomSpawnPoint).transform.position, Quaternion.identity);
            enemy.GetComponent<IEnemy>().AddHp(actualHpBonusEnemy[randomEnemy]);
            actualNumberEnemyLeft[randomEnemy]--;
            if(actualNumberEnemyLeft[randomEnemy] == 0)
            {
                actualNumberEnemyLeft.RemoveAt(randomEnemy);
                actualEnemyLeft.RemoveAt(randomEnemy);
            }
            float randomInterval = Random.Range(actualMinInterval, actualMaxInterval);
            yield return new WaitForSeconds(randomInterval);
            StartCoroutine(SpawnTimer());
        }
        else
        {
            actualRound++;
            if (actualRound < dataWaveList.Count)
            {
                StartRound(actualRound);
            }
        }
        
    }
}

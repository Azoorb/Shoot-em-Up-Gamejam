using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab = null;

    [SerializeField] private float spawnRate = 0;
    private float _rate = 0.5f;

    void Update()
    {
        if (_rate <= 0f)
        {
            int spawnSide = Random.Range(0, 4); //Choix de là ou l'ennemi arrive (NSEO)
            Debug.Log(spawnSide);

            Vector2 spawnPos = new Vector2(0f, 0f);

            switch (spawnSide)
            {
                case 0:
                    //Nord
                    spawnPos = new Vector2(Random.Range(-8f, 8f), -4.5f);
                    break;

                case 1:
                    //Sud
                    spawnPos = new Vector2(Random.Range(-8f, 8f), 4.5f);
                    break;

                case 2:
                    //Est
                    spawnPos = new Vector2(8f, Random.Range(-4.5f, 4.5f));
                    break;

                case 3:
                    //Ouest
                    spawnPos = new Vector2(-8f, Random.Range(-4.5f, 4.5f));
                    break;
            }

            GameObject e = Instantiate(enemyPrefab, (Vector2)transform.position + spawnPos, Quaternion.identity);

            _rate = spawnRate;
        }

        _rate -= Time.deltaTime;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _newEnemy;
    [SerializeField]
    private GameObject[] _powerups;
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine("SpawnEnemies");
        StartCoroutine("SpawnPowerup");
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        while(_stopSpawning == false)
        {
            int enemyType = Random.Range(0, 10);
            
            Vector3 spawnPos = new Vector3(Random.Range(-7f, 7f), 8, 0);
            if(enemyType < 7)
            { 
                GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else
            { 
                GameObject newEnemy = Instantiate(_newEnemy, spawnPos, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(5f);
        while(_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 8);

            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 8, 0);
            Instantiate(_powerups[randomPowerup], spawnPos, Quaternion.identity);
            
            yield return new WaitForSeconds(Random.Range(5f,8f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

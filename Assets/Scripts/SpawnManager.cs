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

    private int _baseEnemies = 4;
    [SerializeField]
    private int _waveLevel = 1;
    [SerializeField]
    private int _enemiesToSpawn;
    [SerializeField]
    private int _remainingEnemies;
    [SerializeField]
    private int _enemiesSpawned = 0;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _boss;

    private bool _isBossActive = true;

    private void Start()
    {
        _enemiesToSpawn = _baseEnemies + _waveLevel;
        _remainingEnemies = _enemiesToSpawn;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is Null in Spawn Manger");
        }
    }
    private void Update()
    {
          
    }

    public void StartSpawning()
    {
        StartCoroutine("WaveStarting");
    }

    public void EnemyDestroyed()
    {
        if(_remainingEnemies > 1 && !_isBossActive)
        {
            _remainingEnemies--;
        }
        else if (!_isBossActive)
        {
            _remainingEnemies--;
            StopCoroutine("SpawnEnemies");
            StopCoroutine("SpawnPowerup");
            if (_waveLevel <= 1)
            {
                _waveLevel++;
                StartCoroutine("WaveStarting");
            }
            else
            {
                StartCoroutine("SpawnPowerup");
                StartCoroutine("BossTime");
                _isBossActive = true;
            }
            
        }
        else
        { _remainingEnemies--; }
    }

    IEnumerator BossTime()
    {
        yield return new WaitForSeconds(1f);
        GameObject boss = Instantiate(_boss, new Vector3(0, 8f, 0), Quaternion.identity);
        boss.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    IEnumerator WaveStarting()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Wave " + _waveLevel + " starting!");
        _enemiesToSpawn = _baseEnemies + _waveLevel;
        _remainingEnemies = _enemiesToSpawn;
        _enemiesSpawned = 0;
        _uiManager.UpdateWave(_waveLevel);
        StartCoroutine("SpawnEnemies");
        StartCoroutine("SpawnPowerup");
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3f);
        while(_stopSpawning == false && _enemiesSpawned < _enemiesToSpawn)
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
            _enemiesSpawned++;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(5f);
        while (_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 100);

            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 8, 0);

            if (randomPowerup < 30)
            {
                Instantiate(_powerups[0], spawnPos, Quaternion.identity);
            }
            else if (randomPowerup >= 30 && randomPowerup < 50)
            {
                Instantiate(_powerups[1], spawnPos, Quaternion.identity);
            }
            else if (randomPowerup >= 50 && randomPowerup < 70)
            {
                Instantiate(_powerups[2], spawnPos, Quaternion.identity);
            }
            else if (randomPowerup >= 70 && randomPowerup < 75)
            {
                Instantiate(_powerups[3], spawnPos, Quaternion.identity);
            }
            else if (randomPowerup >= 75 && randomPowerup < 80)
            {
                Instantiate(_powerups[4], spawnPos, Quaternion.identity);
            }
            else if (randomPowerup >= 80 && randomPowerup < 85)
            {
                Instantiate(_powerups[5], spawnPos, Quaternion.identity);
            }
            else if (randomPowerup >= 85 && randomPowerup < 90)
            {
                Instantiate(_powerups[6], spawnPos, Quaternion.identity);
            }
            else if (randomPowerup >= 90 && randomPowerup < 95)
            {
                Instantiate(_powerups[7], spawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(_powerups[8], spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(5f,8f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBackup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (_player == null)
        {
            currentState = BossState.Won;
        }
        if (_canSpawn)
        {
            //_canSpawn = false;
            //Spawn();
            //StartCoroutine(EnemySpawner(false));

        }
        switch (currentState)
        {
            case BossState.Arriving:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 3, 0),
                1f * Time.deltaTime);
                if (transform.position.y == 3f)
                {
                    StartCoroutine("Arrived");
                }
                break;

            case BossState.SpawnPos:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 6, 0),
                1f * Time.deltaTime);
                if (transform.position.y == 6f)
                {
                    currentState = BossState.Spawning;
                }
                break;

            case BossState.Spawning:
                currentState = BossState.AttackPos;
                _canSpawn = true;
                Spawn();

                _canSpawn = false;
                break;

            case BossState.AttackPos:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-6, 5, 0),
                1f * Time.deltaTime);
                if (transform.position.x == -6f)
                {
                    currentState = BossState.Attack;
                }
                break;

            case BossState.Attack:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(6, 5, 0),
                1f * Time.deltaTime);
                EnemyFire();
                if (transform.position.x == 6f)
                {
                    currentState = BossState.Recharging;
                }
                break;

            case BossState.Recharging:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(3, 5, 0),
                1f * Time.deltaTime);
                if (transform.position.x == 3)
                {
                    StartCoroutine("Charging");
                    currentState = BossState.Vulnerable;
                }
                break;
            case BossState.Vulnerable:
                float spasmX = Random.Range(2.5f, 3.5f);
                float spasmY = Random.Range(4.5f, 5.5f);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(spasmX, spasmY, 0),
                1f * Time.deltaTime);
                break;
            case BossState.Won:
                Debug.Log("Player dead");
                break;
        }*/
    }
    /*IEnumerator Arrived()
    {
        yield return new WaitForSeconds(3f);
        currentState = BossState.SpawnPos;
    }

    IEnumerator EnemySpawner(bool cS)
    {
        if (_canSpawn)
        {
            _canSpawn = cS;
            Vector3 spawnL = new Vector3(transform.position.x + 2f, transform.position.y - 0.25f, 0);
            GameObject newEnemy = Instantiate(_enemy, spawnL, Quaternion.identity);
            newEnemy.transform.rotation = Quaternion.Euler(0, 0, 90);
            Vector3 spawnR = new Vector3(transform.position.x - 2f, transform.position.y - 0.25f, 0);
            newEnemy = Instantiate(_enemy, spawnR, Quaternion.identity);
            newEnemy.transform.rotation = Quaternion.Euler(0, 0, -90);
            StopSpawn();
        }
        yield return null;
    }
    void StopSpawn()
    {
        StopCoroutine(EnemySpawner(false));
    }
    void Spawn()
    {
        if (_canSpawn)
        {
            _canSpawn = false;
            Vector3 spawnL = new Vector3(transform.position.x + 2f, transform.position.y - 0.25f, 0);
            GameObject newEnemy = Instantiate(_enemy, spawnL, Quaternion.identity);
            newEnemy.transform.rotation = Quaternion.Euler(0, 0, 90);
            Vector3 spawnR = new Vector3(transform.position.x - 2f, transform.position.y - 0.25f, 0);
            newEnemy = Instantiate(_enemy, spawnR, Quaternion.identity);
            newEnemy.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }

    void EnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = 0.75f;
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser;

            enemyLaser = Instantiate(_BossLaser, transform.position, Quaternion.identity);


            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            foreach (Laser l in lasers)
            {
                l.AssignEnemyLaser();
            }
            //_audioSource.clip = _enemyLaserSound;
            //_audioSource.Play();
        }
    }

    IEnumerator Charging()
    {
        yield return new WaitForSeconds(5);
        currentState = BossState.SpawnPos;

    }*/
}

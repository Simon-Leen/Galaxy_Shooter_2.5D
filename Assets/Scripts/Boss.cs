using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private int _bossHealth = 10;

    private Player _player;

    [SerializeField]
    private GameObject _BossLaser;
    private float _fireRate = 2f;
    private float _canFire;

    private bool _canSpawn = false;

    [SerializeField]
    private GameObject _enemy;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _bossLaserSound;
    [SerializeField]
    private AudioClip _bossChargeLaserSound;
    [SerializeField]
    private AudioClip _bossVulnerableSound;
    [SerializeField]
    private AudioClip _explosionSound;

    [SerializeField]
    private GameObject[] _bossDamages;

    private Animator _anim;

    private GameObject[] _enemies;

    public enum BossState
    {
        Arriving,
        SpawnPos,
        Spawning,
        AttackPos,
        Attack,
        Recharging,
        Vulnerable,
        Won
    }

    [SerializeField]
    private BossState currentState;

    void Start()
    {
        currentState = BossState.Arriving;

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null in boss");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Boss is Null");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is Null on Boss");
        }

        

        _canFire = Time.time + 1f;
    }

    void Update()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (_player == null)
        {
            currentState = BossState.Won;
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
                StartCoroutine(EnemySpawner());
                break;

            case BossState.AttackPos:
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(-6, 5, 0),
                1f * Time.deltaTime);
                if (transform.position.x == -6f)
                {
                    StartCoroutine(AttackReady());
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
        }
    }
    IEnumerator Arrived()
    {
        yield return new WaitForSeconds(2);
        currentState = BossState.SpawnPos;
    }

    IEnumerator EnemySpawner()
    {
        if (_canSpawn)
        {
            _canSpawn = false;
            Spawn();
            yield return new WaitForSeconds(2);
            Spawn();
        }
        yield return null;
    }
    void Spawn()
    {
        Vector3 spawnL = new Vector3(transform.position.x + 2f, transform.position.y - 0.25f, 0);
        GameObject newEnemy = Instantiate(_enemy, spawnL, Quaternion.identity);
        newEnemy.transform.rotation = Quaternion.Euler(0, 0, 90);
        Vector3 spawnR = new Vector3(transform.position.x - 2f, transform.position.y - 0.25f, 0);
        newEnemy = Instantiate(_enemy, spawnR, Quaternion.identity);
        newEnemy.transform.rotation = Quaternion.Euler(0, 0, -90);
    }
    IEnumerator AttackReady()
    {
        AudioSource.PlayClipAtPoint(_bossChargeLaserSound, transform.position);
        yield return new WaitForSeconds(2);
        currentState = BossState.Attack;

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
            _audioSource.clip = _bossLaserSound;
            _audioSource.Play();
        }
    }

    IEnumerator Charging()
    {
        _audioSource.clip = _bossVulnerableSound;
        _audioSource.Play();
        yield return new WaitForSeconds(2.5f);
        currentState = BossState.SpawnPos;

    }
    private void BossDamage(int health)
    {
        int damage = health - 1;
        switch(health)
        {
            case 9:
                _bossDamages[damage].SetActive(true);
                break;
            case 8:
                _bossDamages[damage].SetActive(true);
                break;
            case 7:
                _bossDamages[damage].SetActive(true);
                break;
            case 6:
                _bossDamages[damage].SetActive(true);
                break;
            case 5:
                _bossDamages[damage].SetActive(true);
                break;
            case 4:
                _bossDamages[damage].SetActive(true);
                break;
            case 3:
                _bossDamages[damage].SetActive(true);
                break;
            case 2:
                _bossDamages[damage].SetActive(true);
                break;
            case 1:
                _bossDamages[damage].SetActive(true);
                break;
            case 0:
                foreach(GameObject d in _bossDamages)
                {
                    d.SetActive(false);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            if(_bossHealth > 1)
            {
                _bossHealth--;
                BossDamage(_bossHealth);
            }
            else
            {
                _bossHealth--;
                BossDamage(_bossHealth);

                currentState = BossState.Won;
                StopAllCoroutines();
                StartCoroutine(EndWaves());
            }
            
        }
    }
    IEnumerator EndWaves()
    {
        DestroyEnemies();
        yield return new WaitForSeconds(2);
        _anim.SetTrigger("OnEnemyDeath");
        _audioSource.clip = _explosionSound;
        _audioSource.Play();
        _canFire = 3600f;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.5f);
    }
    private void DestroyEnemies()
    {
        foreach (GameObject enemy in _enemies)
        {
            Animator a = enemy.GetComponent<Animator>();
            a.SetTrigger("OnEnemyDeath");
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
            _canFire = 3600f;
            Destroy(enemy.GetComponent<Collider2D>());
            Destroy(enemy.gameObject, 2f);
        }
    }

}

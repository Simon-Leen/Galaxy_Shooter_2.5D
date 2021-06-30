using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _redEnemyLaser;
    private float _fireRate = 3f;
    private float _canFire = -1;

    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _enemyLaserSound;

    private float _sideways = 0;

    [SerializeField]
    private bool _isRedEnemy = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private bool _activeShield = true;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is Null");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is Null");
        }
        if(_isRedEnemy)
        {
            _speed = 3f;
        }
        else
        {
            _shieldVisualizer.SetActive(true);
            _speed = 4f;
        }
        StartCoroutine("Sideways");

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        CalcMovement();
        {
            if (_isRedEnemy)
            {
                RedEnemyFire();
            }
            else
            {
                EnemyFire();
            }
        }
        
    }
    void EnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            foreach (Laser l in lasers)
            {
                l.AssignEnemyLaser();
            }
            _audioSource.clip = _enemyLaserSound;
            _audioSource.Play();
        }
    }
    void RedEnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_redEnemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            foreach (Laser l in lasers)
            {
                l.AssignEnemyLaser();
            }
            _audioSource.clip = _enemyLaserSound;
            _audioSource.Play();
        }
    }


    void CalcMovement()
    {
        transform.Translate( (Vector3.down + new Vector3(_sideways, 0, 0)) * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            transform.position = new Vector3(transform.position.x, 7f, 0);
        }
        if (transform.position.x > 8f)
        {
            _sideways = -_sideways;
        }
        else if (transform.position.x < -8f)
        {
            _sideways = -_sideways;
        }
    }
    IEnumerator Sideways()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1,3));
            _sideways = Random.Range(-0.25f, 0.25f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (!_isRedEnemy && _activeShield == true)
            {
                _activeShield = false;
                _shieldVisualizer.SetActive(false);
                _player.TakeDamage();
                return;
            }
            if (transform.childCount > 0)
            {
                foreach (Transform c in transform)
                {
                    Destroy(c.gameObject);
                }
            }
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.TakeDamage();
                Destroy(GetComponent<Collider2D>());
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
            _canFire = 3600f;
            _spawnManager.EnemyDestroyed();
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Laser")
        {
            if(!_isRedEnemy && _activeShield == true)
            {
                _activeShield = false;
                _shieldVisualizer.SetActive(false);
                Destroy(other.gameObject);
                return;
            }
            if (transform.childCount > 0)
            {
                foreach(Transform c in transform)
                {
                    Destroy(c.gameObject);
                }
            }
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(Random.Range(5,11));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
            _canFire = 3600f;
            Destroy(GetComponent<Collider2D>());
            _spawnManager.EnemyDestroyed();
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "PlayerEMP")
        {
            if (_player != null)
            {
                _player.AddScore(Random.Range(5, 11));
            }
            if (transform.childCount > 0)
            {
                foreach (Transform c in transform)
                {
                    Destroy(c.gameObject);
                }
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
            _canFire = 3600f;
            Destroy(GetComponent<Collider2D>());
            _spawnManager.EnemyDestroyed();
            Destroy(this.gameObject, 2.5f);
        }
    }
}

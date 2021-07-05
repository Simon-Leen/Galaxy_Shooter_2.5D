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
    [SerializeField]
    private GameObject _laserBehind;
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

    private bool _isPlayerBehind = false;
    private bool _chasingPlayer = false;

    private bool _onComingLaser = false;
    private int _onComingSide = 0;

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
        if (Time.time > _canFire && !_chasingPlayer)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser;

            if(!_isPlayerBehind)
            {
                enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                enemyLaser = Instantiate(_laserBehind, transform.position + new Vector3(0,2.3f,0), Quaternion.Euler(0, 0, 180));
            }

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
        if(_onComingLaser)
        {
            LaserDodge();
        }
        transform.Translate( (Vector3.down + new Vector3(_sideways, 0, 0)) * _speed * Time.deltaTime);

        if( _player != null && !_isRedEnemy)
        {
            float distanceToo = Vector3.Distance(transform.position, _player.transform.position);
            float playerY = _player.transform.position.y;
            float enemyY = transform.position.y;
            Vector3 enemyPos = transform.position;
            Vector3 playerPos = _player.transform.position;

            Quaternion targetRot = Quaternion.LookRotation(transform.forward, (enemyPos - playerPos));

            if(distanceToo > 3f)
            {
                _isPlayerBehind = false;
                _chasingPlayer = false;
            }

            if (distanceToo < 3f && enemyY >= playerY)
            {
                _chasingPlayer = true;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, (_speed * 1.5f) * Time.deltaTime);
                _isPlayerBehind = false;
            }
            else if(distanceToo < 3f && enemyY < playerY)
            {
                _isPlayerBehind = true;
            }
            else
            {
                _chasingPlayer = false;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity,_speed * Time.deltaTime);
            }
        }
        
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

    void LaserDodge()
    {
        if ( _onComingSide < 0)
        {
            transform.Translate(Vector3.right * (_speed*2) * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * (_speed * 2) * Time.deltaTime);
        }
    }

    public void LaserSide(bool oncoming, int side)
    {
        if (oncoming)
        {
            _onComingLaser = true;
            _onComingSide = side;
        }
        else
        {
            _onComingLaser = false;
            _onComingSide = side;
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

        if (other.tag == "Missile")
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftWingDamage;
    [SerializeField]
    private GameObject _rightWingDamage;
    private float _fireRate = 0.3f;
    private float _nextFire = 0f;
    private int _lives = 3;
    [SerializeField]
    private int _score;
    private SpawnManager spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _emptyAmmo;
    [SerializeField]
    private AudioClip _playerHit;
    private float _thrusterSpeed = 1.5f;
    [SerializeField]
    private GameObject _thrusterPrefab;

    private int _shieldHealth;
    private int _playerAmmo;

    [SerializeField]
    private PostProcessVolume _ppv;
    private ColorGrading _cgl;
    private Bloom _bloom;

    [SerializeField]
    private GameObject _chaosGuns;
    private bool _isChaosActive;

    [SerializeField]
    private GameObject _playerEMP;
    [SerializeField]
    private bool _isEMPActive = false;

    private CameraShake _camShake;

    private float _thrustersLevel = 3f;
    private float _canThrust = 0f;
    private float _thrusterCharge = 3f;
    private bool _isThrustersActive = false;
    private bool _isThrustersCharging = false;

    [SerializeField]
    private GameObject _magnetField;

    private GameObject[] _powerups;

    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private bool _isMissileActive = false;
    private GameObject[] _enemies;
    [SerializeField]
    private AudioClip _noEnemiesSound;

    void Start()
    {
        transform.position = new Vector3(0, -2, 0);
        

        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        
        _camShake = Camera.main.GetComponent<CameraShake>();

        

        if (_camShake == null)
        {
            Debug.LogError("Camera Shake is null");
        }

        _ppv.profile.TryGetSettings(out _cgl);
        _ppv.profile.TryGetSettings(out _bloom);


        if (spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is Null");
        }
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is Null");
        }
        _playerAmmo = 15;
        _uiManager.UpdateAmmo(_playerAmmo);
    }

    void Update()
    {
        _powerups = GameObject.FindGameObjectsWithTag("Powerup");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        CalculateMovement();


        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            if(_playerAmmo > 0)
            {
                ShootLaser();
            }
            else
            {
                _audioSource.clip = _emptyAmmo;
                _audioSource.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canThrust < Time.time)
        {
            if(_thrustersLevel > 0)
            {
                _isThrustersActive = true;
                ThrustersActivate();
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isThrustersActive = false;
            ThrustersDeactivate();
        }

        ThrustersUsage();

        if(Input.GetKeyDown(KeyCode.E) )
        {
            if (_isEMPActive == true)
            {
                FireEMP();
            }
            else
            {
                _audioSource.clip = _emptyAmmo;
                _audioSource.Play();
            }
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            _magnetField.SetActive(true);
            foreach(GameObject powerUp in _powerups)
            {
                Powerup p = powerUp.GetComponent<Powerup>();
                p.Magnetized();
            }
            
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            _magnetField.SetActive(false);
            foreach (GameObject powerUp in _powerups)
            {
                Powerup p = powerUp.GetComponent<Powerup>();
                p.DeMagnetize();
            }
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            if (_isMissileActive == true)
            {
                if(_enemies.Length < 1)
                {
                    _audioSource.clip = _noEnemiesSound;
                    _audioSource.Play();
                }
                else
                {
                    FireMissile();
                }
            }
            else
            {
                _audioSource.clip = _emptyAmmo;
                _audioSource.Play();
            }
        }
    }
    
    void ThrustersActivate()
    {
        _speed *= _thrusterSpeed;
        Vector3 scaler = new Vector3(1.15f, 1.15f, 1f);
        _thrusterPrefab.transform.localScale = scaler;
        Vector3 thrusterPos = new Vector3(transform.position.x, (transform.position.y - 1.3f), transform.position.z);
        _thrusterPrefab.transform.position = thrusterPos;
    }
    void ThrustersDeactivate()
    {
        _speed = 3.5f;
        Vector3 scaler = new Vector3(1f, 1f, 1f);
        _thrusterPrefab.transform.localScale = scaler;
        Vector3 thrusterPos = new Vector3(transform.position.x, (transform.position.y - 1.2f), transform.position.z);
        _thrusterPrefab.transform.position = thrusterPos;
    }

    void ThrustersUsage()
    {
        if(_isThrustersActive)
        {   
            if(_thrustersLevel > 0)
            { 
                _thrustersLevel -= Time.deltaTime;
                _uiManager.UpdateThrusters(_thrustersLevel);
            }
            else
            {
                _thrustersLevel = 0;
                ThrustersDeactivate();
                _canThrust = _thrusterCharge + Time.time;
                _isThrustersCharging = true;
                _uiManager.ThrustersCharging(_isThrustersCharging);
                StartCoroutine("ThrusterCharged");
                _uiManager.UpdateThrusters(_thrustersLevel);
                _speed = 3.5f;
            }
            
        }
        else
        {
            if(_isThrustersCharging)
            {
                if (_thrustersLevel < 3)
                {
                    _thrustersLevel += Time.deltaTime;
                    _uiManager.UpdateThrusters(_thrustersLevel);
                }
                else
                {
                    _thrustersLevel = 3;
                    _uiManager.UpdateThrusters(_thrustersLevel);
                }
            }
            else
            {
                if (_thrustersLevel < 3)
                {
                    _thrustersLevel += Time.deltaTime / 3;
                    _uiManager.UpdateThrusters(_thrustersLevel);
                }
                else
                {
                    _thrustersLevel = 3;
                    _uiManager.UpdateThrusters(_thrustersLevel);
                }
            }
        }
    }

    IEnumerator ThrusterCharged()
    {
        yield return new WaitForSeconds(3f);
        _uiManager.ThrustersCharging(false);
        _isThrustersCharging = false;
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * Time.deltaTime * _speed);
    
        if (transform.position.y >= 0.75f)
        {
            transform.position = new Vector3(transform.position.x, 0.75f, 0);
        }
        else if (transform.position.y <= -2.5f)
        {
            transform.position = new Vector3(transform.position.x, -2.5f, 0);
        }

        if (transform.position.x > 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, 0);
        }
        else if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, 0);
        }
    }

    void ShootLaser()
    {
        
        _nextFire = Time.time + _fireRate;
        
        if(_playerAmmo < 3)
        {
            _isTripleShotActive = false;
        }
        if (_isTripleShotActive)
        {
            _playerAmmo = _playerAmmo-3;
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
        else if(_isChaosActive)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(-0.92f, -0.6f, 0), Quaternion.Euler(0f, 0f, 123f));
            Instantiate(_laserPrefab, transform.position + new Vector3(-1.1f, 0.13f, 0), Quaternion.Euler(0f, 0f, 85f));
            Instantiate(_laserPrefab, transform.position + new Vector3(-0.7f, 0.85f, 0), Quaternion.Euler(0f, 0f, 32f));
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1, 0), Quaternion.Euler(0f, 0f, 0f));
            Instantiate(_laserPrefab, transform.position + new Vector3(0.7f, 0.85f, 0), Quaternion.Euler(0f, 0f, -32f));
            Instantiate(_laserPrefab, transform.position + new Vector3(1.1f, 0.13f, 0), Quaternion.Euler(0f, 0f, -85f));
            Instantiate(_laserPrefab, transform.position + new Vector3(0.92f, -0.6f, 0), Quaternion.Euler(0f, 0f, -123f));
        }
        else
        {
            _playerAmmo--;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
        _uiManager.UpdateAmmo(_playerAmmo);
        _audioSource.clip = _laserSound ;
        _audioSource.Play();
        
    }

    public void FireEMP()
    {
        Instantiate(_playerEMP, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        _isEMPActive = false;
    }

    public void FireMissile()
    {
        Instantiate(_missilePrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        _isMissileActive = false;
    }
    public void TakeDamage()
    {
        if(_isShieldActive == true)
        {
            _shieldHealth--;
            ShieldHealth();
            
            return;
        }
        StartCoroutine("PlayerHit");
        StartCoroutine(_camShake.CamShake());
        _audioSource.clip = _playerHit;
        _audioSource.Play();
        
        _lives--;

        if(_lives == 2)
        {
            _leftWingDamage.SetActive(true);
        }
        else if(_lives == 1)
        {
            _rightWingDamage.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);
        if(_lives < 1)
        {
            spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    IEnumerator PlayerHit()
    {
        _cgl.temperature.value = 100f;
        _cgl.tint.value = 100f;
        _bloom.intensity.value = 5f;
        yield return new WaitForSeconds(0.15f);
        _cgl.temperature.value = -15f;
        _cgl.tint.value = 10f;
        _bloom.intensity.value = 3f;
    }
    public void HealthPowerup()
    {
        if(_lives == 2)
        {
            _lives++;
            _leftWingDamage.SetActive(false);
        }
        else if(_lives == 1)
        {
            _lives++;
            _rightWingDamage.SetActive(false);
        }
        _uiManager.UpdateLives(_lives);
    }
    public void ShieldHealth()
    {
        Vector3 scaler;
        switch (_shieldHealth)
        {
            case 3:
                 scaler = new Vector3(2f, 2f, 2f);
                _shieldVisualizer.transform.localScale = scaler;
                break;
            case 2:
                 scaler = new Vector3(1.75f, 1.75f, 1.75f);
                _shieldVisualizer.transform.localScale = scaler;
                break;
            case 1:
                scaler = new Vector3(1.5f, 1.5f, 1.5f);
                _shieldVisualizer.transform.localScale = scaler;
                break;
            case 0:
                _isShieldActive = false;
                _shieldVisualizer.SetActive(false);
                break;
            default:
                _isShieldActive = false;
                _shieldVisualizer.SetActive(false);
                break;
        }
    }
    public void TripleShotActivate()
    {
        _isTripleShotActive = true;
        StartCoroutine("TripleShotPowerDown");
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }
    public void SpeedActivate()
    {
        _isSpeedActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine("SpeedPowerDown");
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActivate()
    {
        _shieldHealth = 3;
        ShieldHealth();
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
    public void RefillAmmo()
    {
        _playerAmmo = 15;
        _uiManager.UpdateAmmo(_playerAmmo);
    }
    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    public void ChaosActivate()
    {
        RefillAmmo();
        _chaosGuns.SetActive(true);
        _isChaosActive = true;
        _uiManager.ChaosActive(_isChaosActive);
        _uiManager.UpdateAmmo(_playerAmmo);
        StartCoroutine("ChaosCoolDown");
    }

    IEnumerator ChaosCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _chaosGuns.SetActive(false);
        _isChaosActive = false;
        _uiManager.ChaosActive(_isChaosActive);
        _uiManager.UpdateAmmo(_playerAmmo);
    }

    public void EMPActivate()
    {
        _isEMPActive = true;
    }

    public void Damage()
    {
        TakeDamage();
        if(_playerAmmo < 5)
        {
            _playerAmmo = 0;
        }
        else
        {
            _playerAmmo -= 5;
        }
        _uiManager.UpdateAmmo(_playerAmmo);
    }

    public void MissileActivate()
    {
        _isMissileActive = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyEMP")
        {
            Destroy(other.gameObject);
            Damage();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int _powerupID; //0=tripleShot, 1=speed, 2=shields
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private AudioClip _powerupDestroyed;
    SpriteRenderer _renderer;

    private Player _player;

    private bool _isMagnetized = false;
    private bool _isPowerupActive = true;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null is powerup");
        }
    }
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (_player != null && _isMagnetized && _isPowerupActive)
        {
            float distanceToo = Vector3.Distance(transform.position, _player.transform.position);
            float playerY = _player.transform.position.y;
            float powerupY = transform.position.y;
            Vector3 powerupPos = transform.position;
            Vector3 playerPos = _player.transform.position;

            Quaternion targetRot = Quaternion.LookRotation(transform.forward, (powerupPos - playerPos));

            if (distanceToo < 4f )
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, (_speed * 1.5f) * Time.deltaTime);
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, _speed * Time.deltaTime);
        }

        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }

    public void Magnetized()
    {
        _isMagnetized = true;
    }

    public void DeMagnetize()
    {
        _isMagnetized = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActivate();
                        break;
                    case 1:
                        player.SpeedActivate();
                        break;
                    case 2:
                        player.ShieldActivate();
                        break;
                    case 3:
                        player.RefillAmmo();
                        break;
                    case 4:
                        player.HealthPowerup();
                        break;
                    case 5:
                        player.ChaosActivate();
                        break;
                    case 6:
                        player.EMPActivate();
                        break;
                    case 7:
                        player.Damage();
                        break;
                    case 8:
                        player.MissileActivate();
                        break;
                    default:
                        Debug.Log("Unknown item collected");
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);
            Destroy(this.gameObject);
        }

        if(other.tag == "EnemyLaser")
        {
            if(transform.position.y < 5f)
            {
                _isPowerupActive = false;
                StartCoroutine("FizzleOut");
                AudioSource.PlayClipAtPoint(_powerupDestroyed, transform.position);
                Destroy(other.gameObject);
            } 
        }
        
    }
    IEnumerator FizzleOut()
    {
        Color rendC = new Color(255f, 0, 0, 1);
        _renderer.color = rendC;
        yield return new WaitForSeconds(0.25f);
        rendC = new Color(255f, 0, 0, 0.75f);
        _renderer.color = rendC;
        yield return new WaitForSeconds(0.25f);
        rendC = new Color(255f, 0, 0, 0.5f);
        _renderer.color = rendC;
        yield return new WaitForSeconds(0.25f);
        rendC = new Color(255f, 0, 0, 0.25f);
        _renderer.color = rendC;
        yield return new WaitForSeconds(0.25f);
        Destroy(this.gameObject);
    }
}

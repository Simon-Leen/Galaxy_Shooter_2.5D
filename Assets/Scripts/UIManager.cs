using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Image _ammoImg;
    [SerializeField]
    private Sprite[] _ammoSprites;

    private bool _isChaosActive = false;

    [SerializeField]
    private Image _thrusterLevel;
    [SerializeField]
    private Sprite[] _thrusterSprites;
    [SerializeField]
    private Image _thrusterImg;

    private bool _isThrusterCharging = false;

    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _BossHealthText;

    [SerializeField]
    private PostProcessVolume _ppv;
    private ColorGrading _cgl;
    private Bloom _bloom;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _waveText.text = "Wave: 0";
        _BossHealthText.gameObject.SetActive(false);
        _BossHealthText.text = "Boss Health: 10";
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }
        _ppv.profile.TryGetSettings(out _cgl);
        _ppv.profile.TryGetSettings(out _bloom);
    }

    public void UpdateThrusters(float level)
    {
        if (_isThrusterCharging)
        {
            _thrusterImg.sprite = _thrusterSprites[1];
        }
        else
        {
            _thrusterImg.sprite = _thrusterSprites[0];
        }
        _thrusterLevel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, level);
    }
    public void ThrustersCharging(bool charging)
    {
        if(charging == true)
        {
            _isThrusterCharging = true;
        }
        else
        {
            _isThrusterCharging = false;
        }
    }
    public void ChaosActive(bool chaos)
    {
        if(chaos == true)
        {
            _isChaosActive = true;
        }
        else
        {
            _isChaosActive = false;
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateAmmo(int playerAmmo)
    {
        _ammoText.text =  playerAmmo+"/15";
        if(_isChaosActive == true)
        {
            _ammoImg.sprite = _ammoSprites[16];
        }
        else
        {
            _ammoImg.sprite = _ammoSprites[playerAmmo];
        }
        
    }

    public void UpdateLives(int lives)
    {
        _livesImg.sprite = _liveSprites[lives];
        if(lives == 0)
        {
            GameOverSeq(false);
        }
    }

    public void UpdateWave(int wave)
    {
        _waveText.text = "Wave: " + wave;
    }

    public void ActivateBossHealth()
    {
        _BossHealthText.gameObject.SetActive(true);
    }
    public void UpdateBossHealth(int health)
    {
        _BossHealthText.text = "Boss Health: " + health;
    }
    public void GameOverSeq( bool status)
    {
        StartCoroutine("GameOver");
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
        if(status)
        {
            _cgl.temperature.value = 100f;
            _cgl.tint.value = -60f;
            _bloom.intensity.value = 5f;
        }
    }

    IEnumerator GameOver()
    {
        while(true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }
    }
}

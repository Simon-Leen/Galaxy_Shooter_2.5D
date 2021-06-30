using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _waveText.text = "Wave: 0";
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }
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
            GameOverSeq();
        }
    }

    public void UpdateWave(int wave)
    {
        _waveText.text = "Wave: " + wave;
    }
    void GameOverSeq()
    {
        StartCoroutine("GameOver");
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
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

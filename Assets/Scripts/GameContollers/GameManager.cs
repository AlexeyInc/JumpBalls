using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public Text scoreText;
    public Text bestScore;

    public Collider2D gameZone;

    private string _lastNameKey = "PlayerNickname";
    private string _bestScoreName = "BestScore";
    private int _score;

    private List<Coroutine> gameCourutines;
    
    //сделать корректное окончание игры и меню

    private BallsManager _ballsManager;
    private BonusManager _bonusManager;
    private PlayerController _playerController;
    private LeaderBoard _leaderBoard;
    private ExplosionBonus _explosionBonus;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameCourutines = new List<Coroutine>();

        bestScore.text = "BEST: " + PlayerPrefs.GetInt(_bestScoreName);
    }

    public void Init(BallsManager ballsManager, CorridorsConductor corridorsConductor, BonusManager bonusManager, PlayerController playerController, LeaderBoard leaderBoard, ExplosionBonus explosionBonus)
    {
        _ballsManager = ballsManager;
        _ballsManager.Initialize(corridorsConductor);

        _bonusManager = bonusManager;
        _playerController = playerController;

        _leaderBoard = leaderBoard;
        _explosionBonus = explosionBonus;
    }

    private void SetupScore(int score)
    {
        _score = score;
        scoreText.text = _score.ToString();
    }

    public void StartGame()
    {
        SetupScore(0);

        _leaderBoard.LoadScoreData();

        StartGameProccess();
        //StopGameProccess();
    }

    private void StartGameProccess()
    {
        gameCourutines.Add(StartCoroutine(GameLoop()));
        gameCourutines.Add(StartCoroutine(_bonusManager.BonusesInitialization()));
    }

    private void StopGameProccess()
    {
        foreach (var c in gameCourutines)
        {
            StopCoroutine(c);
        } 
    }

    private IEnumerator GameLoop()
    {
        while (true)
        { 
            yield return new WaitForSeconds(1);
            Debug.Log("here");
            _ballsManager.ThrowBalls(2);//Random.Range(1, 5)

            yield return new WaitForSeconds(2);//Random.Range(0.25f, 3f)
        }
    }

    public void Pause()
    {
        Time.timeScale = 0; 
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }

    public void Restart()
    {
        _ballsManager.Restart();
        _playerController.Restart();
        _bonusManager.Restart();

        _leaderBoard.AddScore(_score);
        _leaderBoard.LoadScoreData();

        SetupScore(0);

        Unpause();
    }

    public void GameOver()
    {
        StopAllCoroutines();

        _leaderBoard.AddScore(_score);

        gameOverPanel.SetActive(true);
    }

    public void OpenLeaderBoard()
    {
        _leaderBoard.InitLeaderBoard();
    }

    public void UpdateScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();

        PlayerPrefs.SetInt(PlayerNickname, _score);

        if (_score > PlayerPrefs.GetInt(_bestScoreName))
        {
            bestScore.text = "BEST: " + _score;
            PlayerPrefs.SetInt(_bestScoreName, _score);
        }
    }

    public string PlayerNickname
    {
        get
        {
            if (PlayerPrefs.GetString(_lastNameKey) == "")
            {
                PlayerPrefs.SetString(_lastNameKey, "Player1");
            }
            return PlayerPrefs.GetString(_lastNameKey);
        }
        set
        {
            PlayerPrefs.SetString(_lastNameKey, value);
        }
    }

    public void ActiveGameLoop(bool isActive)
    {
        if (isActive)
        {
            StartGameProccess();
        }
        else
        {
            StopGameProccess();
        }
    }

    public void Quit()
    {
        Application.Quit();
    } 
      
    public BallsManager BallsManager
    {
        get { return _ballsManager; }
    }

    public PlayerController PlayerController
    {
        get { return _playerController; }
    }

    public BonusManager BonusManager
    {
        get { return _bonusManager; }
    }

    public ExplosionBonus ExplosionBonus
    {
        get { return _explosionBonus; }
    }  
} 
using System.Collections; 
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public Text scoreText;
    public Text bestScore;

    public GameObject[] bonuses;
    public Transform bonusSpawnPositions; 
    //private Vector3 _spawnPositions; //finish

    private string _defaultPlayerName = "Player1";
    private string _bestScoreName = "BestScore";
    private int _score;

    private BallsManager _ballsManager;
    private PlayerController _playerController;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        } 
    }

    private void Start()
    {
        PlayerPrefs.SetInt(_defaultPlayerName, 0);
        bestScore.text = "BEST: " + PlayerPrefs.GetInt(_bestScoreName);
    } 

    public void Init(BallsManager ballsManager, CorridorsConductor corridorsConductor, PlayerController playerController)
    {
        _ballsManager = ballsManager;
        _ballsManager.Initialize(corridorsConductor);

        _playerController = playerController;
    }

    private void SetupScore(int score)
    {
        _score = score;
        scoreText.text = _score.ToString();
    }

    public void StartGame()
    {
        SetupScore(0);

        StartCoroutine(GameLoop());
        StartCoroutine(BonusesInitialization());
    } 

    private IEnumerator GameLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            _ballsManager.ThrowBalls(Random.Range(1, 5));

            yield return new WaitForSeconds(Random.Range(0.25f, 3f));
        } 
    }

    private IEnumerator BonusesInitialization()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            int bonusIndx = Random.Range(0, bonuses.Length);
            int spawnPosIndx = Random.Range(0, bonusSpawnPositions.Length);
            Instantiate(bonuses[bonusIndx], bonusSpawnPositions[spawnPosIndx], false);

            yield return new WaitForSeconds(Random.Range(5,9));
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

        SetupScore(0);

        Unpause();
    }

    public void GameOver()
    { 
        gameOverPanel.SetActive(true);
    }

    public void UpdateScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();

        PlayerPrefs.SetInt(_defaultPlayerName, _score);

        if (_score > PlayerPrefs.GetInt(_bestScoreName))
        {
            bestScore.text = "BEST: " + _score;
            PlayerPrefs.SetInt(_bestScoreName, _score);
        }
    }
     
    public void SetupBonus(GameObject ballObj, BonusUpgradeType ballUpgradeType)
    {
        switch (ballUpgradeType)
        {
            case BonusUpgradeType.Level:
                _ballsManager.UpgrateBallLevel(ballObj);
                break;

            case BonusUpgradeType.BallSize:
                _ballsManager.UpgrateBallSize(ballObj);
                break;

            case BonusUpgradeType.ExtraBall:
                _ballsManager.CreateExtraBall(ballObj);
                break;

            case BonusUpgradeType.PlatformWidth:
                _playerController.IncreasePlatformWidth();
                break;

            default:
                Debug.Log("Something go wrong...");
                break;
        }
    }

    public BallsManager BallsManager
    {
        get { return _ballsManager; }
    }

    //public PlayerController PlayerController
    //{
    //    get { return _playerController; }
    //}
} 
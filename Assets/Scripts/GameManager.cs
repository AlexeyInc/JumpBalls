using System.Collections; 
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public Text scoreText;
    public Text bestScore;

    public Transform bonusSpawn;
    public GameObject[] bonuses;

    string _defaultPlayerName = "Player1";
    string _bestScoreName = "BestScore";
    int _score;
     
    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }

        PlayerPrefs.SetInt(_defaultPlayerName, 0);
        bestScore.text = "BEST: " + PlayerPrefs.GetInt(_bestScoreName);
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

            BallsManager.Instance.ThrowBalls(Random.Range(1, 6));

            yield return new WaitForSeconds(Random.Range(0.1f, 3f));
        } 
    }

    private IEnumerator BonusesInitialization()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            int bonusIndx = Random.Range(0, bonuses.Length);
            Instantiate(bonuses[bonusIndx], bonusSpawn, false);

            yield return new WaitForSeconds(Random.Range(9,12));
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
        BallsManager.Instance.Restart();

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
} 
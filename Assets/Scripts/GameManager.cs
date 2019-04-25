using System.Collections; 
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text scoreText;

    public Transform bonusSpawn;
    public GameObject[] bonuses;

    string _defaultPlayerName = "Player_Alex";
    int _score;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }

        PlayerPrefs.SetInt(_defaultPlayerName, 0);
    }

    void Start()
    {
        SetupScore();

        StartCoroutine(GameLoop());
        StartCoroutine(BonusesInitialization());
    } 

    private void SetupScore()
    {
        _score = PlayerPrefs.GetInt(_defaultPlayerName, 0);
        scoreText.text = _score.ToString();
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            BallsManager.Instance.ThrowBalls(Random.Range(1, 4));

            yield return new WaitForSeconds(1f);
        } 
    }

    private IEnumerator BonusesInitialization()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            //int bonusIndx = Random.Range(0, bonuses.Length);
            Instantiate(bonuses[0], bonusSpawn, true);

            yield return new WaitForSeconds(10f);
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

    public void NewGame()
    {
        Debug.Log("Start new game!");
    }

    public void UpdateScore(int value)
    {
        _score += value;
        scoreText.text = _score.ToString();

        PlayerPrefs.SetInt(_defaultPlayerName, _score);
    }
} 
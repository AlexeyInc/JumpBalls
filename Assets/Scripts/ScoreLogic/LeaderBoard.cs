using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public Transform ScoreContainer;
    public GameObject ScorePrefab;

    const string scoreDataFileName = "Score.json";

    private List<Score> _scoreList; 
    private ScoreData _scoreData;

    private int _maxCountScores = 15;

    private void Start()
    {
        SetupLeaderBoard();
    } 

    public void SetupLeaderBoard()
    { 
        LoadScoreData();
        InitLeaderBoard();
    }

    private void LoadScoreData()
    {
        string filePath = Application.persistentDataPath + scoreDataFileName;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            _scoreData = JsonUtility.FromJson<ScoreData>(dataAsJson);
        }
        else
        {
            _scoreData = new ScoreData();
        }

        _scoreList = _scoreData.scoreData?.ToList();
    }
     
    private void SaveScoreData()
    {
        _scoreData.scoreData = _scoreList.Count > _maxCountScores ? 
                                _scoreList.Take(_maxCountScores).ToArray() : 
                                _scoreList.ToArray();

        string dataAsJson = JsonUtility.ToJson(_scoreData);

        string filePath = Application.persistentDataPath + scoreDataFileName;
        File.WriteAllText(filePath, dataAsJson);
    }

    private void InitLeaderBoard()
    {
        if (_scoreList != null)
        {
            ClearOldScores();
             
            for (int i = 0; i < _scoreList.Count && i < _maxCountScores; i++)
            {
                GameObject score = Instantiate(ScorePrefab, ScoreContainer);
                 
                Text[] scoreLines = score.GetComponentsInChildren<Text>();
                scoreLines[0].text = (i + 1).ToString();
                scoreLines[1].text = _scoreList[i].Nickname;
                scoreLines[2].text = _scoreList[i].Points.ToString(); 
            }
        }
    }

    private void ClearOldScores()
    { 
        for (int i = 0; i < ScoreContainer.childCount; i++)
        {
            Destroy(ScoreContainer.GetChild(i).gameObject);
        }
    }

    public void AddScore(int score)
    { 
        string nickName = GameManager.Instance.PlayerNickname;
        Score newScore = new Score() { Nickname = nickName, Points = score };

        if (_scoreList == null)
        {
            _scoreList = new List<Score>();
            _scoreList.Add(newScore);

            SaveScoreData(); 
        }
        else
        { 
            for (int i = 0; i < _scoreList.Count; i++)
            {
                if (_scoreList[i].Points <= score)
                {
                    _scoreList.Insert(i, newScore);
                     
                    SaveScoreData();
                    return;
                }
            }
            if (_scoreList.Count < _maxCountScores)
            {
                _scoreList.Insert(_scoreList.Count - 1, newScore);

                SaveScoreData();
            }
        }
    }
}

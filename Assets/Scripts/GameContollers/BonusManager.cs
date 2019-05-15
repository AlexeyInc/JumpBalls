using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
    public GameObject bonusContainer;
    public GameObject[] bonuses;
    public Transform bonusSpawnPositions;
    public GameObject TextAnimation;

    private List<Bonus> _bonusesList;
    private Dictionary<Vector3, bool> _occupiedPos; 
    private Vector3[] _bonusPos; 
    private Dictionary<BonusUpgradeType, string> _bonusText;
     
    void Start()
    { 
        _bonusText = new Dictionary<BonusUpgradeType, string>();

        _bonusesList = new List<Bonus>();
        for (int i = 0; i < bonuses.Length; i++)
        {
            _bonusesList.Add(bonuses[i].GetComponentInChildren<Bonus>());
        }

        Init();
    }
     
    private void Init()
    { 
        Transform[] positions = bonusSpawnPositions.GetComponentsInChildren<Transform>().Skip(1).ToArray();

        _occupiedPos = new Dictionary<Vector3, bool>();
        _bonusPos = new Vector3[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            _occupiedPos.Add(positions[i].position, false);
            _bonusPos[i] = positions[i].position;
        }

        //-----------------------------------------------------------------

        _bonusText.Add(BonusUpgradeType.ExtraBall, "Extra balls!");
        _bonusText.Add(BonusUpgradeType.BallSize, "Size increased!");
        _bonusText.Add(BonusUpgradeType.Level, "Level UP!");
        _bonusText.Add(BonusUpgradeType.PlatformWidth, "Platform increased!");
        _bonusText.Add(BonusUpgradeType.ExplosionBalls, "Blow up balls!");
    }

    public IEnumerator BonusesInitialization()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            int bonusIndx = Random.Range(0, bonuses.Length);//

            if (!CanExecuteBonus(bonusIndx))
            {
                continue;
            }

            int spawnPosIndx;
            do
            {
                spawnPosIndx = Random.Range(0, _bonusPos.Length);
                yield return new WaitForEndOfFrame();

            } while (_occupiedPos[_bonusPos[spawnPosIndx]]);
             
            GameObject newBonus = Instantiate(bonuses[bonusIndx].gameObject, _bonusPos[spawnPosIndx], Quaternion.identity);//
            newBonus.transform.parent = bonusContainer.transform;

            _occupiedPos[_bonusPos[spawnPosIndx]] = true;

            yield return new WaitForSeconds(Random.Range(6, 9));//
        }
    }

    private bool CanExecuteBonus(int indx)
    {
        if (_bonusesList[indx].bonusUpgradeType == BonusUpgradeType.ExplosionBalls)
        {
            if (!GameManager.Instance.BallsManager.IsEnaughBallsOnGround())
            { 
                return false;
            }
            else
            { 
                return true;
            }
        }
        return true;
    }

    public void SetupBonusFor(GameObject ballObj, GameObject bonusObj, BonusUpgradeType ballUpgradeType)
    {
        switch (ballUpgradeType)
        {
            case BonusUpgradeType.Level:
                GameManager.Instance.BallsManager.UpgrateBallLevel(ballObj);
                break;

            case BonusUpgradeType.BallSize: 
                GameManager.Instance.BallsManager.UpgrateBallSize(ballObj); 
                DestroyByTime(bonusObj, 0f, true);
                break;

            case BonusUpgradeType.ExtraBall:
                GameManager.Instance.BallsManager.CreateExtraBall(ballObj);
                break;

            case BonusUpgradeType.PlatformWidth:
                GameManager.Instance.PlayerController.IncreasePlatformWidth(); 
                DestroyByTime(bonusObj, 0f, true);
                break;

            case BonusUpgradeType.ExplosionBalls: 
                GameManager.Instance.ExplosionBonus.MakeExplosion();
                DestroyByTime(bonusObj, 0f, true);
                break;

            default:
                Debug.Log("No bonuses found...");
                break;
        }
    }

    public void InitBonusTextAnim(Vector3 position, BonusUpgradeType bonusUpgradeType)
    {
        string bonusText = _bonusText[bonusUpgradeType];

        Vector3 startPos = new Vector3(position.x, position.y + 0.3f, 0);

        GameObject bonusTextAnim = Instantiate(TextAnimation, startPos, Quaternion.identity);
        Text textAnim = bonusTextAnim.GetComponentInChildren<Text>();
        textAnim.text = bonusText;

        DestroyByTime(bonusTextAnim, 2.5f); 
    }

    public void Restart()
    {
        int countBonuses = bonusContainer.transform.childCount;
        for (int i = 0; i < countBonuses; i++)
        {
            DestroyByTime(bonusContainer.transform.GetChild(0).gameObject, 0f, true);
        }

        //foreach (var item in _occupiedPos.Keys.ToList())
        //{
        //    _occupiedPos[item] = false;
        //}
    }
     
    public void DestroyByTime(GameObject gameObject, float time, bool isBonus = false)
    {
        if (isBonus)
        {
            _occupiedPos[gameObject.transform.position] = false;
        }
         
        Destroy(gameObject, time);
    }
}

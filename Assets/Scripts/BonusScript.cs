 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusScript : MonoBehaviour
{
    public BonusUpgradeType bonusUpgradeType;
    public GameObject TextAnimation;

    private Animator _animator;
    private Dictionary<BonusUpgradeType, string> _bonusText;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _bonusText = new Dictionary<BonusUpgradeType, string>();

        Init();
    }

    private void Init()
    {
        _bonusText.Add(BonusUpgradeType.ExtraBall, "Extra balls!"); 
        _bonusText.Add(BonusUpgradeType.Scale, "Size increased!"); 
        _bonusText.Add(BonusUpgradeType.Color, "Level UP!");
    }
     
    private void OnTriggerEnter2D(Collider2D other)
    { 
        string objLayer = LayerMask.LayerToName(other.gameObject.layer);

        if (objLayer == "FlyingBall")
        { 
            BallsManager.Instance.ExecBallBonus(other.gameObject, bonusUpgradeType);

            CheckBonusBehavior();
        }
    }

    private void CheckBonusBehavior()
    { 
        string bonusText = _bonusText[bonusUpgradeType];

        Vector3 startPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, 0);
        InitFlyTextBonusAnim(startPos, bonusText);

        if (bonusUpgradeType == BonusUpgradeType.Scale)
        {
            DestroyTime(this.gameObject, 0f);
        }
    }

    private void InitFlyTextBonusAnim(Vector3 position, string text)
    {
        GameObject bonusTextAnim = Instantiate(TextAnimation, position, Quaternion.identity); 
        Text textAnim = bonusTextAnim.GetComponentInChildren<Text>(); 
        textAnim.text = text;

        DestroyTime(bonusTextAnim, 2f);
    }

    public void DestroyTime(GameObject gameObject, float time)
    {
        Destroy(gameObject, time);
    }

    public void DestroyByAnimLifeTime()
    { 
        Destroy(this.gameObject);
    }
}

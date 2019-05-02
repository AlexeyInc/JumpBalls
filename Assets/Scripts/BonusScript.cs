using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BonusUpgradeType
{
    Level, BallSize, PlatformWidth, ExtraBall
}

public class BonusScript : MonoBehaviour
{
    public BonusUpgradeType bonusUpgradeType;
    public GameObject TextAnimation;
    public float scaleSpeed;

    private Animator _animator;
    private Dictionary<BonusUpgradeType, string> _bonusText;

    private RectTransform _rectTransform;
    private Vector2 _startSizeDelta;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _bonusText = new Dictionary<BonusUpgradeType, string>();
        _rectTransform = GetComponent<RectTransform>();
        _startSizeDelta = _rectTransform.sizeDelta; 

        Init();
    }

    private void Init()
    {
        _bonusText.Add(BonusUpgradeType.ExtraBall, "Extra balls!"); 
        _bonusText.Add(BonusUpgradeType.BallSize, "Size increased!"); 
        _bonusText.Add(BonusUpgradeType.Level, "Level UP!");
        _bonusText.Add(BonusUpgradeType.PlatformWidth, "Platform increased!");
    }
     
    private void OnTriggerEnter2D(Collider2D other)
    { 
        string objLayer = LayerMask.LayerToName(other.gameObject.layer);

        if (objLayer == "FlyingBall")
        {
            GameManager.Instance.SetupBonus(other.gameObject, bonusUpgradeType);

            StartCoroutine(PickUpAnim());
            CheckBonusBehavior();
        }
    }

    private IEnumerator PickUpAnim()
    {
        if (_startSizeDelta == _rectTransform.sizeDelta)
        {
            Vector2 targetSizeDelta = _startSizeDelta + new Vector2(5, 5);

            while (_rectTransform.sizeDelta != targetSizeDelta)
            {
                _rectTransform.sizeDelta = Vector3.MoveTowards(_rectTransform.sizeDelta, targetSizeDelta, Time.smoothDeltaTime * scaleSpeed);

                yield return new WaitForSeconds(0.025f);
            }

            while (_rectTransform.sizeDelta != _startSizeDelta)
            {
                _rectTransform.sizeDelta = Vector3.MoveTowards(_rectTransform.sizeDelta, _startSizeDelta, Time.smoothDeltaTime * scaleSpeed);

                yield return new WaitForSeconds(0.015f);
            }
        }
    }

    private void CheckBonusBehavior()
    { 
        string bonusText = _bonusText[bonusUpgradeType];

        Vector3 startPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.3f, 0);
        InitBonusTextFlyAnim(startPos, bonusText);

        if (bonusUpgradeType == BonusUpgradeType.BallSize || bonusUpgradeType == BonusUpgradeType.PlatformWidth)
        {
            DestroyTime(this.gameObject.transform.parent.gameObject, 0f);
        }
    }

    private void InitBonusTextFlyAnim(Vector3 position, string text)
    {
        GameObject bonusTextAnim = Instantiate(TextAnimation, position, Quaternion.identity); 
        Text textAnim = bonusTextAnim.GetComponentInChildren<Text>(); 
        textAnim.text = text;

        DestroyTime(bonusTextAnim, 2.5f);
    }

    public void DestroyTime(GameObject gameObject, float time)
    {
        Destroy(gameObject, time);
    }

    public void DestroyByAnimLifeTime()
    { 
        Destroy(this.gameObject.transform.parent.gameObject);
    }


}

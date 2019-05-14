using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BonusUpgradeType
{
    Level, BallSize, PlatformWidth, ExtraBall, ExplosionBalls
}

public class Bonus : MonoBehaviour
{
    public BonusUpgradeType bonusUpgradeType;
    public float scaleSpeed;
     
    private RectTransform _rectTransform;
    private Vector2 _startSizeDelta;

    private GameObject _thisBonusObj;

    private void Start()
    { 
        _rectTransform = GetComponent<RectTransform>();
        _startSizeDelta = _rectTransform.sizeDelta;

        _thisBonusObj = this.gameObject.transform.parent.gameObject;
    }
     
    private void OnTriggerEnter2D(Collider2D other)
    {  
        if (other.tag == "Ball")
        {
            string objLayer = LayerMask.LayerToName(other.gameObject.layer);

            //second condition allowing extra ball catch all bonuses exept "ExtraBall" again, coz extra ball have "SolidBall" Layer
            if (objLayer == "FlyingBall" || GameManager.Instance.BallsManager[other.gameObject].ExtraBallBonus) 
            {
                StartCoroutine(PickUpAnim());

                GameManager.Instance.BonusManager.SetupBonusFor(other.gameObject, _thisBonusObj, bonusUpgradeType);
                GameManager.Instance.BonusManager.InitBonusTextAnim(this.transform.position, bonusUpgradeType);
            }
        } 
    }

    private IEnumerator PickUpAnim()
    {
        if (_startSizeDelta == _rectTransform.sizeDelta)
        {
            Vector2 targetSizeDelta = _startSizeDelta + new Vector2(6f, 6f);

            while (_rectTransform.sizeDelta != targetSizeDelta)
            {
                _rectTransform.sizeDelta = Vector3.MoveTowards(_rectTransform.sizeDelta, targetSizeDelta, Time.smoothDeltaTime * scaleSpeed);

                yield return new WaitForSeconds(0.0125f);
            }

            while (_rectTransform.sizeDelta != _startSizeDelta)
            {
                _rectTransform.sizeDelta = Vector3.MoveTowards(_rectTransform.sizeDelta, _startSizeDelta, Time.smoothDeltaTime * scaleSpeed);

                yield return new WaitForSeconds(0.015f);
            }
        }
    }

    public void DestroyByAnimLifeTime()
    {
        GameManager.Instance.BonusManager.DestroyByTime(_thisBonusObj, 0f, true);
    } 
}

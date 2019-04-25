using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusScript : MonoBehaviour
{
    public BonusUpgradeType bonusUpgradeType;
    public GameObject TextAnimation;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            BallsManager.Instance.UpdateBall(other.gameObject, bonusUpgradeType);

            CheckBonusBehavior();
        }
    }

    private void CheckBonusBehavior()
    {
        if (bonusUpgradeType == BonusUpgradeType.Color)
        {
            _animator.SetTrigger("PickUp");
        }
        else if (bonusUpgradeType == BonusUpgradeType.Scale || bonusUpgradeType == BonusUpgradeType.BonusBall)
        {
            string bonusText = bonusUpgradeType == BonusUpgradeType.Scale ? "Ball size increased!" : "Added bonus balls!";
            InitFlyTextBonusAnim(this.transform.position, bonusText);

            Destroy(this.gameObject);
        } 
    }

    private void InitFlyTextBonusAnim(Vector3 position, string text)
    {
        GameObject bonusTextAnim = Instantiate(TextAnimation, position, Quaternion.identity); 
        Text textAnim = bonusTextAnim.GetComponentInChildren<Text>(); 
        textAnim.text = text;

        Debug.Log("InitFlyTextBonusAnim");
        Destroy(bonusTextAnim, 2f);
    }

    public void Suicide()
    { 
        Destroy(this.gameObject);
    }
}

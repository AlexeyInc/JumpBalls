 using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    public GameObject TextAnimation;

    private Text _scoreText;
     
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            Color color = GameManager.Instance.BallsManager[other.gameObject].Color;
            int addPoints = GameManager.Instance.BallsManager[other.gameObject].Points;

            GameManager.Instance.UpdateScore(addPoints);

            InitFlyScoreAnim(position:  new Vector2(other.transform.position.x, this.transform.position.y),
                             textColor: color,
                             points:    addPoints); 
        }
    } 

    private void InitFlyScoreAnim(Vector3 position, Color textColor, int points)
    {
        GameObject scoreTextAnim = Instantiate(TextAnimation, position, Quaternion.identity);
        scoreTextAnim.transform.SetParent(this.transform);
        _scoreText = scoreTextAnim.GetComponentInChildren<Text>();
        _scoreText.color = textColor;
        _scoreText.text = "+" + points;

        Destroy(scoreTextAnim, 2f);
    }
}

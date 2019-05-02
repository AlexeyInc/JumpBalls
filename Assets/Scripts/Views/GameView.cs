using UnityEngine;

public class GameView : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public BallsManager BallsManager;
    public CorridorsConductor CorridorsConductor; 

    private void Start()
    {
        Instantiate(PlayerPrefab);
        PlayerController playerController = PlayerPrefab.GetComponent<PlayerController>();

        GameManager.Instance.Init(BallsManager, CorridorsConductor, playerController);
    } 
}

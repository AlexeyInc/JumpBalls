using UnityEngine;

public class GameView : MonoBehaviour
{ 
    public BallsManager BallsManager;
    public BonusManager BonusManager;

    public GameObject PlayerPrefab;
    public CorridorsConductor CorridorsConductor;

    public LeaderBoard LeaderBoard;
    public ExplosionBonus ExplosionBonus; 

    private void Start()
    {
        GameObject player = Instantiate(PlayerPrefab);
        PlayerController PlayerController = player.GetComponent<PlayerController>();

        GameManager.Instance.Init(BallsManager, 
                                  CorridorsConductor, 
                                  BonusManager,
                                  PlayerController, 
                                  LeaderBoard, 
                                  ExplosionBonus);
    } 
}

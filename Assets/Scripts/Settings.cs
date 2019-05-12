using UnityEngine;
using UnityEngine.UI;
 
public class Settings : MonoBehaviour
{
    public InputField inputPlayerNickname;
     
    private void Start()
    {
        SetPlayerNickname(GameManager.Instance.PlayerNickname);
        inputPlayerNickname.onEndEdit.AddListener(ChangeUserNickname);
         
    }

    public void ChangeUserNickname(string newNickname)
    {
        GameManager.Instance.PlayerNickname = newNickname;
        SetPlayerNickname(newNickname);
    }

    private void SetPlayerNickname(string nickname)
    {
        inputPlayerNickname.text = nickname;
    }
}

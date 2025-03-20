using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ResultNetwork : MonoBehaviourPunCallbacks
{
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainScene");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("StartMenu");
    }
}

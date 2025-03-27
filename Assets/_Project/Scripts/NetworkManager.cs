using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public CanvasGroup m_MenuUI;
    public GameObject m_ConnectingObj;
    public GameObject m_MatchmakingObj;

    void Start()
    {
        m_MenuUI.interactable = false;
        m_ConnectingObj.SetActive(true);
        m_MatchmakingObj.SetActive(false);

        // Hier gaan we eerst verbinden
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1.0"; // we verbinden alleen met games die dezelfde versie hebben
        }
        else
        {
            m_MenuUI.interactable = true;
            m_ConnectingObj.SetActive(false);
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        m_MenuUI.interactable = true;
        m_ConnectingObj.SetActive(false);
    }

    // HOST
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("Test", new Photon.Realtime.RoomOptions() { MaxPlayers = 2 }); // we maken een nieuwe room, max 2 spelers
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        // als de room die we maken gemaakt is, dan gaan we ingame
        // we gebruiken hier de 'gewone' load scene, want we zijn host dus alles is nieuw
        SceneManager.LoadScene("Game");
    }

    // PLAYER
    public void JoinRoom()
    {
        // we gaan een room vinden, en anders een nieuwe maken
        m_MenuUI.interactable = true;
        m_ConnectingObj.SetActive(false);

        PhotonNetwork.JoinOrCreateRoom("Test", new Photon.Realtime.RoomOptions() { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // als de room gevonden hebben, dan gaan we ingame
        // we gebruiken hier networked load scene, zodat we andere spelers kunnen ontvangen
        PhotonNetwork.LoadLevel("Game");
    }

    // ISSUES
    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        base.OnErrorInfo(errorInfo);

        // is er een probleem? Dan zorgen we ervoor dat de speler in ieder geval iets kan doen.
        m_MenuUI.interactable = true;
        m_ConnectingObj.SetActive(false);
        m_MatchmakingObj.SetActive(false);
    }
}

using Photon.Pun;
using UnityEngine;

public class SimpleLauncher : MonoBehaviourPunCallbacks
{
    public PhotonView m_PlayerPrefab;
    [SerializeField] Canvas m_canvas;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomOrCreateRoom(); // dees naar start game button
    }

    public override void OnJoinedRoom() // dees in de start van game scene
    {
        Debug.Log("Joined a room.");
        PhotonNetwork.Instantiate(m_PlayerPrefab.name, Vector3.zero, Quaternion.identity);
        m_canvas.gameObject.SetActive(true);
    }
}
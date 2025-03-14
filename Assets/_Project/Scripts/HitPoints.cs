using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitPoints : MonoBehaviourPun
{
    public int m_HP = 10;
    public PhotonView m_view;
    [SerializeField] private GameManager m_gameManager;

    private void Start()
    {
        m_view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        DestroyOnKill();
        GetGameManager();
    }

    [PunRPC]
    public void ManageOtherPlayer()
    {
        if (!photonView.IsMine) return;

        m_gameManager.m_GameSettings.m_OutCome = 1;
        SceneManager.LoadScene("GameOver");

        PhotonNetwork.Disconnect();
        //PhotonNetwork.LeaveRoom();
    }

    protected void DestroyOnKill()
    {
        if (m_HP <= 0)
        {
            if (gameObject.CompareTag("Player") && photonView.IsMine)
            {
                m_view.RPC("ManageOtherPlayer", RpcTarget.Others);
                m_gameManager.m_GameSettings.m_OutCome = 2;
                SceneManager.LoadScene("GameOver");

                PhotonNetwork.Disconnect();
                //PhotonNetwork.LeaveRoom();
            }

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    protected void GetGameManager()
    {
        if (m_gameManager == null)
        {
            m_gameManager = FindObjectOfType<GameManager>();
        }
    }
}

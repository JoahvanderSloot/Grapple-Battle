using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitPoints : MonoBehaviour
{
    public int m_HP = 10;
    public PhotonView m_view;
    GameManager m_gameManager;

    private void Start()
    {
        m_view = GetComponent<PhotonView>();
        m_gameManager = GameManager.FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        DestroyOnKill();
    }

    [PunRPC]
    public void ManageOtherPlayer()
    {
        m_gameManager.m_GameSettings.m_OutCome = 1;
        SceneManager.LoadScene("GameOver");
    }

    protected void DestroyOnKill()
    {
        if (m_HP <= 0)
        {
            if(gameObject.tag == "Player")
            {
                m_gameManager.m_GameSettings.m_OutCome = 2;
                SceneManager.LoadScene("GameOver");
                m_view.RPC("ManageOtherPlayer", RpcTarget.Others);
            }
            Destroy(gameObject);
        }
    }
}

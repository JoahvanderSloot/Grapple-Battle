using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitPoints : MonoBehaviourPun
{
    public int m_HP = 10;

    private void Update()
    {
        DestroyOnKill();

        if(GameManager.Instance.m_GameSettings.m_GameTimer <= 0 && PhotonNetwork.IsMasterClient)
        {
            OnDraw();
        }
    }

    public void Timer()
    {
        GameManager.Instance.m_GameSettings.m_GameTimer--;
        photonView.RPC("SetTimer", RpcTarget.Others, GameManager.Instance.m_GameSettings.m_GameTimer);
    }

    [PunRPC]
    public void SetTimer(int _gameTime)
    {
        GameManager.Instance.m_GameSettings.m_GameTimer = _gameTime;
    }

    [PunRPC]
    public void ManageOtherPlayer()
    {
        if (!photonView.IsMine) return;

        GameManager.Instance.m_GameSettings.m_OutCome = 1;
        SceneManager.LoadScene("GameOver");
    }

    protected void DestroyOnKill()
    {
        if (m_HP <= 0)
        {
            if (gameObject.CompareTag("Player") && photonView.IsMine)
            {
                OnKilled();
            }
        }
    }

    public void OnKilled()
    {
        photonView.RPC("SetWinner", RpcTarget.Others, true);
        SetWinner(false);
    }

    public void OnDraw()
    {
        photonView.RPC("SetDraw", RpcTarget.Others);
        SetDraw();
    }


    [PunRPC]
    public void SetWinner(bool _isWinner)
    {
        GameManager.Instance.IsResult = true;
        GameManager.Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = _isWinner ? "YOU WIN!" : "YOU LOSE!";
        GameManager.Instance.ResultObj.SetActive(true);
    }

    [PunRPC]
    public void SetDraw()
    {
        GameManager.Instance.IsResult = true;
        GameManager.Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = "DRAW!";
        GameManager.Instance.ResultObj.SetActive(true);
    }
}

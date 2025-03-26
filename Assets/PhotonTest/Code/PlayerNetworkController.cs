using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerNetworkController : MonoBehaviourPunCallbacks
{
    //public float MovementSpeed = 1f;

    //// Update is called once per frame
    //void Update()
    //{
    //    if (photonView.IsMine)
    //    {
    //        // geen UI zichtbaar? we kunnen bewegen
    //        if (!GameManager.Instance.IsPaused && !GameManager.Instance.IsResult)
    //            transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * MovementSpeed, 0, Input.GetAxis("Vertical") * Time.deltaTime * MovementSpeed);
    //    }
    //}

    //[PunRPC]
    //public void OnKilled()
    //{
    //    // doe dingen
    //    photonView.RPC("SetWinner", RpcTarget.Others, true);
    //    SetWinner(false);
    //}

    //[PunRPC]
    //public void OnDraw()
    //{
    //    // doe dingen
    //    photonView.RPC("SetDraw", RpcTarget.Others);
    //    SetDraw();
    //}


    //[PunRPC]
    //public void SetWinner(bool _isWinner)
    //{
    //    GameManager.Instance.IsResult = true;
    //    GameManager.Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = _isWinner ? "YOU WIN!" : "YOU LOSE!";
    //    GameManager.Instance.ResultObj.SetActive(true);
    //}

    //public void SetDraw()
    //{
    //    GameManager.Instance.IsResult = true;
    //    GameManager.Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = "DRAW!";
    //    GameManager.Instance.ResultObj.SetActive(true);
    //}
}
